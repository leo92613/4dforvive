using UnityEngine;
using System;
using System.IO;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;

public enum SynchronizerType {
	Server,
	Client,
	Recorder,
	Playback
}

public class LineSynchronizer: MonoBehaviour {
	public SynchronizerType type = SynchronizerType.Client;

	// SERVER
	private string headsetID = "";
	
	// RECORDING
	public float recordedFPS = 1f; // record once per second
	public static string recordPath = "recorded_frames";
	private static string LastFrameNumberPath { get { return Path.Combine(recordPath, "frame_count"); } }
	
	private int MillisecondsPerFrame {
		get { return (int)(1000f / recordedFPS); }
	}
	private long CurrentMilliseconds {
		get { return DateTime.UtcNow.Ticks / TimeSpan.TicksPerMillisecond; }
	}
	private long lastRecordedFrameTime;
	
	// CLIENT
	public bool receivedData = false;
	private Socket clientSock;
	
	// ALL
	public LineUnity[] lines;
	
	public makeLine[] CurrentMakeLines {
		get {
			var ml = new makeLine[lines.Length];
			for (var i = 0; i < ml.Length; i++) {
				var l = lines[i].lines;
				if (l.Count < 1) {
					lines[i].addLine();
				}
				ml[i] = l[0].GetComponent<makeLine>();
			}
			return ml;
		}
	}
	
	private const int BACKFILL_PORT = 7345;
	
	void Start() {
		if (type == SynchronizerType.Client) {
			clientSock = new Socket(AddressFamily.InterNetwork,
			                        SocketType.Stream, ProtocolType.Tcp);
			var ipEnd = new IPEndPoint(IPAddress.Any, BACKFILL_PORT);
			clientSock.Bind(ipEnd);
			clientSock.Listen(100);
		}
		if (type == SynchronizerType.Recorder) {
			if (Directory.Exists(recordPath)) {
				//Directory.Delete(recordPath, true);
			}
			Directory.CreateDirectory(recordPath);
			using (var s = File.OpenWrite(LastFrameNumberPath)) {
				var b = new BinaryWriter(s);
				b.Write(0u);
			}
		}
	}
	
	void Update() {
		if (type == SynchronizerType.Recorder &&
		    (CurrentMilliseconds - lastRecordedFrameTime) > MillisecondsPerFrame) {
			RecordFrame();
		}
		if (type == SynchronizerType.Server) {
			return;
		}
		
		// touch the screen to skip receiving data
		receivedData |= type == SynchronizerType.Client && Input.touchCount > 0;
		
		if (type == SynchronizerType.Client) {
			ReceiveClientState();
			if (receivedData) {
				if (clientSock != null) {
					clientSock.Close();
				}
				// remove recorder once data received
				UnityEngine.Object.Destroy(gameObject);
			}
		}
	}
	
	void RecordFrame() {
		// RECORD FRAME
		lastRecordedFrameTime = CurrentMilliseconds;
		var frame = LastRecordedFrameNumber(true);
		using (var s = File.Open(Path.Combine(recordPath, "frame" + frame),
		                         FileMode.OpenOrCreate)) {
			s.Seek(0, SeekOrigin.Begin);
			var ml = CurrentMakeLines;
			s.WriteLineFrame(ref ml);
		}
	}
	
	// returns a success value
	public bool LoadRecordedFrame(UInt32 frame) {
		var path = Path.Combine(recordPath, "frame" + frame);
		if (!File.Exists(path)) {
			UnityEngine.Debug
				.LogWarning("Tried to load frame (" +
				            path +
				            ") which doesn't exist.");
			return false;
		}
		GetFromFile(path);
		return true;
	}
	
	public static UInt32 LastRecordedFrameNumber(bool increment = false) {
		UInt32 frame;
		using (var s = File.Open(LastFrameNumberPath,
		                         FileMode.Open)) {
			var reader = new BinaryReader(s);
			frame = reader.ReadUInt32();
			if  (increment) {
				s.Seek(0, SeekOrigin.Begin);
				frame++;
				var writer = new BinaryWriter(s);
				writer.Write(frame);
			}
		}
		return frame;
	}
	
	// MARK: Data Marshalling
	
	void UpdateMakeLines(Stream s, ref makeLine[] mlines) {
		var frame = s.ReadLineFrame();
		for(var i = 0; i < mlines.Length; i++) {
			foreach (var l in frame[i]) {
				foreach (var p in l.points) {
					var cell = mlines[i].getHashedCell(p.pos);
					if (!mlines[i].hashGrid.ContainsKey(cell)) {
						mlines[i].hashGrid[cell] = new List<makeLine.Point>();
					}
					mlines[i].hashGrid[cell].Add(p);
				}
			}
			mlines[i].lines = frame[i];
			lines[i].lines[0] = mlines[i].gameObject;
			mlines[i].makeTexture();
			//mlines[i].rebuildLine();
		}
	}
	
	void ReadLines(Stream s) {
		var ml = CurrentMakeLines;
		UpdateMakeLines(s, ref ml);
	}
	
	// MARK: Network IO
	void ReceiveClientState() {
		var readable = new ArrayList();
		const int POLL_TIME = 50000; // 0.05 seconds
		readable.Add(clientSock);
		Socket.Select(readable, null, null, POLL_TIME);
		if (readable.Count == 0) return;
		
		var handler = clientSock.Accept();
		var stop = Stopwatch.StartNew();
		using (var stream = new NetworkStream(handler)) {
			// Read device ID header
			var reader = new BinaryReader(stream);
			char header = reader.ReadChar(); // header as 1 byte number
			// FIXME: set the ID here

			ReadLines(stream);
		}
		handler.Close();
		clientSock.Close();
		stop.Stop();
		UnityEngine.Debug.Log(string.Format("Receiving lines took {0} milliseconds", stop.ElapsedMilliseconds));
		receivedData = true;
	}
	
	void Serve() {
		// port forwarding
		var proc = new Process();
		proc.StartInfo.UseShellExecute = true;
		proc.StartInfo.FileName = "adb";
		proc.StartInfo.Arguments = string.Format("forward tcp:{0} tcp:{0}",
		                                         BACKFILL_PORT);
		proc.StartInfo.CreateNoWindow = true;
		proc.Start();
		
		var sock = new Socket(AddressFamily.InterNetwork,
		                      SocketType.Stream, ProtocolType.Tcp);
		var ipEnd = new IPEndPoint(IPAddress.Parse("127.0.0.1"),
		                           BACKFILL_PORT);
		UnityEngine.Debug.Log("Sending lines...");
		sock.Connect(ipEnd);
		var ml = CurrentMakeLines;
		using (var stream = new NetworkStream(sock)) {
			// Write a header to indicate device ID
			ushort HID;
			if (!UInt16.TryParse(headsetID, out HID) && HID <= char.MaxValue) {
				UnityEngine.Debug.LogError("HeadsetID " + headsetID + " is invalid");
				return;
			}
			var writer = new BinaryWriter(stream);
			writer.Write((char)HID); // NOTE: this is where we set the header

			stream.WriteLineFrame(ref ml);
		}
		UnityEngine.Debug.Log("Sent lines.");
	}
	
	// MARK: File IO
	private const string FILENAME = "lines.frame";
	
	void WriteToFile(string path) {
		var ml = CurrentMakeLines;
		using (var sw = File.Open(path, FileMode.Create)) {
			sw.WriteLineFrame(ref ml);
		}
	}
	
	void GetFromFile(string path) {
		using (var s = File.OpenRead(path)) {
			ReadLines(s);
		}
	}
	
	// MARK: UI
	void OnGUI() {
		if (type != SynchronizerType.Server) {
			return;
		}
		
		if (GUI.Button(new Rect(0, 0, 100, 50), "Send ADB")) {
			Serve();
		}

		headsetID = GUI.TextField (new Rect (0, 50, 100, 20), headsetID);
		
		if (GUI.Button(new Rect(0, 100, 100, 50), "Write To File")) {
			WriteToFile(FILENAME);
		}
		if (GUI.Button(new Rect(0, 175, 100, 50), "Read From File")) {
			GetFromFile(FILENAME);
		}
	}
}

public static class BinaryFormatExtensions {

	// Used to make writing lines super easy
	
	public static List<List<makeLine.Line>> ReadLineFrame(this Stream s) {
		var reader = new BinaryReader(s);
		return reader.ReadLineFrame();
	}
	
	public static void WriteLineFrame(this Stream s, ref makeLine[] mlines) {
		var frame = new List<List<makeLine.Line>>();
		foreach (var ml in mlines) {
			frame.Add(ml.lines);
		}
		s.WriteLineFrame(ref frame);
	}
	
	public static void WriteLineFrame(this Stream s,
	                                  ref List<List<makeLine.Line>> lframe) {
		var writer = new BinaryWriter(s);
		writer.Write(ref lframe);
	}
	
	public static List<List<makeLine.Line>> ReadLineFrame(this BinaryReader reader) {
		var countLineSets = reader.ReadInt32();
		
		var lframe = new List<List<makeLine.Line>>();
		for (var i = 0; i < countLineSets; i++) {
			lframe.Add(reader.ReadLineSet());
		}
		
		return lframe;
	}
	
	public static void Write(this BinaryWriter writer,
	                         ref List<List<makeLine.Line>> lframe) {
		writer.Write(lframe.Count);
		foreach (var lset in lframe) {
			writer.Write(lset);
		}
	}
	
	public static List<makeLine.Line> ReadLineSet(this BinaryReader reader) {
		var countLines = reader.ReadInt32();
		
		var lset = new List<makeLine.Line>();
		for (var i = 0; i < countLines; i++) {
			lset.Add(reader.ReadLine());
		}
		return lset;
	}
	
	public static void Write(this BinaryWriter writer, List<makeLine.Line> lset) {
		writer.Write(lset.Count);
		foreach (var l in lset) {
			writer.Write(l);
		}
	}
	
	public static makeLine.Line ReadLine(this BinaryReader reader) {        
		var countPoints = reader.ReadInt32();
		
		var l = new makeLine.Line();
		
		var points = new List<makeLine.Point>();
		for(var i = 0; i < countPoints; i++) {
			var p = reader.ReadPoint();
			p.parent = l;
			points.Add(p);
		}
		
		l.points = points;
		l.opacity = 1f;
		return l;
	}
	
	public static void Write(this BinaryWriter writer, makeLine.Line l) {
		writer.Write(l.points.Count);
		
		foreach(var p in l.points) {
			writer.Write(p);
		}
	}
	
	public static makeLine.Point ReadPoint(this BinaryReader reader) {
		var x = reader.ReadSingle();
		var y = reader.ReadSingle();
		var z = reader.ReadSingle();
		
		var pos = new Vector3(x, y, z);
		var p = new makeLine.Point();
		p.pos = pos;
		
		return p;
	}
	
	public static void Write(this BinaryWriter writer, makeLine.Point p) {
		var pos = p.pos;
		
		writer.Write(pos.x);
		writer.Write(pos.y);
		writer.Write(pos.z);
	}
}