using UnityEngine;
using System.Collections;

namespace Holojam {
	public class ArmIK : MonoBehaviour {

		public bool Right = false;

		public Transform startEffector;
		public Transform midEffector;
		public Transform endEffector;

		public float startToMidDistance = 0.18f;
		public float midToEndDistance = 0.18f;

		private Vector3 C, D;
		private float cc, x, y;

		void Update() {
			if (startEffector && midEffector && endEffector) {
				this.IK(startEffector, midEffector, endEffector, startToMidDistance, midToEndDistance);
			} else {
				Debug.LogError("Error: ArmIK " + this.name + " does not have all of its effectors!");
			}
		}


		protected void IK(Transform startEffector, Transform midEffector, Transform endEffector, float a, float b) {

			C = endEffector.position - startEffector.position;
			D = Hint(C);

			//smoothing
			float t = Mathf.Sqrt(Vector3.Dot(C, C)) / (a + b) - .2f;
			t = Mathf.Max(0, Mathf.Min(1, t * t * (3 - t - t)));
			t = .9f + .2f * Mathf.Sqrt(t);
			a *= t;
			b *= t;
			//smoothing

			cc = Vector3.Dot(C, C);
			x = (1 + (a * a - b * b) / cc) / 2;
			y = Vector3.Dot(C, D) / cc;
			D -= y * C;
			y = Mathf.Sqrt(Mathf.Max(0, a * a - cc * x * x) / Vector3.Dot(D, D));
			D = x * C + y * D;

			midEffector.localPosition = D + startEffector.position;
		}

		protected Vector3 Hint(Vector3 endEffectorPosition) {
			float r = Mathf.Sqrt(0.5f);
			Vector3 c = endEffectorPosition;
			float[] C = { c.x, c.y, c.z };
			float[,,] map;

			if (!Right) {
				map = new float[,,]{
				{{  -1,  0,  0 }  ,  { 0, -r, -r }},
				{{ 1,  0,  0 }  ,  { 0,  0,  1 }},
				{{  0,  1,  0 }  ,  { -1,  0,  0 }},
				{{  0, -1,  0 }  ,  { -r,  0, -r }},
				{{  0,  0,  1 }  ,  { -r, -r,  0 }},
				{{  -r,  0,  r }  ,  { -r,  0, -r }},
				{{ r,  0,  r }  ,  { -r,  0,  r }},
				};
			} else {
				map = new float[,,]{
				{{ 1,  0,  0 }  ,  { 0, -r, -r }},
				{{ -1,  0,  0 }  ,  { 0,  0,  1 }},
				{{ 0,  1,  0 }  ,  { 1,  0,  0 }},
				{{ 0, -1,  0 }  ,  { r,  0, -r }},
				{{ 0,  0,  1 }  ,  { r, -r,  0 }},
				{{ r,  0,  r }  ,  { r,  0, -r }},
				{{ -r,  0,  r }  ,  { r,  0,  r }},
				};
			}

			float[] D = { 0, 0, 0 };
			for (int n = 0; n < 7; n++) {
				float[] thisMap = { map[n, 0, 0], map[n, 0, 1], map[n, 0, 2] };
				float d = dot(thisMap, C);
				if (d > 0) {
					for (int j = 0; j < 3; j++) {
						D[j] += d * map[n, 1, j];
					}
				}
			}

			normalize(D);
			return new Vector3(D[0], D[1], D[2]);
		}

		float dot(float[] a, float[] b) {
			return a[0] * b[0] + a[1] * b[1] + a[2] * b[2];
		}

		float[] normalize(float[] a) {
			float length = Mathf.Sqrt((a[0] * a[0]) + (a[1] * a[1]) + (a[2] * a[2]));
			float[] r = { a[0] / length, a[1] / length, a[2] / length };
			return (r);
		}
	}
}

