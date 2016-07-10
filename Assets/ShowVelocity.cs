using UnityEngine;
using System.Collections;

public class ShowVelocity : MonoBehaviour {
    public Vector3 velocity;
    Vector3 prepos, pos;
	// Use this for initialization
	void Start () {
        prepos = this.transform.position;
        pos = this.transform.position;
	}
	
	// Update is called once per frame
	void Update () {
        pos = this.transform.position;
        velocity = (pos - prepos) / Time.deltaTime;
        prepos = pos;
	}
}
