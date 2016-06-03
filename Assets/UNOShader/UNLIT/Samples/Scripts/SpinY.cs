using UnityEngine;
using System.Collections;

public class SpinY : MonoBehaviour {

	// Use this for initialization
	public float rotationSpeed;
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		transform.Rotate(0, Time.deltaTime * rotationSpeed, 0, Space.Self);
	}
}
