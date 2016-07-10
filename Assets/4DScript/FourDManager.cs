using UnityEngine;
using System.Collections;
using System;

namespace Holojam
{
	public class FourDManager : WiiGlobalReceiver , IGlobalWiiMotePlusHandler {
	public GameObject[] FourDObjectcs;
	int toggle;
	public GameObject Trackball;
	Transform pre_tran;

	public void updateobject(int toggle)
	{
		for (int i = 0; i < FourDObjectcs.Length; i++)
		{
			if (i == toggle)
			{
				FourDObjectcs [i].GetComponent<Transform> ().position = pre_tran.position;
				FourDObjectcs [i].SetActive (true);
				Trackball.GetComponent<Transform> ().position = FourDObjectcs [i].GetComponent<Transform> ().position;
				Trackball.GetComponent<Transform> ().parent = FourDObjectcs [i].GetComponent<Transform> ();
			}
			else{
				FourDObjectcs[i].SetActive(false);
			}
	}
	}

	// Use this for initialization
	void Start () {
		toggle = 0;
		pre_tran = Trackball.GetComponent<Transform> ();
		updateobject(toggle);

	}


		public void OnGlobalPlusPressDown (WiiMoteEventData eventData)
		{
			pre_tran = FourDObjectcs [toggle].GetComponent<Transform> ();
			toggle = (toggle + 1) % FourDObjectcs.Length;
			updateobject (toggle);
		}
		public void OnGlobalPlusPress (WiiMoteEventData eventData)
		{
		}
		public void OnGlobalPlusPressUp (WiiMoteEventData eventData)
		{
		}



}

}
