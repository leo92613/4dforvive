using UnityEngine;
using System.Collections;

public class CameraPan : MonoBehaviour {

	// Use this for initialization
	public float xLimitLeft;
	public float xLimitRight;

	public float xMultiplier = 1.0f;	// Speed of the camera when being panned

	Vector3 camLastPos;
	float mouseXLastPos;
	void Start () 
	{
		//camLastPos = transform.position;
		//transform.position = new Vector3 (0,curPos.y,curPos.z);
	}
	
	// Update is called once per frame
	void Update () 
	{

		if (Input.GetMouseButtonDown(0))
		{
			camLastPos = transform.position;
			mouseXLastPos = Input.mousePosition.x / Screen.width;
		}
			
		if (Input.GetMouseButton(0))
		{
			float mouseXCurPos = (((Input.mousePosition.x / Screen.width) - mouseXLastPos) * xMultiplier)+ camLastPos.x;
			mouseXCurPos = Mathf.Clamp (mouseXCurPos, xLimitLeft ,xLimitRight);	
			transform.position = new Vector3(mouseXCurPos,camLastPos.y,camLastPos.z);
		}
		else
		{
		
		}
	}
}
