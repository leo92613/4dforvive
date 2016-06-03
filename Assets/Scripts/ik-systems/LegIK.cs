using UnityEngine;
using System.Collections;

public class LegIK : MonoBehaviour {

    public Transform startEffector;
    public Transform midEffector;
    public Transform endEffector;

    public float scale = 0.1f;
    public float minimumOffset = 0.05f;
    public float maximumOffset = 0.5f;

    private float dist;

    void Start() {
        dist = Vector3.Distance(startEffector.position, endEffector.position);
    }

    // Update is called once per frame
    void Update() {
        if (startEffector && midEffector && endEffector) {
            float d, nks;
            d = Vector3.Distance(startEffector.position, endEffector.position);
            nks = Mathf.Clamp((dist / d) * scale, minimumOffset, maximumOffset);
            midEffector.position = ((startEffector.position + endEffector.position) / 2f) + (nks * endEffector.forward);
        } else {
            Debug.LogError("Error: LegIK " + this.name + " does not have all of its effectors!");
        }
    }
}
