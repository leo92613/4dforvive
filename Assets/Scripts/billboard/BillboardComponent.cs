using System;
using UnityEngine;

public class BillboardComponent : MonoBehaviour
{
	/*
	 * All positioning data, including height and width,
	 * is given on [0, 1].
	 */
	public Vector2 viewportPosition = new Vector2 ();
	public Vector2 viewportSize = new Vector2 (0.5f, 0.07f);
	public float distance = 5f;
	public Camera cam;
	public bool hidden = false;

	void Start ()
	{
		if (!cam) {
			cam = Camera.main;
		}
	}

	void Layout () 
	{
		if (hidden) {
			return;
		}
		// Keep locked to camera
		gameObject.transform.rotation = cam.transform.rotation;
		var worldPos = cam.ViewportToWorldPoint (new Vector3 (viewportPosition.x, viewportPosition.y, distance));
		gameObject.transform.position = worldPos;
	}

	void Update ()
	{
		Layout ();
	}
}