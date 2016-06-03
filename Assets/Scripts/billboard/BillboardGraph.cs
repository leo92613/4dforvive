using UnityEngine;
using System.Collections.Generic;
using System;

public class BillboardGraph : BillboardComponent
{

	// WARNING: Graph assumes non negative data
	
	// Accepts data on [0, 1] unless `normalize` is set to true
	public Queue<float> data = new Queue<float> ();
	private int lastRenderedDataCount = 0;

	// The max number of points to show on screen at one time
	public int granularity = 150;

	// Causes data to be normalized to [0, 1] based on the max value seen
	public bool normalizeData = true;
	public float maxValue = 1f;
	private LineRenderer line;

	public Material material;

	public string labelText {
		get {
			if (label)
				return label.text;
			return "";
		}
		set {
			if (!label)
				label = (new GameObject ()).AddComponent<BillboardLabel> ();
			label.text = value;
		}
	}

	private BillboardLabel label;
	private float secondTock = 0f;

	void Start ()
	{
		if (!line && !(line = gameObject.GetComponent<LineRenderer> ())) {
			line = gameObject.AddComponent<LineRenderer> ();
		}
		
		line.SetWidth (0.04f, 0.04f);
		if (!label)
			label = (new GameObject ()).AddComponent<BillboardLabel> ();
		if (!cam) {
			cam = Camera.main;
		}
		label.cam = cam;
		label.viewportSize.x = 0.08f; // A good length for 3 characters
	}

	void Update ()
	{
		label.hidden = hidden;
		if (hidden) {
			line.SetVertexCount (0);
			return;
		}
		if (data.Count == lastRenderedDataCount)
			return;
		while (data.Count > granularity) {
			data.Dequeue ();
		}
		label.text = labelText;
		line.material = material;
		Layout ();
	}
	
	void Layout ()
	{
		line.SetVertexCount (data.Count);
		var i = 0;
		var theMax = maxValue;
		foreach (var datum in data) {
			var normalizedDatum = datum;
			if (double.IsInfinity (normalizedDatum))
				continue;
			if (normalizeData) {
				if (datum > maxValue)
					maxValue = datum;
				normalizedDatum = Math.Min (datum / theMax, 1f);
			}
			var x = (viewportSize.x - label.viewportSize.x) * ((float)i / (float)granularity) + 
				viewportPosition.x + label.viewportSize.x;
			var y = (viewportSize.y * normalizedDatum) + viewportPosition.y - viewportSize.y;
			var viewportPoint = new Vector3 (x, y, distance);
			var pos = cam.ViewportToWorldPoint (viewportPoint);
			line.SetPosition (i, pos);
			i++;
		}
		label.viewportPosition = viewportPosition;
	}
}
