using UnityEngine;
using System.Collections;

public class makeRoom : MonoBehaviour {

	public GameObject quad;
	public Vector3 dimensions = Vector3.one;
	public float gridScale = 1;
	public bool fitGrid = true;
	// Use this for initialization
	void Start () {
		Vector3 d = dimensions;
		surface (0, 0, 0, d.x, d.z, d.y, 90, 0, 0);
		surface (0, d.y, 0, d.x, d.z, d.y, -90, 0, 0);
		surface (0, d.y / 2, d.z / 2, d.x, d.y, d.z, 0, 0, 0);
		surface (0, d.y / 2, -d.z / 2, d.x, d.y, d.z, 0, 180, 0);
		surface (d.x/2, d.y / 2, 0, d.z, d.y, d.x, 0, 90, 0);
		surface (-d.x/2, d.y / 2, 0, d.z, d.y, d.x, 0, -90, 0);
		quad.GetComponent<MeshRenderer> ().enabled = false;

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	GameObject surface(float x, float y, float z, float sx, float sy, float sz, float rx, float ry, float rz){
	
		GameObject surf = Instantiate (quad, new Vector3(x,y,z), Quaternion.identity) as GameObject;
		surf.transform.Rotate (new Vector3 (rx, ry, rz));
		surf.transform.localScale = new Vector3 (sx, sy, sz);
		surf.transform.parent = this.transform;
		if(fitGrid)
			surf.GetComponent<MeshRenderer> ().material.SetTextureScale ("_MainTex", new Vector2 (Mathf.Round(sx*gridScale), Mathf.Round(sy*gridScale)));
		else
			surf.GetComponent<MeshRenderer> ().material.SetTextureScale ("_MainTex", new Vector2 (sx*gridScale, sy*gridScale));
		return surf;
	}
}
