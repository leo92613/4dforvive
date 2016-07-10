using UnityEngine;
using System.Collections;
namespace Holojam.IO {
    public class DemoManagertwo : MonoBehaviour {
        public GameObject shape;
        // Use this for initialization
        void Start() {
            shape.GetComponent<FourDShapeDemotwo>().enabled = false;
        }

        // Update is called once per frame
        void Update() {

        }
        void OnTriggerStay(Collider other) {
            if (other.gameObject.tag == "GameController") {
                //Debug.Log("in Shape one");
                shape.GetComponent<FourDShapeDemotwo>().enabled = true;
            }
        }
        void OnTriggerExit(Collider other) {
            if (other.gameObject.tag == "GameController") {
                shape.GetComponent<FourDShapeDemotwo>().enabled = false;
            }
        }
    }
}
