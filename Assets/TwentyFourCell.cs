using UnityEngine;
using System.Collections;
using System;

namespace Holojam.IO
{
	public class Cell
	{
		int size;
		GameObject[] edges;
		public Vector4[] srcVertices;
		public Vector4[] vertices;
		Vector4[] index;
		Transform parentobj;
		GameObject[] spheres;

		void setparent (Transform par)
		{
			for (int i = 0; i < size; i++) {
				edges [i].transform.parent = par;
			}
		}

		public Cell (Transform par)
		{
			parentobj = par;
			size = 96;
			srcVertices = new Vector4[24];
			vertices = new Vector4[24];
			srcVertices[0] = new Vector4(-1,-1,0,0)*0.2f;
			srcVertices[1] = new Vector4(1,-1,0,0)*0.2f;
			srcVertices[2] = new Vector4(-1,1,0,0)*0.2f;
			srcVertices[3] = new Vector4(1,1,0,0)*0.2f;
			srcVertices[4] = new Vector4(-1,0,-1,0)*0.2f;
			srcVertices[5] = new Vector4(1,0,-1,0)*0.2f;
			srcVertices[6] = new Vector4(-1,0,1,0)*0.2f;
			srcVertices[7] = new Vector4(1,0,1,0)*0.2f;
			srcVertices[8] = new Vector4(-1,0,0,-1)*0.2f;
			srcVertices[9] = new Vector4(1,0,0,-1)*0.2f;
			srcVertices[10] = new Vector4(-1,0,0,1)*0.2f;
			srcVertices[11] = new Vector4(1,0,0,1)*0.2f;
			srcVertices[12] = new Vector4(0,-1,-1,0)*0.2f;
			srcVertices[13] = new Vector4(0,1,-1,0)*0.2f;
			srcVertices[14] = new Vector4(0,-1,1,0)*0.2f;
			srcVertices[15] = new Vector4(0,1,1,0)*0.2f;
			srcVertices[16] = new Vector4(0,-1,0,-1)*0.2f;
			srcVertices[17] = new Vector4(0,1,0,-1)*0.2f;
			srcVertices[18] = new Vector4(0,-1,0,1)*0.2f;
			srcVertices[19] = new Vector4(0,1,0,1)*0.2f;
			srcVertices[20] = new Vector4(0,0,-1,-1)*0.2f;
			srcVertices[21] = new Vector4(0,0,1,-1)*0.2f;
			srcVertices[22] = new Vector4(0,0,-1,1)*0.2f;
			srcVertices[23] = new Vector4(0,0,1,1)*0.2f;
			vertices[0] = new Vector4(-1,-1,0,0)*0.2f;
			vertices[1] = new Vector4(1,-1,0,0)*0.2f;
			vertices[2] = new Vector4(-1,1,0,0)*0.2f;
			vertices[3] = new Vector4(1,1,0,0)*0.2f;
			vertices[4] = new Vector4(-1,0,-1,0)*0.2f;
			vertices[5] = new Vector4(1,0,-1,0)*0.2f;
			vertices[6] = new Vector4(-1,0,1,0)*0.2f;
			vertices[7] = new Vector4(1,0,1,0)*0.2f;
			vertices[8] = new Vector4(-1,0,0,-1)*0.2f;
			vertices[9] = new Vector4(1,0,0,-1)*0.2f;
			vertices[10] = new Vector4(-1,0,0,1)*0.2f;
			vertices[11] = new Vector4(1,0,0,1)*0.2f;
			vertices[12] = new Vector4(0,-1,-1,0)*0.2f;
			vertices[13] = new Vector4(0,1,-1,0)*0.2f;
			vertices[14] = new Vector4(0,-1,1,0)*0.2f;
			vertices[15] = new Vector4(0,1,1,0)*0.2f;
			vertices[16] = new Vector4(0,-1,0,-1)*0.2f;
			vertices[17] = new Vector4(0,1,0,-1)*0.2f;
			vertices[18] = new Vector4(0,-1,0,1)*0.2f;
			vertices[19] = new Vector4(0,1,0,1)*0.2f;
			vertices[20] = new Vector4(0,0,-1,-1)*0.2f;
			vertices[21] = new Vector4(0,0,1,-1)*0.2f;
			vertices[22] = new Vector4(0,0,-1,1)*0.2f;
			vertices[23] = new Vector4(0,0,1,1)*0.2f;
			index = new Vector4[96];
			index[0] = new Vector4(0,4,1,4);
			index[1] = new Vector4(0,6,1,4);
			index[2] = new Vector4(0,8,1,3);
			index[3] = new Vector4(0,10,1,3);
			index[4] = new Vector4(0,12,2,4);
			index[5] = new Vector4(0,14,2,4);
			index[6] = new Vector4(0,16,2,3);
			index[7] = new Vector4(0,18,2,3);
			index[8] = new Vector4(1,5,1,4);
			index[9] = new Vector4(1,7,1,4);
			index[10] = new Vector4(1,9,1,3);
			index[11] = new Vector4(1,11,1,3);
			index[12] = new Vector4(1,12,2,4);
			index[13] = new Vector4(1,14,2,4);
			index[14] = new Vector4(1,16,2,3);
			index[15] = new Vector4(1,18,2,3);
			index[16] = new Vector4(2,4,1,4);
			index[17] = new Vector4(2,6,1,4);
			index[18] = new Vector4(2,8,1,3);
			index[19] = new Vector4(2,10,1,3);
			index[20] = new Vector4(2,13,2,4);
			index[21] = new Vector4(2,15,2,4);
			index[22] = new Vector4(2,17,2,3);
			index[23] = new Vector4(2,19,2,3);
			index[24] = new Vector4(3,5,1,4);
			index[25] = new Vector4(3,7,1,4);
			index[26] = new Vector4(3,9,1,3);
			index[27] = new Vector4(3,11,1,3);
			index[28] = new Vector4(3,13,2,4);
			index[29] = new Vector4(3,15,2,4);
			index[30] = new Vector4(3,17,2,3);
			index[31] = new Vector4(3,19,2,3);
			index[32] = new Vector4(4,8,1,2);
			index[33] = new Vector4(4,10,1,2);
			index[34] = new Vector4(4,12,3,4);
			index[35] = new Vector4(4,13,3,4);
			index[36] = new Vector4(4,20,3,2);
			index[37] = new Vector4(4,22,3,2);
			index[38] = new Vector4(5,9,1,2);
			index[39] = new Vector4(5,11,1,2);
			index[40] = new Vector4(5,12,3,4);
			index[41] = new Vector4(5,13,3,4);
			index[42] = new Vector4(5,20,3,2);
			index[43] = new Vector4(5,22,3,2);
			index[44] = new Vector4(6,8,1,2);
			index[45] = new Vector4(6,10,1,2);
			index[46] = new Vector4(6,14,3,4);
			index[47] = new Vector4(6,15,3,4);
			index[48] = new Vector4(6,21,3,2);
			index[49] = new Vector4(6,23,3,2);
			index[50] = new Vector4(7,9,1,2);
			index[51] = new Vector4(7,11,1,2);
			index[52] = new Vector4(7,14,3,4);
			index[53] = new Vector4(7,15,3,4);
			index[54] = new Vector4(7,21,3,2);
			index[55] = new Vector4(7,23,3,2);
			index[56] = new Vector4(8,16,4,3);
			index[57] = new Vector4(8,17,4,3);
			index[58] = new Vector4(8,20,4,2);
			index[59] = new Vector4(8,21,4,2);
			index[60] = new Vector4(9,16,4,3);
			index[61] = new Vector4(9,17,4,3);
			index[62] = new Vector4(9,20,4,2);
			index[63] = new Vector4(9,21,4,2);
			index[64] = new Vector4(10,18,4,3);
			index[65] = new Vector4(10,19,4,3);
			index[66] = new Vector4(10,22,4,2);
			index[67] = new Vector4(10,23,4,2);
			index[68] = new Vector4(11,18,4,3);
			index[69] = new Vector4(11,19,4,3);
			index[70] = new Vector4(11,22,4,2);
			index[71] = new Vector4(11,23,4,2);
			index[72] = new Vector4(12,16,2,1);
			index[73] = new Vector4(12,18,2,1);
			index[74] = new Vector4(12,20,3,1);
			index[75] = new Vector4(12,22,3,1);
			index[76] = new Vector4(13,17,2,1);
			index[77] = new Vector4(13,19,2,1);
			index[78] = new Vector4(13,20,3,1);
			index[79] = new Vector4(13,22,3,1);
			index[80] = new Vector4(14,16,2,1);
			index[81] = new Vector4(14,18,2,1);
			index[82] = new Vector4(14,21,3,1);
			index[83] = new Vector4(14,23,3,1);
			index[84] = new Vector4(15,17,2,1);
			index[85] = new Vector4(15,19,2,1);
			index[86] = new Vector4(15,21,3,1);
			index[87] = new Vector4(15,23,3,1);
			index[88] = new Vector4(16,20,4,1);
			index[89] = new Vector4(16,21,4,1);
			index[90] = new Vector4(17,20,4,1);
			index[91] = new Vector4(17,21,4,1);
			index[92] = new Vector4(18,22,4,1);
			index[93] = new Vector4(18,23,4,1);
			index[94] = new Vector4(19,22,4,1);
			index[95] = new Vector4(19,23,4,1);


			edges = new GameObject[96];
			spheres = new GameObject[24];

			for (int i = 0; i < 24; i++) {
				Vector3 pos = new Vector3 (vertices [i].x, vertices [i].y, vertices [i].z);
				GameObject verObj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				verObj.transform.position = pos;
				verObj.transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
				spheres [i] = verObj;
				spheres [i].transform.parent = par;

			}

			for (int i = 0; i < size; i++) {
				int start, end, color;
				start = (int)index [i].x;
				end = (int)index [i].y;
				color = (int)index [i].z;
				Vector3 beginpoint_ = new Vector3 (vertices [start].x, vertices [start].y, vertices [start].z);
				Vector3 endpoint_ = new Vector3 (vertices [end].x, vertices [end].y, vertices [end].z);
				Vector3 pos = Vector3.Lerp (beginpoint_, endpoint_, (float)0.5);
				float distance = Vector3.Distance (beginpoint_, endpoint_);
				GameObject segObj = GameObject.CreatePrimitive (PrimitiveType.Cube);
				segObj.transform.position = pos;
				segObj.transform.LookAt (endpoint_);
				segObj.transform.Rotate (new Vector3 (1.0f, 0, 0), 90);
				segObj.transform.localScale = new Vector3 (0.01f, distance, 0.01f);
				edges [i] = segObj;
				coloredge (i, color);
			}

			// This is for testing the algorithm



			setparent (par);
		}

		public void coloredge(int i, int index)
		{
			if (index == 1) {
				edges [i].GetComponent<Renderer>().material.color = Color.green;
			}
			if (index == 2) {
				edges [i].GetComponent<Renderer>().material.color = Color.red;
			}
			if (index == 3) {
				edges [i].GetComponent<Renderer>().material.color = Color.blue;
			}
			if (index == 4) {
				edges [i].GetComponent<Renderer>().material.color = Color.yellow;
			}

		}


		public void returnpoint4 (float[] src, int i)
		{
			src [0] = (float)vertices [i].x;
			src [1] = (float)vertices [i].y;
			src [2] = (float)vertices [i].z;
			src [3] = (float)vertices [i].w;
		}

		public void updatepoint4 (float[] src, int i)
		{
			vertices [i].x = (float)src [0];
			vertices [i].y = (float)src [1];
			vertices [i].z = (float)src [2];
			vertices [i].w = (float)src [3];
		}

		public void update_edges ()
		{

			for (int i = 0; i < 24; i++) {
				Vector3 pos = new Vector3 (vertices [i].x, vertices [i].y, vertices [i].z);
				spheres [i].transform.localPosition = pos;
				spheres [i].transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);

			}


			for (int i = 0; i < 96; i++) {
				int start, end;
				start = (int)index [i].x;
				end = (int)index [i].y;
				Vector3 beginpoint_ = new Vector3 (vertices [start].x, vertices [start].y, vertices [start].z);
				Vector3 endpoint_ = new Vector3 (vertices [end].x, vertices [end].y, vertices [end].z);
				Vector3 pos = Vector3.Lerp (beginpoint_, endpoint_, (float)0.5);
				float distance = Vector3.Distance (beginpoint_, endpoint_);
				Quaternion rot = Quaternion.LookRotation (beginpoint_ - endpoint_);
				edges [i].transform.localPosition = pos;
				edges [i].transform.LookAt (endpoint_);
				edges [i].transform.rotation = rot;
				edges [i].transform.localScale = new Vector3 (0.01f, 0.01f, distance);
			}
		}

	}


	public class TwentyFourCell : WiiGlobalReceiver, IGlobalWiiMoteBHandler , IGlobalWiiMoteAHandler
	{
		public Transform box;
		public Trackball trackball;
		public Cell cell;
		public Vector3 A_;
		public Vector3 B_;
		public Vector4 A, B;
		bool isbutton;
		public GameObject Trackball;
		float radius;
		public Vector3 movement;


		void UpdateRotation (Cell cell, Trackball trackball, Vector4 A_, Vector4 B_)
		{

			float[] A = new float[4]{ A_.x, A_.y, A_.z, A_.w };
			float[] B = new float[4]{ B_.x, B_.y, B_.z, B_.w };

			trackball.rotate (A, B);

			for (int i = 0; i < 24; i++) {

				float[] src = new float[4];
				src [0] = cell.srcVertices [i].x;
				src [1] = cell.srcVertices [i].y;
				src [2] = cell.srcVertices [i].z;
				src [3] = cell.srcVertices [i].w;
				float[] dst = new float[4];

				trackball.transform (src, dst);

				cell.updatepoint4 (dst, i);
				cell.update_edges ();
			}
		}

		// Use this for initialization
		void Start ()
		{
			box = this.GetComponent<Transform> ();
			trackball = new Trackball (4);
			cell = new Cell (box);
			box.position = new Vector3 (0f, 1.5f, -0.9f);
			A_ = new Vector3 ();
			B_ = new Vector3 ();
			isbutton = false;
			A = new Vector4 ();
			B = new Vector4 ();
			//radius = Trackball.GetComponent<SphereCollider> ().radius;
			radius = 1.0f;

		}

		// Update is called once per frame
		void Update ()
		{
			if (isbutton) {
				Vector3 relapos = new Vector3 ();
				relapos = (B_ - box.position)*8f/3f;
				float r = (float)Math.Sqrt(relapos.x * relapos.x + relapos.y * relapos.y + relapos.z * relapos.z);
				if (r < radius) {					
					B = new Vector4 (relapos.x, relapos.y, relapos.z, (float)Math.Sqrt (radius*radius - relapos.x * relapos.x - relapos.y * relapos.y - relapos.z * relapos.z));
				}
				else{
					//float length = relapos.magnitude;
					Vector3 Q = (radius / r) * relapos;
					relapos = Q + box.position;
					B = new Vector4 (Q.x, Q.y, Q.z, 0f);
				}
				UpdateRotation (cell, trackball, A, B);
				A = B;
			}
		}


		public void OnGlobalBPress (WiiMoteEventData eventData)
		{
			isbutton = true;
			B_ = eventData.module.transform.position;
			Debug.Log (eventData.module.transform.position);
		}

		public void OnGlobalBPressDown (WiiMoteEventData eventData)
		{
			isbutton = true;
			B_ = eventData.module.transform.position;
			Vector3 relapos = new Vector3 ();
			relapos = (B_ - box.position)*8f/3f;
			float r = (float)Math.Sqrt(relapos.x * relapos.x + relapos.y * relapos.y + relapos.z * relapos.z);
			if (r < radius) {					
				B = new Vector4 (relapos.x, relapos.y, relapos.z, (float)Math.Sqrt (radius*radius - relapos.x * relapos.x - relapos.y * relapos.y - relapos.z * relapos.z));
			}
			else{
				//float length = relapos.magnitude;
				Vector3 Q = (radius / r) * relapos;
				//relapos = Q + box.position;
				B = new Vector4 (Q.x, Q.y, Q.z, 0f);
			}
			A = B;
			Debug.Log (eventData.module.transform.position);

		}

		public void OnGlobalAPressDown(WiiMoteEventData eventData)
		{
			movement = new Vector3 ();
			movement = box.position - eventData.module.transform.position;
		}

		public void OnGlobalAPress (WiiMoteEventData eventData)
		{
			box.position = eventData.module.transform.position + movement;
		}
		public void OnGlobalAPressUp (WiiMoteEventData eventData)
		{
		}


		public void OnGlobalBPressUp (WiiMoteEventData eventData)
		{
			isbutton = false;
			A_ = eventData.module.transform.position;
			B_ = eventData.module.transform.position;
			Debug.Log (eventData.module.transform.position);
		}
	}

}



