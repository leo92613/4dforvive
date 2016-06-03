
using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class LineUnity : MonoBehaviour {
	
	private Transform parent;
	public GameObject brush;

	public int maxVertexCount = 100;
	private int vertexCount = 0;

	public GameObject line;
	public List<GameObject> lines = new List<GameObject> ();


	private bool drawing = false;
	private bool erasing = false;

	private bool added = true;
	private bool click = false;

	
//	public int maxLines = 10;

	void Start(){
		init ();
//		lines.Add(Instantiate(line));

		
	}
	
	public void init(){
//		parent = new GameObject ().transform;
	}
	
	void Update() {

		onDrag ();
		killOld ();

		if (Input.GetMouseButton (0)) {
			onClick ();
		}
		if (Input.GetMouseButton (1)) {
			onRightClick ();
		}
		if (Input.GetMouseButtonUp (0)) {
			onRelease ();
		}
		if (Input.GetMouseButtonUp (1)) {
			onRelease ();
		}
	}

	public void onRightClick(){
		erasing = true;
	}

	public void onClick(){
		if (!drawing)
			click = true;
		else
			click = false;
		drawing = true;
	}

	public void onRelease(){
		drawing=false;
		erasing = false;
		click = false;
		added=true;
	}

	public void onDrag(){

		if (added&&click) {
			addLine ();
			click = false;
			added=false;
		}

		if (drawing) {
			drawLine (drawing);
		} else if (erasing) {
			eraseLine(drawing);
		}
	}

	public void drawLine(bool draw){
		makeLine other = (makeLine)lines [lines.Count-1].GetComponent (typeof(makeLine));
		other.addPoints (brush,draw);
	}

	public void eraseLine(bool draw){
		for (int i = 0; i < lines.Count; i ++) {
			makeLine other = (makeLine)lines [i].GetComponent (typeof(makeLine));
			other.addPoints (brush, draw);
		}
	}
	
	void killOld(){

		if (lines.Count > 0) {

			makeLine other = (makeLine)lines [0].GetComponent (typeof(makeLine));
			vertexCount=other.getPointCount();

//			if(vertexCount>maxVertexCount){
//				other.dequeueLine(0,1);
//			}

//			
//			if (lines.Count > maxLines) 
//				other.isBeingDestroyed = true;
//			else
//				other.isBeingDestroyed = false;

			vertexCount = 0;

//			for(int i = 0 ; i < lines.Count ; i++){
//				other = (makeLine)lines [i].GetComponent (typeof(makeLine));
//				vertexCount += other.getPointCount();
//				if (other.isDestroyed) {
//					Destroy (lines [i].gameObject);
//					lines.Remove (lines [i]);
//				}
//			}
		}
	}
	
	public void addLine(){
		if(lines.Count<1)
			lines.Add(Instantiate(line));
		makeLine other = (makeLine)lines [0].GetComponent (typeof(makeLine));
		other.addNewLine (brush);
		other.maxPoints = maxVertexCount;
//		lines.Add(Instantiate(line));
	}

	
}