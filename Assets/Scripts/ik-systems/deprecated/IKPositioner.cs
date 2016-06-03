using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace Holojam {
	public class IKPositioner : MonoBehaviour {
		public List<Transform> IKtransforms;
		private List<Transform> IKtransforms_old;
		public List<string> IKlabels;
		//public Transform body;
		private string lbl;

		private float max_delta_dist = 2f;
		private float max_dist_to_head = 3f;
		private float untracked_time;

		public MasterStream stream;
		// Use this for initialization
		void Start () {
			IKtransforms_old = IKtransforms;
			untracked_time = 0;
		}
		
		// Update is called once per frame
		void Update () {
			for (int i = 0; i < IKtransforms.Count; i++) {
				if(i == 0) {
					lbl = IKlabels[0];
				} else {
					lbl = IKlabels[0] + "_" + IKlabels[i];
				}
				Vector3 pos = stream.getLiveObjectPosition (lbl);
				Quaternion rot = stream.getLiveObjectRotation (lbl);
				if (pos == Vector3.zero) {
					untracked_time += Time.deltaTime;
					IKtransforms[i] = IKtransforms_old[i];
				} else {
					if (true||(Vector3.Distance(IKtransforms[i].position, IKtransforms[0].position) < max_dist_to_head &&
						Vector3.Distance(IKtransforms[i].position, IKtransforms_old[i].position) < max_delta_dist * untracked_time)) {
						IKtransforms[i].position = pos;
						IKtransforms[i].rotation = rot;
						IKtransforms_old[i] = IKtransforms[i];
						untracked_time = 0;
					} else {
						untracked_time += Time.deltaTime;
						IKtransforms[i] = IKtransforms_old[i];
					}
				}
			}
		}
	}
}