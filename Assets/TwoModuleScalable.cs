using UnityEngine;
using System.Collections;
using System;


namespace Holojam {
	[RequireComponent(typeof(Collider))]
	public class TwoModuleScalable : MonoBehaviour, IWiiMoteBHandler, IHandTwoPoseHandler {


		/// <summary>
		/// Component allowing two modules to grab and scale the object.
		/// </summary>

		private Transform primary;
		private Transform secondary;

		private Vector3 initialScale;
		private float initialDistance;

		////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//

		void Start() {

		}

		void Update() {
			if (primary && secondary) {
				float dist = Vector3.Distance(primary.position, secondary.position);
				this.transform.localScale = initialScale * (dist / initialDistance);
			}
		}

		////////////////////////////////////////////////////
		//
		// EventSystem Functions [I/O]
		//

		public void OnBPressDown(WiiMoteEventData eventData) {
			if (primary == null) {
				primary = eventData.module.transform;
			} else if (secondary == null) {
				secondary = eventData.module.transform;
				initialScale = this.transform.localScale;
				initialDistance = Vector3.Distance(primary.position, secondary.position);
			}
		}

		public void OnBPress(WiiMoteEventData eventData) {
			//
		}

		public void OnBPressUp(WiiMoteEventData eventData) {
			//release primary if primary
			if (primary == eventData.module.transform) {
				primary = null;
			}

			//release secondary if secondary
			if (secondary == eventData.module.transform) {
				secondary = null;
			}
		}

		public void OnHandTwo(HandEventData eventData) {
			//
		}

		public void OnHandTwoDown(HandEventData eventData) {
			if (primary == null) {
				primary = eventData.module.transform;
			} else if (secondary == null) {
				secondary = eventData.module.transform;
				initialScale = this.transform.localScale;
				initialDistance = Vector3.Distance(primary.position, secondary.position);
			}
		}

		public void OnHandTwoUp(HandEventData eventData) {
			//release primary if primary
			if (primary == eventData.module.transform) {
				primary = null;
			}

			//release secondary if secondary
			if (secondary == eventData.module.transform) {
				secondary = null;
			}
		}
	}
}

