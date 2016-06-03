using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Holojam {
	[RequireComponent(typeof(Collider))]
	public class OneModuleGrabbable : MonoBehaviour, IWiiMoteBHandler, IHandTwoPoseHandler {

		/// <summary>
		/// Component allowing a module to grab an object. Will overload when 2+ modules grab.
		/// </summary>

		private List<KeyValuePair<Transform, float>> transforms = new List<KeyValuePair<Transform, float>>();


		////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//

		void Start() {

		}

		void Update() {
			if (transforms.Count == 1) {
				KeyValuePair<Transform, float> pair = transforms[0];
				this.transform.position = pair.Key.position + pair.Key.forward * pair.Value;
			}
		}

		////////////////////////////////////////////////////
		//
		// EventSystem Functions [I/O]
		//

		public void OnBPressDown(WiiMoteEventData eventData) {
			Transform tform = eventData.module.transform;
			float dist = Vector3.Distance(this.transform.position, tform.position);
			transforms.Add(new KeyValuePair<Transform,float>(tform, dist));
		}

		public void OnBPress(WiiMoteEventData eventData) {
			
		}

		public void OnBPressUp(WiiMoteEventData eventData) {
			foreach (KeyValuePair<Transform, float> pair in transforms) {
				if (pair.Key == eventData.module.transform) {
					transforms.Remove(pair);
					return;
				}
			}
		}

		public void OnHandTwoDown(HandEventData eventData) {
			Transform tform = eventData.module.transform;
			float dist = Vector3.Distance(this.transform.position, tform.position);
			transforms.Add(new KeyValuePair<Transform, float>(tform, dist));
		}

		public void OnHandTwo(HandEventData eventData) {

		}

		public void OnHandTwoUp(HandEventData eventData) {
			foreach (KeyValuePair<Transform, float> pair in transforms) {
				if (pair.Key == eventData.module.transform) {
					transforms.Remove(pair);
					return;
				}
			}
		}
	}
}

