using UnityEngine;
using System.Collections.Generic;
using System;

public class Billboard : MonoBehaviour
{
	private float secondTock = 0f;
	private float y = 1;
	private float xStart = 0;
	private float xEnd = 0;
	public Vector2 insets = new Vector2(0.15f, 0.15f);
	public bool hidden;
	public Camera cam;
	public float padding = 0.02f;

	void Start ()
	{
		if (!cam) {
			cam = Camera.main;
		}
	}

	void Update ()
	{
		foreach (var component in gameObject.GetComponentsInChildren<BillboardComponent> ()) {
			component.hidden = hidden;
			component.cam = cam;
			if (y - component.viewportSize.y - padding <= insets.y) {
				y = 1 - insets.y;
				xStart = xEnd;
			}
			component.viewportPosition.x = xStart;
			component.viewportPosition.y = y;
			y -= component.viewportSize.y + padding;
			if (xStart + component.viewportSize.x + padding > xEnd) {
				xEnd = xStart + component.viewportSize.x + padding;
			}
		}
		y = 1 - insets.y;
		xStart = xEnd = insets.x;
	}

}