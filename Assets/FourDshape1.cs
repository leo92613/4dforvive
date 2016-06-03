using UnityEngine;
using System.Collections;
using System;

namespace Holojam.IO
{
	public class FourDshape
	{
		int size;
		GameObject[] edges;
		public Vector4[] srcVertices;
		public Vector4[] vertices;
		Vector3[] index;
		Transform parentobj;
		GameObject[] spheres;

		void setparent (Transform par)
		{
			for (int i = 0; i < size; i++) {
				edges [i].transform.parent = par;
			}
		}

		public FourDshape (Transform par)
		{
			parentobj = par;
			size = 24;
			srcVertices = new Vector4[8];
			vertices = new Vector4[8];
			vertices[0] = new Vector4(-1,0,0,0)*0.2f;
			vertices[1] = new Vector4(1,0,0,0)*0.2f;
			vertices[2] = new Vector4(0,-1,0,0)*0.2f;
			vertices[3] = new Vector4(0,1,0,0)*0.2f;
			vertices[4] = new Vector4(0,0,-1,0)*0.2f;
			vertices[5] = new Vector4(0,0,1,0)*0.2f;
			vertices[6] = new Vector4(0,0,0,-1)*0.2f;
			vertices[7] = new Vector4(0,0,0,1)*0.2f;
			vertices[0] = new Vector4(-1,0,0,0)*0.2f;
			vertices[1] = new Vector4(1,0,0,0)*0.2f;
			vertices[2] = new Vector4(0,-1,0,0)*0.2f;
			vertices[3] = new Vector4(0,1,0,0)*0.2f;
			vertices[4] = new Vector4(0,0,-1,0)*0.2f;
			vertices[5] = new Vector4(0,0,1,0)*0.2f;
			vertices[6] = new Vector4(0,0,0,-1)*0.2f;
			vertices[7] = new Vector4(0,0,0,1)*0.2f;
			srcVertices[0] = new Vector4(-1,0,0,0)*0.2f;
			srcVertices[1] = new Vector4(1,0,0,0)*0.2f;
			srcVertices[2] = new Vector4(0,-1,0,0)*0.2f;
			srcVertices[3] = new Vector4(0,1,0,0)*0.2f;
			srcVertices[4] = new Vector4(0,0,-1,0)*0.2f;
			srcVertices[5] = new Vector4(0,0,1,0)*0.2f;
			srcVertices[6] = new Vector4(0,0,0,-1)*0.2f;
			srcVertices[7] = new Vector4(0,0,0,1)*0.2f;
			srcVertices[0] = new Vector4(-1,0,0,0)*0.2f;
			srcVertices[1] = new Vector4(1,0,0,0)*0.2f;
			srcVertices[2] = new Vector4(0,-1,0,0)*0.2f;
			srcVertices[3] = new Vector4(0,1,0,0)*0.2f;
			srcVertices[4] = new Vector4(0,0,-1,0)*0.2f;
			srcVertices[5] = new Vector4(0,0,1,0)*0.2f;
			srcVertices[6] = new Vector4(0,0,0,-1)*0.2f;
			srcVertices[7] = new Vector4(0,0,0,1)*0.2f;
			index = new Vector3[24];
			index[0] = new Vector3(0,2,1);
			index[1] = new Vector3(1,2,1);
			index[2] = new Vector3(0,3,1);
			index[3] = new Vector3(1,3,1);
			index[4] = new Vector3(0,4,3);
			index[5] = new Vector3(1,4,3);
			index[6] = new Vector3(0,5,3);
			index[7] = new Vector3(1,5,3);
			index[8] = new Vector3(0,6,5);
			index[9] = new Vector3(1,6,5);
			index[10] = new Vector3(0,7,5);
			index[11] = new Vector3(1,7,5);
			index[12] = new Vector3(2,4,2);
			index[13] = new Vector3(3,4,2);
			index[14] = new Vector3(2,5,2);
			index[15] = new Vector3(3,5,2);
			index[16] = new Vector3(2,6,4);
			index[17] = new Vector3(3,6,4);
			index[18] = new Vector3(2,7,4);
			index[19] = new Vector3(3,7,4);
			index[20] = new Vector3(4,6,6);
			index[21] = new Vector3(5,6,6);
			index[22] = new Vector3(4,7,6);
			index[23] = new Vector3(5,7,6);

			edges = new GameObject[24];
			spheres = new GameObject[8];

			for (int i = 0; i < 8; i++) {
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
			if (index == 5) {
				edges [i].GetComponent<Renderer>().material.color = Color.black;
			}
			if (index == 6) {
				edges [i].GetComponent<Renderer>().material.color = Color.grey;
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

			for (int i = 0; i < 8; i++) {
				Vector3 pos = new Vector3 (vertices [i].x, vertices [i].y, vertices [i].z);
				spheres [i].transform.localPosition = pos;
				spheres [i].transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);

			}


			for (int i = 0; i < 24; i++) {
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


	public class FourDshape1 : WiiGlobalReceiver, IGlobalWiiMoteBHandler, IGlobalWiiMoteAHandler
	{
		public Transform box;
		public Trackball trackball;
		public FourDshape cell;
		public Vector3 A_;
		public Vector3 B_;
		public Vector4 A, B;
		bool isbutton;
		public GameObject Trackball;
		float radius;
		public Vector3 movement;


		void UpdateRotation (FourDshape cell, Trackball trackball, Vector4 A_, Vector4 B_)
		{

			float[] A = new float[4]{ A_.x, A_.y, A_.z, A_.w };
			float[] B = new float[4]{ B_.x, B_.y, B_.z, B_.w };

			trackball.rotate (A, B);

			for (int i = 0; i < 8; i++) {

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
			cell = new FourDshape (box);
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
		public void OnGlobalBPressUp (WiiMoteEventData eventData)
		{
			isbutton = false;
			A_ = eventData.module.transform.position;
			B_ = eventData.module.transform.position;
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

	}

}

