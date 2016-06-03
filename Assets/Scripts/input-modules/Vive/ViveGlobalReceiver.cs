using UnityEngine;
using System.Collections;

namespace Holojam.IO {
	public class ViveGlobalReceiver : MonoBehaviour {

		public ViveControllerModule module;

		protected virtual void OnEnable() {
			ViveControllerModule.receivers.Add(this);
		}

		protected virtual void OnDisable() {
			ViveControllerModule.receivers.Remove(this);
		}
	}
}

