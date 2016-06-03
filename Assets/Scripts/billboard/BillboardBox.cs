using UnityEngine;
using System;

class BillboardBox : BillboardComponent
{
	private LineRenderer line;
	private Vector3 topLeft = new Vector3 (),
		topRight = new Vector3 (),
		bottomLeft = new Vector3 (),
		bottomRight = new Vector3 ();

	public Material material;

	void Start ()
	{	
		if (!(line = gameObject.GetComponent<LineRenderer> ())) {
			line = gameObject.AddComponent<LineRenderer> ();
		}
		
		line.SetWidth (0.05f, 0.05f);
		line.SetVertexCount (4); // four points in a box
	}

	void Layout ()
	{
		topLeft.z = topRight.z = bottomLeft.z = bottomRight.z = distance;
		topLeft.x = viewportPosition.x;
		topLeft.y = viewportPosition.y;
		topRight.x = topLeft.x + viewportSize.x;
		topRight.y = topLeft.y;
		bottomLeft.x = viewportPosition.x;
		bottomLeft.y = viewportPosition.y - viewportSize.y;
		bottomRight.x = topLeft.x + viewportSize.x;
		bottomRight.y = topLeft.y - viewportSize.y;

		line.SetPosition (0, cam.ViewportToWorldPoint(topLeft));
		line.SetPosition (1, cam.ViewportToWorldPoint(topRight));
		line.SetPosition (2, cam.ViewportToWorldPoint(bottomRight));
		line.SetPosition (3, cam.ViewportToWorldPoint(bottomLeft));
	}

	void Update ()
	{
		if (hidden)
			return;
		line.material = material;
		Layout ();
	}

}