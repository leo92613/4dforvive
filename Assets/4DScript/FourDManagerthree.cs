using UnityEngine;
using System.Collections;
namespace Holojam.IO {
    public class FourDManagerthree : MonoBehaviour {
        public GameObject shape;
        // Use this for initialization
        void Start() {
            shape.GetComponent<FourDShapeDemoHypercube>().enabled = false;
        }

        // Update is called once per frame
        void Update() {

        }
        void OnTriggerStay(Collider other) {
            if (other.gameObject.tag == "GameController") {
                //Debug.Log("in Shape one");
                shape.GetComponent<FourDShapeDemoHypercube>().enabled = true;
            }
        }
        void OnTriggerExit(Collider other) {
            if (other.gameObject.tag == "GameController") {
                shape.GetComponent<FourDShapeDemoHypercube>().enabled = false;
            }
        }
    }
}