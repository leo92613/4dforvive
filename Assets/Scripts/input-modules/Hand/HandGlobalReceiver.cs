using UnityEngine;
using System.Collections;

namespace Holojam {
	public class HandGlobalReceiver : MonoBehaviour {

		public HandModule module;

		protected virtual void OnEnable() {
			HandModule.receivers.Add(this);
		}

		protected virtual void OnDisable() {
			HandModule.receivers.Remove(this);
		}
	}
}

