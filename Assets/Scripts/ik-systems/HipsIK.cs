using UnityEngine;
using System.Collections;

namespace Holojam {
    public class HipsIK : MonoBehaviour {

        public HoloIKSystem system;
        public Transform headEffector;
        public Transform hipsEffector;

        private Vector3 offset;
        private Vector3 pos;
        private Vector3 _nb;

        // Use this for initialization
        void Start() {
            offset = headEffector.position - hipsEffector.position;

        }

        // Update is called once per frame
        void Update() {
            _nb = system.transform.position;
            pos = SolveForHipsPosition();
            hipsEffector.position = pos;
            hipsEffector.up = (headEffector.position - hipsEffector.position).normalized;
        }

        Vector3 SolveForHipsPosition() {
            Vector3 hit = Vector3.zero;

            if (SolveHipsIntersection(out hit)) {
                return hit;
            } else {
                hit = headEffector.position - (headEffector.position - _nb).normalized * Vector3.Distance(offset, Vector3.zero);
                Vector3 headxz = Vector3.Scale(headEffector.position, Vector3.right + Vector3.forward);
                Vector3 offsetxz = (headxz - _nb) * -0.5f;
                hit += offsetxz;
            }


            return hit;
        }

        bool SolveHipsIntersection(out Vector3 hitPoint) {

            float d, q, t, l2, r2, m2;
            hitPoint = Vector3.zero;

            Vector3 s2r = headEffector.position - _nb;
            l2 = s2r.sqrMagnitude;
            d = Vector3.Dot(s2r, Vector3.up);
            r2 = Mathf.Pow(Vector3.Distance(offset, Vector3.zero), 2f);

            if (d < 0.0f && l2 > r2) {
                return false;
            }

            m2 = (l2 - (d * d));
            if (m2 > r2) {
                return false;
            }

            q = Mathf.Sqrt(r2 - m2);


            if (l2 > r2) {
                t = d - q;
            } else {
                t = d + q;
            }

            Vector3 v = Vector3.up * t;

            hitPoint = _nb + v;
            return true;
        }
    }
}

