using UnityEngine;
using System.Collections;
using System;

namespace Holojam.IO
{
	public class Trackball
	{
		int size;
		float[,] mat, rot, tmp, err;
		bool isDebug = false;

		public Trackball (int size)
		{
			this.size = size;
			mat = new float[size, size];
			rot = new float[size, size];
			tmp = new float[size, size];
			err = new float[size, size];
			identity ();
		}

		public string toString ()
		{
			return toString (mat);
		}

		public string toString (float[,] mat)
		{
			string s = "{ ";
			for (int row = 0; row < size; row++) {
				s += "{";
				for (int col = 0; col < size; col++)
					s += round (mat [row, col]) + ",";
				s += "},";
			}
			s += " }";
			return s;
		}

		public void identity ()
		{
			identity (mat);
		}

		public void identity (float[,] mat)
		{
			for (int row = 0; row < size; row++)
				for (int col = 0; col < size; col++)
					mat [row, col] = row == col ? 1.0f : 0.0f;
		}

		// Compute rotation that brings unit length A to nearby unit length B.

		public void rotate (float[] A, float[] B)
		{
			computeRotation (rot, A, B);
			multiply (rot);
		}

		public void computeRotation (float[,] rot, float[] A, float[] B)
		{

			// Start with matrix I + product ( 2*transpose(B-A) , A )

			identity (rot);
			for (int row = 0; row < size; row++)
				for (int col = 0; col < size; col++)
					rot [row, col] += 2 * (B [row] - A [row]) * A [col];

			// Iterate until matrix is numerically orthonormal:

			for (float totalError = 1.0f; totalError >= 0.00001f ;) {

				// Initialize each row error to 0:

				for (int i = 0; i < size; i++)
					for (int k = 0; k < size; k++)
						err [i, k] = 0.0f;

				// Add to error between each pair of rows:

				for (int i = 0; i < size - 1; i++) {
					for (int j = i + 1; j < size; j++) {
						float[] row1, row2;
						row1 = new float[size];
						row2 = new float[size];
						for (int k = 0; k < size; k++) {
							row1 [k] = rot [i, k];
							row2 [k] = rot [j, k];
						}
						float t = dot (row1, row2);
						for (int k = 0; k < size; k++) {
							err [i, k] += rot [j, k] * t / 2.0f;
							err [j, k] += rot [i, k] * t / 2.0f;
						}
					}
				}

				// For each row, subtract errors and normalize:

				totalError = 0.0f;
				for (int i = 0; i < size; i++) {
					for (int k = 0; k < size; k++) {
						rot [i, k] -= err [i, k];
						totalError += err [i, k] * err [i, k];
					}
					float[] row = new float[size];
					for (int k = 0; k < size; k++) {
						row [k] = rot [i, k];
					}
					normalize (rot, i, row);
				}
			}
		}

		public void multiply (float[,] src)
		{
			multiply (src, mat, tmp);
			copy (tmp, mat);
		}

		public void multiply (float[,] a, float[,] b, float[,] dst)
		{
			for (int row = 0; row < size; row++)
				for (int col = 0; col < size; col++) {
					dst [row, col] = 0.0f;
					for (int k = 0; k < size; k++)
						dst [row, col] += a [row, k] * b [k, col];
				}
		}

		public void transform (float[] src, float[] dst)
		{
			transform (mat, src, dst);
		}

		public void transform (float[,] mat, float[] src, float[] dst)
		{
			for (int row = 0; row < size; row++) {
				dst [row] = 0.0f;
				for (int col = 0; col < size; col++)
					dst [row] += mat [row, col] * src [col];
			}
		}

		public void copy (float[,] src, float[,] dst)
		{
			for (int row = 0; row < size; row++)
				for (int col = 0; col < size; col++)
					dst [row, col] = src [row, col];
		}

		public float dot (float[] a, float[] b)
		{
			float t = 0.0f;
			for (int k = 0; k < size; k++)
				t += a [k] * b [k];
			return t;
		}

		public void normalize (float[,] a, int i, float[] b)
		{
			float s = (float)Math.Sqrt (dot (b, b));
			for (int k = 0; k < size; k++)
				a [i, k] /= s;
		}

		public string round (float t)
		{
			return "" + ((int)(t * 1000) / 1000.0f);
		}

		public void transpose (float[,] src, float[,] dst)
		{
			for (int row = 0; row < size; row++)
				for (int col = 0; col < size; col++)
					dst [col, row] = src [row, col];
		}
	}

	public class HyperCube
	{
		int size;
		GameObject[] edges;
		public Vector4[] srcVertices;
		public Vector4[] vertices;
		Vector2[] index;
		Transform parentobj;
		GameObject[] spheres;

		void setparent (Transform par)
		{
			for (int i = 0; i < size; i++) {
				edges [i].transform.parent = par;
			}
		}

		public HyperCube (Transform par)
		{
			parentobj = par;
			size = 32;
			srcVertices = new Vector4[16];
			vertices = new Vector4[16];
			int n = 0;
			for (int i = -1; i <= 1; i += 2)
				for (int j = -1; j <= 1; j += 2)
					for (int k = -1; k <= 1; k += 2)
						for (int l = -1; l <= 1; l += 2) {
							vertices [n] = new Vector4 ((float)l * 0.175f, (float)k * 0.175f, (float)j * 0.175f, (float)i * 0.175f);
							srcVertices [n++] = new Vector4 ((float)l * 0.175f, (float)k * 0.175f, (float)j * 0.175f, (float)i * 0.175f);
						}
			index = new Vector2[32];
			index [0] = new Vector2 (0, 1);
			index [1] = new Vector2 (2, 3);
			index [2] = new Vector2 (4, 5);
			index [3] = new Vector2 (6, 7);
			index [4] = new Vector2 (8, 9);
			index [5] = new Vector2 (10, 11);
			index [6] = new Vector2 (12, 13);
			index [7] = new Vector2 (14, 15);
			index [8] = new Vector2 (0, 2);
			index [9] = new Vector2 (1, 3);
			index [10] = new Vector2 (4, 6);
			index [11] = new Vector2 (5, 7);
			index [12] = new Vector2 (8, 10);
			index [13] = new Vector2 (9, 11);
			index [14] = new Vector2 (12, 14);
			index [15] = new Vector2 (13, 15);
			index [16] = new Vector2 (0, 4);
			index [17] = new Vector2 (1, 5);
			index [18] = new Vector2 (2, 6);
			index [19] = new Vector2 (3, 7);
			index [20] = new Vector2 (8, 12);
			index [21] = new Vector2 (9, 13);
			index [22] = new Vector2 (10, 14);
			index [23] = new Vector2 (11, 15);
			index [24] = new Vector2 (0, 8);
			index [25] = new Vector2 (1, 9);
			index [26] = new Vector2 (2, 10);
			index [27] = new Vector2 (3, 11);
			index [28] = new Vector2 (4, 12);
			index [29] = new Vector2 (5, 13);
			index [30] = new Vector2 (6, 14);
			index [31] = new Vector2 (7, 15);

			edges = new GameObject[32];
			spheres = new GameObject[16];

			for (int i = 0; i < 16; i++) {
				Vector3 pos = new Vector3 (vertices [i].x, vertices [i].y, vertices [i].z);
				GameObject verObj = GameObject.CreatePrimitive (PrimitiveType.Sphere);
				verObj.transform.position = pos;
				verObj.transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);
				spheres [i] = verObj;
				spheres [i].transform.parent = par;
					
			}

			for (int i = 0; i < size; i++) {
				int start, end;
				start = (int)index [i].x;
				end = (int)index [i].y;
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
			}

			// This is for testing the algorithm



			setparent (par);
		}

        public Vector3 get3dver (int i) {
            Vector3 rst;
            rst = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);
            return rst;
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

			for (int i = 0; i < 16; i++) {
				Vector3 pos = new Vector3 (vertices [i].x, vertices [i].y, vertices [i].z);
				spheres [i].transform.localPosition = pos;
				spheres [i].transform.localScale = new Vector3 (0.01f, 0.01f, 0.01f);

			}


			for (int i = 0; i < 32; i++) {
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


	public class FourDWiiMoteReceiver : WiiGlobalReceiver, IGlobalWiiMoteBHandler,  IGlobalWiiMoteAHandler
	{
		public Transform box;
		public Trackball trackball;
		public HyperCube cube;
		public Vector3 A_;
		public Vector3 B_;
		public Vector4 A, B;
		bool isbutton;
		public GameObject Trackball;
		float radius;
		public Vector3 movement;


		void UpdateRotation (HyperCube cube, Trackball trackball, Vector4 A_, Vector4 B_)
		{
			
			float[] A = new float[4]{ A_.x, A_.y, A_.z, A_.w };
			float[] B = new float[4]{ B_.x, B_.y, B_.z, B_.w };

			trackball.rotate (A, B);

			for (int i = 0; i < 16; i++) {

				float[] src = new float[4];
				src [0] = cube.srcVertices [i].x;
				src [1] = cube.srcVertices [i].y;
				src [2] = cube.srcVertices [i].z;
				src [3] = cube.srcVertices [i].w;
				float[] dst = new float[4];
	
				trackball.transform (src, dst);

				cube.updatepoint4 (dst, i);
				cube.update_edges ();
			}
		}

		// Use this for initialization
		void Start ()
		{
			box = this.GetComponent<Transform> ();
			trackball = new Trackball (4);
			cube = new HyperCube (box);
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
					UpdateRotation (cube, trackball, A, B);
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
