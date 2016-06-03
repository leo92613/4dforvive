using UnityEngine;
using System.Collections;

namespace Holojam {
	public class HoloIKSystem : MonoBehaviour {

		public string label;

		public string[] mocapLabels = new string[4] { "_lefthand", "_righthand", "_leftankle", "_rightankle" };

		public Transform head;
		public Transform leftHand;
		public Transform rightHand;
		public Transform leftFoot;
		public Transform rightFoot;

		public Vector3 handOffset;
		public Vector3 footOffset;

		protected MasterStream stream;

		void Awake() {
			stream = MasterStream.Instance;
			this.InitObjectControllers();
		}

		void InitObjectControllers() {
			ObjectController c;
			GameObject[] objs = new GameObject[5] {head.gameObject,leftHand.gameObject,rightHand.gameObject,
										    leftFoot.gameObject, rightFoot.gameObject };
			string[] labels = new string[5] { label, label + mocapLabels[0], label + mocapLabels[1], label + mocapLabels[2], label + mocapLabels[3]};
			Vector3[] offsets = new Vector3[5] {Vector3.zero, new Vector3(handOffset.x*-1,handOffset.y,handOffset.z), handOffset,
													new Vector3(footOffset.x*-1,footOffset.y,footOffset.z), footOffset };
			for (int i = 0; i < 5; i++) {
				c = objs[i].GetComponent<ObjectController>();
				if (c == null)
					c = objs[i].AddComponent<ObjectController>();
				c.label = labels[i];
				c.offset = offsets[i];
			}
		}



		void Update() {
            this.PositionBody();
		}

        Vector3 _nb;
        void PositionBody() {
            float la = leftFoot.rotation.eulerAngles.y;
            float ra = rightFoot.rotation.eulerAngles.y;
            float ny = ((Mathf.Abs(la - ra) > 180) ? (la + ra + 360) : (la + ra)) / 2f;
            ny = ny % 360;

            transform.rotation = Quaternion.Euler(0, ny, 0);
            //Debug.Log(ny);

            _nb = new Vector3((leftFoot.position.x + rightFoot.position.x) / 2f, 0f, (leftFoot.position.z + rightFoot.position.z) / 2f);
            this.transform.position = _nb;
        }
	}
}

