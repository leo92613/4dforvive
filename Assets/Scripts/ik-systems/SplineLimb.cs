using UnityEngine;
using System.Collections;

namespace Holojam {
	public class SplineLimb : MonoBehaviour {

		public Color color;
		public Material material;
		public int detail = 10;

        public float startWidth = 0.05f;
        public float endWidth = 0.03f;

		public Transform startEffector;
		public Transform midEffector;
		public Transform endEffector;

		private LineRenderer line;

		void Awake() {
			line = this.GetComponent<LineRenderer>();
            if (material)
			    line.material = material;
		}

		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {
			if (startEffector && midEffector && endEffector) {
				this.transform.position = endEffector.position;
				line.SetColors(color, color);
				line.SetWidth(startWidth, endWidth);
				this.BuildLine();
			} else {
				Debug.LogError("Error: Spline limb " + this.name + " does not have all of its effectors!");
			}

		}

		void BuildLine() {

            Vector3[] points = new Vector3[3] {startEffector.position, midEffector.position, endEffector.position};
            Vector3[] interpPoints = getPoints(points);

            line.SetVertexCount(interpPoints.Length);
            line.SetPositions(interpPoints);
		}

		Vector3 interp(Vector3[] P, float t) {
			return Vector3.Lerp(Vector3.Lerp(P[0], P[1], t), Vector3.Lerp(P[1], P[2], t), t);
		}

		Vector3[] getPoints(Vector3[] points) {
			Vector3[] returnPoints = new Vector3[detail];
			for (int i = 0; i < detail; i++) {
				if (points.Length <= 3)
					returnPoints[i] = interp(points, (float)i / (detail - 1));
				else
					returnPoints[i] = interp(points, (float)i / (detail - 1));
			}
			return returnPoints;
		}

		Vector3 interpb3(Vector3[] points, float t) {
			Vector3 vector = new Vector3();
			vector.x = b3(t, points[0].x, points[1].x, points[2].x, points[3].x);
			vector.y = b3(t, points[0].y, points[1].y, points[2].y, points[3].y);
			vector.z = b3(t, points[0].z, points[1].z, points[2].z, points[3].z);
			return vector;
		}

		// Cubic Bezier Functions
		float b3p0(float t, float p) {
			float k = 1 - t;
			return k * k * k * p;
		}

		float b3p1(float t, float p) {
			float k = 1 - t;
			return 3 * k * k * t * p;
		}

		float b3p2(float t, float p) {
			float k = 1 - t;
			return 3 * k * t * t * p;
		}

		float b3p3(float t, float p) {
			return t * t * t * p;
		}

		float b3(float t, float p0, float p1, float p2, float p3) {
			return b3p0(t, p0) + b3p1(t, p1) + b3p2(t, p2) + b3p3(t, p3);
		}
	}

}
