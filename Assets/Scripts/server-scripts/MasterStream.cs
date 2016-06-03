using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Xml;
using System.IO;
using ProtoBuf;
using update_protocol_v3;

namespace Holojam
{

	public class MasterStream : Singleton<MasterStream>
	{
		private class OtherMarker
		{
			public Vector3 pos;
		}
		private class LiveObjectStorage
		{
			public Vector3 pos;
			public Quaternion rot;
			public int button_bits;
			public List<Vector2> axis_buttons;

			// TODO: Handle extra data
		}


		private const int BLACK_BOX_CLIENT_PORT = 1611;
		private Vector3 DEFAULT_POS_VEC;
		private Quaternion DEFAULT_ROT_QUAT;
		private int nBytesReceived;
		private bool stopReceive;
		private IPEndPoint ipEndPoint;
		private System.Threading.Thread thread = null;
		private Socket socket;
		private byte[] b1;
		private byte[] b2;
		private byte[] b3;
		private MemoryStream b1ms;
		private MemoryStream b2ms;
		private float accum;
		private int nPackets;
		private int nFrames;

		private UnityEngine.Object lock_object;

		private long lastLoadedFrame;
		private byte[] lastLoadedBuffer;
		private MemoryStream lastLoadedBufferMS;

		private Dictionary<string, LiveObjectStorage> labelToLiveObject;

		public void Start() {
			lock_object = new UnityEngine.Object();
			Application.runInBackground = true;
			Application.targetFrameRate = -1;
			DEFAULT_POS_VEC = new Vector3 (0, 0, 0);
			DEFAULT_ROT_QUAT = new Quaternion (0, 0, 0, 0);
			labelToLiveObject = new Dictionary<string, LiveObjectStorage>();
			accum = 0;
			nPackets = 0;
			nFrames = 0;
			// ~65KB buffer sizes
			b1 = new byte[65507];
			b2 = new byte[65507];
			b1ms = new MemoryStream(b1);
			b2ms = new MemoryStream(b2);
			thread = new System.Threading.Thread(ThreadRun);
			thread.Start();
		}
		// Handle new thread data / invoke Unity routines outside of the socket thread.
		public void Update() {
			accum += Time.deltaTime;
			float round_accum = (float)Math.Floor(accum);
			if (round_accum > 0) {
				accum -= round_accum;
				// print ("FPS: " + ((float)nFrames / round_accum).ToString());
				print ("packets per second: " + ((float)nPackets / round_accum).ToString());
				nPackets = 0;
				nFrames = 0;
			}
			nFrames++;
		}

		public Vector3 getLiveObjectPosition(string name) {
			LiveObjectStorage o;
			lock (lock_object) {
				if (!labelToLiveObject.TryGetValue (name, out o)) {
					//print ("Body not found: " + name);
					return DEFAULT_POS_VEC;
				}
			}
			return o.pos;
		}
		public Quaternion getLiveObjectRotation(string name) {
			LiveObjectStorage o;
			lock (lock_object) {
				if (!labelToLiveObject.TryGetValue (name, out o)) {
					//print ("Body not found: " + name);
					return DEFAULT_ROT_QUAT;
				}
			}
			return o.rot;
		}
		public int getLiveObjectButtonBits(string name) {
			LiveObjectStorage o;
			lock (lock_object) {
				if (!labelToLiveObject.TryGetValue (name, out o)) {
					//print ("Body not found: " + name);
					return 0;
				}
			}
			return o.button_bits;
		}
		public Vector2 getLiveObjectAxisButton(string name, int index) {
			LiveObjectStorage o;
			lock (lock_object) {
				if (!labelToLiveObject.TryGetValue (name, out o)) {
					//print ("Body not found: " + name);
					return Vector2.zero;
				}
			}
			return o.axis_buttons[index];
		}
		
		// This thread handles incoming NatNet packets.
		private void ThreadRun ()
		{
			stopReceive = false;
			Socket socket =new Socket(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
			ipEndPoint = new IPEndPoint (IPAddress.Any, BLACK_BOX_CLIENT_PORT);
			//Debug.Log("prebind");
			socket.Bind (ipEndPoint);
			//Debug.Log("bind");
			MulticastOption mo = new MulticastOption (IPAddress.Parse ("224.1.1.1"));
			socket.SetSocketOption (SocketOptionLevel.IP, SocketOptionName.AddMembership, mo);

			nBytesReceived = 0;
			lastLoadedBuffer = b1;
			lastLoadedBufferMS = b1ms;
			lastLoadedFrame = 0;
			
			byte[] newPacketBuffer = b2;
			MemoryStream newPacketBufferMS = b2ms;
			long newPacketFrame = 0;
			
			byte[] tempBuffer;
			MemoryStream tempBufferMS;

			while (true) {
				//Debug.Log("preRECV");
				nBytesReceived = socket.Receive(newPacketBuffer);
				//Debug.Log("RECV");
				nPackets++;
				newPacketBufferMS.Position = 0;
				//Debug.Log ("Deserializing data...");
				update_protocol_v3.Update update = Serializer.Deserialize<update_protocol_v3.Update>(new MemoryStream(newPacketBuffer, 0, nBytesReceived));
				//Debug.Log ("Data deserialized. Received update of type " + update.label);

				newPacketFrame = update.mod_version;
				if(newPacketFrame > lastLoadedFrame) {
					// Swap the buffers and reset the positions.
					lastLoadedBufferMS.Position = 0;
					newPacketBufferMS.Position = 0;
					tempBuffer = lastLoadedBuffer;
					tempBufferMS = lastLoadedBufferMS;
					lastLoadedBuffer = newPacketBuffer;
					lastLoadedBufferMS = newPacketBufferMS;
					newPacketBuffer = tempBuffer;
					newPacketBufferMS = tempBufferMS;
					lastLoadedFrame = newPacketFrame;



					for (int j = 0; j < update.live_objects.Count; j++) {
						LiveObject or = update.live_objects[j];
						string label = or.label;
						if (label == "marker") {
							Debug.Log ("marker at " + or.x + ", " + or.y + ", " + or.z);
						}
						LiveObjectStorage ow;
						lock (lock_object) {
							if (!labelToLiveObject.TryGetValue(label, out ow)) {
								ow = new LiveObjectStorage();
								labelToLiveObject[label] = ow;
							} else {
								ow = labelToLiveObject[label];
							}
							if (update.lhs_frame) {
								ow.pos = new Vector3(-(float)or.x, (float)or.y, (float)or.z);
								ow.rot = new Quaternion(-(float)or.qx, (float)or.qy, (float)or.qz, -(float)or.qw);
							} else {
								ow.pos = new Vector3((float)or.x, (float)or.y, (float)or.z);
								ow.rot = new Quaternion((float)or.qx, (float)or.qy, (float)or.qz, (float)or.qw);
							}
							ow.button_bits = or.button_bits;
						}
					}
				}

				if (stopReceive) {
					break;
				}
			}
		}
		private void OnDestroy ()
		{
			stopReceive = true;
		}
	}
}