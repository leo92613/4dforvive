using System;
using UnityEngine;
namespace Holojam {
	public class ObjectWithOffsetController : MonoBehaviour {
		public string label;
		[HideInInspector]
		public MasterStream mStream;
		public string root_label;
		public void Start() {
			mStream = MasterStream.Instance;
		}
		public void Update() {
			Vector3 position = Vector3.zero;
			Quaternion rotation = Quaternion.identity;
			Vector3 root_position = Vector3.zero;
			Quaternion root_rotation = Quaternion.identity;
			if (mStream != null) {
				position = mStream.getLiveObjectPosition(label);
				rotation = mStream.getLiveObjectRotation(label);
				root_position = mStream.getLiveObjectPosition(root_label);
				root_rotation = mStream.getLiveObjectRotation(root_label);
			}

			SetBodyData(position, rotation, root_position, root_rotation);
		}
		public virtual void SetBodyData(Vector3 pos, Quaternion rot, Vector3 root_position, Quaternion root_rotation) {
			this.transform.localPosition = Quaternion.Inverse(root_rotation) * (pos - root_position);
			this.transform.localRotation = Quaternion.Inverse(root_rotation) * rot;
		}
	}
}