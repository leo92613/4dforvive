using UnityEngine;
using System.Collections;
using System;

namespace Holojam.IO {
    public class Manager : ViveGlobalReceiver, IGlobalTriggerPressSetHandler, IGlobalGripHandler, IGlobalTouchpadPressUpHandler {
        [SerializeField]
        int toggle;
        Vector3 A_;
        Vector3 B_;
        Vector4 A, B;
        public Transform box;
        bool isbutton;
        Trackball trackball;
        float radius;
        public int[] hyperface;
        public HyperCubeMesh hypermesh;
        [SerializeField] Vector3 movement;
        // Use this for initialization
        public Vector3[] vertices;
        [SerializeField] Vector4[] neighbors;
        void Awake() {
            neighbors = new Vector4[8];
            neighbors[0] = new Vector4(1, 0, 0, 0) * 0.175f *2f;
            neighbors[1] = new Vector4(-1, 0, 0, 0) * 0.175f * 2f;
            neighbors[2] = new Vector4(0, 1, 0, 0) * 0.175f * 2f;
            neighbors[3] = new Vector4(0, -1, 0, 0) * 0.175f * 2f;
            neighbors[4] = new Vector4(0, 0, 1, 0) * 0.175f * 2f;
            neighbors[5] = new Vector4(0, 0, -1, 0) * 0.175f * 2f;
            neighbors[6] = new Vector4(0, 0, 0, 1) * 0.175f * 2f;
            neighbors[7] = new Vector4(0, 0, 0, -1) * 0.175f * 2f;
        }

        void Start() {
            toggle = 0;
            trackball = new Trackball(4);
            A_ = new Vector3();
            B_ = new Vector3();
            isbutton = false;
            A = new Vector4();
            B = new Vector4();
            radius = 1.0f;
        }

        // Update is called once per frame
        void Update() {
            if (isbutton) {
                Vector3 relapos = new Vector3();
                relapos = (B_ - box.position) * 8f / 3f;
                float r = (float)Math.Sqrt(relapos.x * relapos.x + relapos.y * relapos.y + relapos.z * relapos.z);
                if (r < radius) {
                    B = new Vector4(relapos.x, relapos.y, relapos.z, (float)Math.Sqrt(radius * radius - relapos.x * relapos.x - relapos.y * relapos.y - relapos.z * relapos.z));
                } else {
                    //float length = relapos.magnitude;
                    Vector3 Q = (radius / r) * relapos;
                    relapos = Q + box.position;
                    B = new Vector4(Q.x, Q.y, Q.z, 0f);
                }
                A = B;
            }
        }
        public void OnGlobalGripPressDown(ViveEventData eventData) {
            movement = new Vector3();
            movement = box.position - eventData.module.transform.position;
        }

        public void OnGlobalGripPress(ViveEventData eventData) {
            box.position = eventData.module.transform.position + movement;

        }

        public void OnGlobalGripPressUp(ViveEventData eventData) {

        }

        public void OnGlobalTriggerPressDown(ViveEventData eventData) {
            isbutton = true;
            B_ = eventData.module.transform.position;
            Vector3 relapos = new Vector3();
            relapos = (B_ - box.position) * 8f / 3f;
            float r = (float)Math.Sqrt(relapos.x * relapos.x + relapos.y * relapos.y + relapos.z * relapos.z);
            if (r < radius) {
                B = new Vector4(relapos.x, relapos.y, relapos.z, (float)Math.Sqrt(radius * radius - relapos.x * relapos.x - relapos.y * relapos.y - relapos.z * relapos.z));
            } else {
                Vector3 Q = (radius / r) * relapos;
                B = new Vector4(Q.x, Q.y, Q.z, 0f);
            }
            A = B;
            // Debug.Log("Trigger Pressed Down");
        }

        public void OnGlobalTriggerPress(ViveEventData eventData) {
            isbutton = true;
            B_ = eventData.module.transform.position;
            // Debug.Log("Trigger Pressed ");
        }

        public void OnGlobalTriggerPressUp(ViveEventData eventData) {
            isbutton = false;
            A_ = eventData.module.transform.position;
            B_ = eventData.module.transform.position;
            // Debug.Log("Trigger Pressed up");
        }

        public void OnGlobalTriggerTouchDown(ViveEventData eventData) {
            throw new NotImplementedException();
        }

        public void OnGlobalTriggerTouch(ViveEventData eventData) {
            throw new NotImplementedException();
        }

        public void OnGlobalTriggerTouchUp(ViveEventData eventData) {
            throw new NotImplementedException();
        }
        public void OnGlobalTouchpadPressUp(ViveEventData eventData) {
            toggle = (toggle + 1) % 8;
        }
    }
}
