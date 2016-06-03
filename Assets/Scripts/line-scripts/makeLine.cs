using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class makeLine : MonoBehaviour {

	LineRenderer lineRen;
	private Renderer rend;
	private Vector3 prevPos;
	private Vector3 prevPrevPos;
	private float prevAngle;
	public float detailDistance = .01f;
	public float detailAngle = 1;
	public float eraseDistance = .1f;
	public float eraseSpeed = .97f;
	public int maxPoints = 100;

	Texture2D texture;
	
	public List<Line> lines;
	public Dictionary<int, List<Point>> hashGrid;

	public bool isBeingDestroyed = false;
	public bool isDestroyed = false;
	public float lineWidth = .02f;

	private float texOffset = 0;

	private const int h1 = 12178051;
	private const int h2 = 12481319;
	private const int h3 = 15485863;
	private const float granularity = 0.1f;

	public class Line {
		public List<Point> points;
		public float opacity;
	};
	public class Point {
		public Vector3 pos;
		public Line parent;
	}

	// Use this for initialization
	void Awake () {
		lines = new List<Line> ();
		lineRen = GetComponent<LineRenderer> ();
		rend = GetComponent<Renderer> ();
		lineRen.SetWidth (lineWidth, lineWidth);
		lineRen.sortingOrder = 1000;
		prevPos = Vector3.zero;
		prevPrevPos = Vector3.zero;
		texture = new Texture2D (1,1,TextureFormat.RGBAFloat,false);
		rend.material.SetTexture ("_Detail", texture);
		hashGrid = new Dictionary<int, List<Point>> ();
	}
	
	// Update is called once per frame
	void Update () {
		reduceTransparency ();
		if (lines.Count > 0) {
			lineRen.enabled = true;
		} else {
			lineRen.enabled = false;
		}
	}

	void reduceTransparency(){

		for (int i = 0; i < lines.Count; i++) {
			if (lines[i].opacity > 0 && lines[i].opacity < .999f){
				lines[i].opacity -= Time.deltaTime;
				makeTexture();
			}
			else if (lines[i].opacity < 0) {
				dequeueLine (i);
				makeTexture();
				rend.material.SetTextureScale ("_MainTex", new Vector2 ((getPointCount())*.1f , 1));
				//rend.material.SetTextureOffset("_MainTex", new Vector2(texOffset, 0));
			}
		}
	}

	public int getPointCount() {
		int total = 0;
		foreach (Line line in lines) {
			total += line.points.Count;
		}
		return total;
	}

	public Point getVertFromTotalIndex(int index) {
		int i = 0;
		while (index >= lines[i].points.Count) {
			index -= lines[i].points.Count;
			i++;
		}
		return lines [i].points [index];
	}

	public void dequeueLine(int index){
		lines.RemoveAt(index);
		rebuildLine ();
	}

	public int getHashedCell(Vector3 pos) {
		int x = Mathf.FloorToInt (pos.x / granularity);
		int y = Mathf.FloorToInt (pos.y / granularity);
		int z = Mathf.FloorToInt (pos.z / granularity);
		return x * h1 + y * h2 + z * h3;
	}

	public void makeTexture(){

		int detail = getPointCount ();
		texture.Resize ((int)detail, 1);
		texture.filterMode = FilterMode.Point;

		int accum = 0;
		for (int i = 0; i < lines.Count; i++) {
			for (int j = 0; j < lines[i].points.Count; j++) {
				if(j == 0 || j == lines[i].points.Count-1){
					texture.SetPixel (accum,0,new Color(1,1,1,0));
				}
				else{
					texture.SetPixel (accum,0,new Color(1,1,1,lines[i].opacity));
				}
				++accum;
			}
			
		}

		texture.Apply ();

	}

	public void rebuildLine(){
		/*
		if (lines.Count < 1)
			return;
		if (lines [0].points.Count < 2)
			return;

		List<Vector3> newPoints = new List<Vector3>();
		newPoints.Add(lines[0].points[0].pos);
		newPoints.Add(lines[0].points[1].pos);
		lineRen.SetVertexCount (1);
		lineRen.SetPosition (0, lines[0].points[0].pos);
		lineRen.SetVertexCount (2);
		lineRen.SetPosition (1, lines[0].points[1].pos);

		for (int i = 2 ; i < vertCount ; i++) {
			Vector3 p0 = getVertFromTotalIndex(i).pos;
			Vector3 p1 = getVertFromTotalIndex(i - 1).pos;
			Vector3 p2 = getVertFromTotalIndex(i - 2).pos;
			Vector3 targetDir = p0 - p1;
			Vector3 forward = p1 - p2;
			float angle = Vector3.Angle (targetDir, forward);

			if(Mathf.Abs(angle)>detailAngle)
				newPoints.Add( p0 );
		
		}
		*/

		int accum = 0;
		for (int i = 0; i < lines.Count; i++) {
			for (int j = 0; j < lines[i].points.Count; j++) {
				lineRen.SetVertexCount (accum+1);
				lineRen.SetPosition(accum,lines[i].points[j].pos);
				++accum;
			}
			
		}
	}

	public void addNewLine(GameObject brush){
		int lineCount = lines.Count;
		int pointCount = (lineCount > 0) ? (lines [lineCount - 1].points.Count) : 0;
		if (lineCount > 0 && pointCount > 0) {
			Point p = new Point ();
			p.pos = lines [lineCount - 1].points [pointCount - 1].pos;
			p.parent = lines [lines.Count - 1];
			lines [lines.Count - 1].points.Add (p);
		}
		Line l = new Line ();
		l.points = new List<Point> ();
		l.opacity = 1f;
		Point p0 = new Point ();
		p0.pos = brush.transform.position;
		p0.parent = l;
		l.points.Add (p0);
		lines.Add (l);
	}

	public void addPoint(GameObject b){

		Point p = new Point ();
		p.pos = b.transform.position;
		p.parent = lines[lines.Count - 1];
		lines[lines.Count-1].points.Add (p);
		
		if(getPointCount ()>maxPoints){
			lines[0].points.RemoveAt(0);
			if (lines[0].points.Count == 0) {
				lines.RemoveAt(0);
			}
		}


		makeTexture();
		rebuildLine ();

		lineRen.SetPosition (getPointCount()-1, b.transform.position);
		rend.material.SetTextureScale ("_MainTex", new Vector2 ((getPointCount())*.1f , 1));

		/*
		if(getPointCount ()>=maxPoints){
			texOffset+=.1f;
			rend.material.SetTextureOffset("_MainTex", new Vector2(texOffset, 0));
		}
		*/

		int cell = getHashedCell (b.transform.position);
		if (hashGrid.ContainsKey (cell)) {
			hashGrid [cell].Add (p);
		} else {
			hashGrid [cell] = new List<Point> ();
			hashGrid [cell].Add (p);
		}
	}

	public void addPoints(GameObject b, bool d){ //d is for drawing

		float distance = Vector3.Distance (b.transform.position, prevPos);
		if (d) {
			if(distance>detailDistance ){
				addPoint (b);
				prevPos = b.transform.position;
			}
		} else {
			if(getPointCount()>0){
				List<Point> pointsToCheck = new List<Point>();
				for (int i = -1; i <= 1; i++) {
					for (int j = -1; j <= 1; j++) {
						for (int k = -1; k <= 1; k++) {
							Vector3 offset = new Vector3(i, j, k) * granularity;
							int cell = getHashedCell(b.transform.position + offset);
							if (hashGrid.ContainsKey(cell)) {
								pointsToCheck.AddRange(hashGrid[cell]);
							}
						}
					}
				}
				for (int i = 0; i < pointsToCheck.Count; i++){
					Point p = pointsToCheck[i];
					if(Vector3.Distance(p.pos,b.transform.position)<eraseDistance){
						if(p.parent.opacity==1)
							p.parent.opacity=.99f;
					}
				}
			}
		}


	}
}
