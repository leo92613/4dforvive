using UnityEngine;
using System;

class BillboardLabel : BillboardComponent
{

	/**
	 * A good value for viewportSize.y is (0.07f * numberOfLinesInText)
	 **/

	private TextMesh mesh;
	private MeshRenderer mrenderer;

	public string text {
		get {
			if (mesh)
				return mesh.text;
			else
				return "";
		}
		set {
			if (!mesh && !(mesh = gameObject.GetComponent<TextMesh> ()))
				mesh = gameObject.AddComponent<TextMesh> ();
			mesh.text = value;
		}
	}

	// Label will automatically adjust insets
	public bool adjustInsets = false;

	void Start ()
	{
		if (!mesh && !(mesh = gameObject.GetComponent<TextMesh> ())) {
			mesh = gameObject.AddComponent<TextMesh> ();
		}
		mesh.fontSize = 100;
		mesh.transform.localScale *= 0.03f;
		mesh.color = Color.black;
		mesh.text = "";
		mrenderer = gameObject.GetComponent<MeshRenderer> ();
		if (!cam) {
			cam = Camera.main;
		}
	}

	void Update ()
	{
		if (hidden) {
			mesh.text = "";
		} else {
			mesh.text = text;
			if (adjustInsets) {
				var bounds = mrenderer.bounds.size;
				bounds.x *= transform.localScale.x * 5;
				bounds.y *= transform.localScale.y * 5;
				
				viewportSize.x = bounds.x;
				viewportSize.y = bounds.y;
			}
			Layout ();
		}
	}

	void Layout ()
	{
		gameObject.transform.rotation = cam.transform.rotation;
		var worldPos = cam.ViewportToWorldPoint (new Vector3 (viewportPosition.x, viewportPosition.y, distance));
		gameObject.transform.position = worldPos;
	}

}