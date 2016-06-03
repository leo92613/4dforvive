using UnityEngine;
using System.Collections;

public class Bounce : MonoBehaviour {

	public Transform PointA;
	public Transform PointB;
	public float speed;
	Transform currentPoint;


	// Use this for initialization
	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
		float moveSpeed = speed * Time.deltaTime;
		if(currentPoint == null)
		{
			currentPoint =  PointA;
		}
		float dist = Vector3.Distance(transform.position, currentPoint.position);
		if(dist < 0.1)
		{
			if (currentPoint == PointA)
				currentPoint = PointB;
			else
				currentPoint = PointA;

		}
		transform.position = Vector3.MoveTowards(transform.position, currentPoint.position, moveSpeed);
	}
}
