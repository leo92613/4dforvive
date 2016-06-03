using UnityEngine;
using System.Collections;

namespace Holojam {
    public class ReferenceLine : MonoBehaviour {

        LineRenderer renderer;

        // Use this for initialization
        void Start() {
            renderer = this.GetComponent<LineRenderer>();
            renderer.SetWidth(0.02f, 0.02f);
        }

        // Update is called once per frame
        void Update() {

            Ray ray = new Ray(transform.position,transform.forward);
            RaycastHit hit;

            if (Physics.Raycast(ray, out hit)) {
                renderer.SetVertexCount(2);
                renderer.SetPosition(0, this.transform.position);
                renderer.SetPosition(1, hit.point);
            } else {
                renderer.SetVertexCount(0);
            }
        }
    }
}

