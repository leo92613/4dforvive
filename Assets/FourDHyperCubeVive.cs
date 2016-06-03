using UnityEngine;
using System.Collections;
using System;

namespace Holojam.IO {
    public class FourDHyperCubeVive : ViveGlobalReceiver, IGlobalTriggerPressSetHandler, IGlobalGripHandler {
        public Transform box;
        public Trackball trackball;
        public HyperCube cube;
        public Vector3 A_;
        public Vector3 B_;
        public Vector4 A, B;
        bool isbutton;
        public GameObject Trackball;
        float radius;
        public Vector3 movement;


        void UpdateRotation(HyperCube cube, Trackball trackball, Vector4 A_, Vector4 B_) {

            float[] A = new float[4] { A_.x, A_.y, A_.z, A_.w };
            float[] B = new float[4] { B_.x, B_.y, B_.z, B_.w };

            trackball.rotate(A, B);

            for (int i = 0; i < 16; i++) {

                float[] src = new float[4];
                src[0] = cube.srcVertices[i].x;
                src[1] = cube.srcVertices[i].y;
                src[2] = cube.srcVertices[i].z;
                src[3] = cube.srcVertices[i].w;
                float[] dst = new float[4];

                trackball.transform(src, dst);

                cube.updatepoint4(dst, i);
                cube.update_edges();
            }
        }

        // Use this for initialization
        void Start() {
            box = this.GetComponent<Transform>();
            trackball = new Trackball(4);
            cube = new HyperCube(box);
          //  box.position = new Vector3(0f, 1.5f, -0.9f);
            A_ = new Vector3();
            B_ = new Vector3();
            isbutton = false;
            A = new Vector4();
            B = new Vector4();
            //radius = Trackball.GetComponent<SphereCollider> ().radius;
            radius = 1.0f;
            UpdateRotation(cube, trackball, A, B);
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
                UpdateRotation(cube, trackball, A, B);
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
                //float length = relapos.magnitude;
                Vector3 Q = (radius / r) * relapos;
                //relapos = Q + box.position;
                B = new Vector4(Q.x, Q.y, Q.z, 0f);
            }
            A = B;
            Debug.Log("Trigger Pressed Down");
        }

        public void OnGlobalTriggerPress(ViveEventData eventData) {
            isbutton = true;
            B_ = eventData.module.transform.position;
            Debug.Log("Trigger Pressed ");
        }

        public void OnGlobalTriggerPressUp(ViveEventData eventData) {
            isbutton = false;
            A_ = eventData.module.transform.position;
            B_ = eventData.module.transform.position;
            Debug.Log("Trigger Pressed up");
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


    }

}

