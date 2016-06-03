using UnityEngine;
using UnityEngine.Events;
using System.Collections;

namespace Holojam
{
	public class WandController : MonoBehaviour {
		public MasterStream mstream;
		public string label;

		private int button_bits;

		void Start () {

		}
		
		void Update () {
			button_bits = mstream.getLiveObjectButtonBits (label);
		}

		public int getButtonBits() {
			return button_bits;
		}
	}
}