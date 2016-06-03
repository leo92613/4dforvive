using UnityEngine;
using System.Collections;

public class trackballmanager : MonoBehaviour {
	public GameObject HyperCube;
	// Use this for initialization
	void Start () {
		Transform parent = HyperCube.GetComponent<Transform> ();
		transform.position = parent.position;
		transform.parent = parent;
	}
	
	// Update is called once per frame
	void Update () {

	}
}
