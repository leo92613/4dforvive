using UnityEngine;
using System.Collections;
using System;

namespace Holojam {
	public class FourDWiiMoteReceiver : WiiGlobalReceiver, IGlobalWiiMoteBHandler {

		// Use this for initialization
		void Start() {

		}

		// Update is called once per frame
		void Update() {

		}

		public void OnGlobalBPress(WiiMoteEventData eventData) {
			Debug.Log(eventData.module.transform.position);
		}

		public void OnGlobalBPressDown(WiiMoteEventData eventData) {
			Debug.Log(eventData.module.transform.position);
		}

		public void OnGlobalBPressUp(WiiMoteEventData eventData) {
			Debug.Log(eventData.module.transform.position);
		}
	}
}

