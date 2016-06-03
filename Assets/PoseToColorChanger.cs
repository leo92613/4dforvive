using UnityEngine;
using System.Collections;

namespace Holojam {
	public class PoseToColorChanger : MonoBehaviour {

		public HandModule module;

		protected Renderer renderer;

		void Awake() {
			renderer = this.GetComponent<Renderer>();
		}

		void Update() {
			HandStatus status = module.GetCurrentHandStatus();

			Color c = Color.white;
			switch (status) {
				case HandStatus.One:
					c = Color.red;
					break;
				case HandStatus.Two:
					c = Color.blue;
					break;
				case HandStatus.Three:
					c = Color.cyan;
					break;
				case HandStatus.Four:
					c = Color.green;
					break;
				case HandStatus.Open:
					c = Color.yellow;
					break;
				case HandStatus.Closed:
					c = Color.magenta;
					break;
				case HandStatus.Rockin:
					c = new Color(Random.value, Random.value, Random.value, 1.0f);
					break;
				default:
					c = Color.white;
					break;
			}

			renderer.material.color = c;
		}
		
	}
}

