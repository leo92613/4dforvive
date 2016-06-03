using UnityEngine;
using System.Collections.Generic;

public class FingerRenderer : MonoBehaviour {
	public Transform[] nodes;
	public Material mat;

	private List<GameObject> boxes;

	// Use this for initialization
	void Start () {
		boxes = new List<GameObject> ();
		for (int i = 0; i < nodes.Length - 1; i++) {
			GameObject box = GameObject.CreatePrimitive (PrimitiveType.Cube);
			box.GetComponent<Renderer> ().material = mat;
			boxes.Add (box);
		}
		/*
		LineRenderer l = this.GetComponent<LineRenderer> ();
		l.SetVertexCount (nodes.Length);
		*/
	}
	
	// Update is called once per frame
	void Update () {
		for (int i = 0; i < nodes.Length - 1; i++) {
			GameObject box = boxes [i];
			Vector3 p1 = nodes [i].position;
			Vector3 p2 = nodes [i + 1].position;
			box.transform.forward = Vector3.Normalize(p2 - p1);
			//box.transform.up = Vector3.Normalize(-Vector3.Cross (box.transform.forward, box.transform.right));
			//box.transform.forward = Vector3.Normalize(p2 - p1);

			box.transform.position = (p1 + p2) / 2f;
			box.transform.localScale = new Vector3 (0.015f, 0.015f, Vector3.Distance (p1, p2));
		}
		/*
		LineRenderer l = this.GetComponent<LineRenderer> ();
		for (int i = 0; i < nodes.Length; i++) {
			l.SetPosition (i, nodes [i].position);
		}
		*/
	}

	public void setColor (Color c) {
		foreach (GameObject box in boxes) {
			box.GetComponent<Renderer> ().material.SetColor ("_Color", c);
		}
		/*
		LineRenderer l = this.GetComponent<LineRenderer> ();
		l.material.color = c;
		*/
	}
}
