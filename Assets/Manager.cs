using UnityEngine;
using System.Collections;
using System;

namespace Holojam.IO {
    public class Manager : ViveGlobalReceiver, IGlobalTriggerPressSetHandler, IGlobalGripHandler, IGlobalTouchpadPressUpHandler , IGlobalTouchpadTouchSetHandler{
        [SerializeField]
        int toggle;
        [SerializeField]
        Vector3 A_;
        [SerializeField]
        Vector3 B_;
        [SerializeField]
        Vector4 A, B;
        Vector2 touch;
        public Transform box;
        bool isbutton;
        public Trackball trackball;
        public Transform root;
        public GameObject initmesh;
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
        void Create(Vector4 _A) {
            GameObject meshClone = (GameObject)Instantiate(initmesh, box.position,box.rotation);
            meshClone.GetComponent<Transform>().parent = box;

            meshClone.GetComponent<Transform>().localScale = new Vector3(1f/0.75f, 1f/0.75f, 1f/0.75f);
            meshClone.GetComponent<Hypermesh>().box = box;
            meshClone.GetComponent<Hypermesh>().manager = this.gameObject;
            meshClone.GetComponent<Hypermesh>().module = module;
            meshClone.GetComponent<Hypermesh>().Setup(B_,B);
            meshClone.GetComponent<Hypermesh>().Init(_A);
        }

        public void UpdateRotation(Trackball trackball, Vector4 A_, Vector4 B_) {

            float[] A = new float[4] { A_.x, A_.y, A_.z, A_.w };
            float[] B = new float[4] { B_.x, B_.y, B_.z, B_.w };

            trackball.rotate(A, B);

        }

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
                UpdateRotation(trackball, A, B);
                A = B;
            }
        }
        public void OnGlobalGripPressDown(ViveEventData eventData) {
            movement = new Vector3();
            movement = box.position - eventData.module.transform.position;
        }

        public void OnGlobalGripPress(ViveEventData eventData) {
          //  box.position = eventData.module.transform.position + movement;

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
            //throw new NotImplementedException();
        }

        public void OnGlobalTriggerTouch(ViveEventData eventData) {
          //  throw new NotImplementedException();
        }

        public void OnGlobalTriggerTouchUp(ViveEventData eventData) {
            //throw new NotImplementedException();
        }
        public void OnGlobalTouchpadPressUp(ViveEventData eventData) {
            Create(neighbors[toggle]);
        }

        void IGlobalTouchpadTouchDownHandler.OnGlobalTouchpadTouchDown(ViveEventData eventData) {
            touch = eventData.touchpadAxis;
        }

        void IGlobalTouchpadTouchHandler.OnGlobalTouchpadTouch(ViveEventData eventData) {
            if (Mathf.Abs(eventData.touchpadAxis.x - touch.x) > 0.3) {
                Vector3 a = new Vector3(touch.x, touch.y, 0);
                Vector3 b = new Vector3(eventData.touchpadAxis.x, eventData.touchpadAxis.y, 0);
                if (a.x * b.y - a.y * b.x < 0)
                    toggle = (toggle + 1) % 8;
                else
                    toggle = (toggle + 7) % 8;
                touch = eventData.touchpadAxis;
            }
           // Debug.Log(faceindex);
        }
        void IGlobalTouchpadTouchUpHandler.OnGlobalTouchpadTouchUp(ViveEventData eventData) {
            //throw new NotImplementedException();
        }
    }
}
