using UnityEngine;
using System.Collections;
using System;

namespace Holojam.Demo.FourthDimension {
    public class Hyperface : ViveGlobalReceiver, IGlobalTriggerPressSetHandler , IGlobalTouchpadTouchSetHandler {

        public Vector4 center;
        public Transform box;           //Drag in the Cube GameObject
        public GameObject manager;      //Drag in the Manager GameObject
        public int[] hyperface;
        public HyperCubeMesh hypermesh;
        public Vector3[] vertices;
        public int faceindex;
        public int[] faces;
        public Mesh mesh;
        public GameObject parent;

        private Vector3 _A, _B;
        private Vector4 A, B;
        private Vector2 touch;
        private bool isTriggerPressed;
        private float radius;

        // Use this for initialization

        void Awake() {
            faceindex = 0;
            _A = new Vector3();
            _B = new Vector3();
            isTriggerPressed = false;
            A = new Vector4();
            B = new Vector4();
            center = new Vector4();
            radius = 1.0f;              //Setup the radius of the sphere collider
            hypermesh = new HyperCubeMesh(center);
            vertices = new Vector3[8];
            hyperface = new int[] {       1,3,5,7,9,11,13,15,
                                          0,2,4,6,8,10,12,14,
                                          2,3,6,7,10,11,14,15,
                                          0,1,4,5,8,9,12,13,
                                          4,5,6,7,12,13,14,15,
                                          0,1,2,3,8,9,10,11,
                                          8,9,10,11,12,13,14,15,
                                          0,1,2,3,4,5,6,7};
            for (int i = 0; i < 8; i++) {
                vertices[i] = hypermesh.get3dver(hyperface[i + faceindex * 8]);
            }

            mesh = new Mesh();
            faces = new int[24] {
                                   2,0,4,6,
                                   3,1,5,7,
                                   1,0,4,5,
                                   3,2,6,7,
                                   1,0,2,3,
                                   5,4,6,7
            };
            mesh.vertices = vertices;
            mesh.SetIndices(faces, MeshTopology.Quads, 0);
            mesh.RecalculateBounds();
            mesh.Optimize();
            GetComponent<MeshFilter>().mesh = mesh;

        }
        void UpdateRotation(HyperCubeMesh cube) {\
            for (int i = 0; i < 16; i++) {
                float[] src = new float[4];
                src[0] = cube.srcVertices[i].x;
                src[1] = cube.srcVertices[i].y;
                src[2] = cube.srcVertices[i].z;
                src[3] = cube.srcVertices[i].w;
                float[] dst = new float[4];
                manager.GetComponent<Manager>().trackball.transform(src, dst);
                cube.updatepoint4(dst, i);
            }

        }
        // Update is called once per frame
        void Update() {
            for (int i = 0; i < 8; i++) {
                vertices[i] = hypermesh.get3dver(hyperface[i + faceindex * 8]);
            }

            if (isTriggerPressed) {
                Vector3 relapos = new Vector3();
                relapos = (_B - box.position) * 8f / 3f;
                float r = (float)Math.Sqrt(relapos.x * relapos.x + relapos.y * relapos.y + relapos.z * relapos.z);
                if (r < radius) {
                    B = new Vector4(relapos.x, relapos.y, relapos.z, (float)Math.Sqrt(radius * radius - relapos.x * relapos.x - relapos.y * relapos.y - relapos.z * relapos.z));
                } else {
                    //float length = relapos.magnitude;
                    Vector3 Q = (radius / r) * relapos;
                    relapos = Q + box.position;
                    B = new Vector4(Q.x, Q.y, Q.z, 0f);
                }
                UpdateRotation(hypermesh);
                A = B;
            }
            mesh.vertices = vertices;
            mesh.SetIndices(faces, MeshTopology.Quads, 0);
            mesh.RecalculateBounds();
            mesh.Optimize();
        }
        public void Renew() {

            hypermesh = new HyperCubeMesh(center);
            vertices = new Vector3[8];
            for (int i = 0; i < 8; i++) {
                vertices[i] = hypermesh.get3dver(hyperface[i + faceindex * 8]);
            }
            mesh.vertices = vertices;
            mesh.SetIndices(faces, MeshTopology.Quads, 0);
            mesh.RecalculateBounds();
            mesh.Optimize();
            UpdateRotation(hypermesh);
        }
        public void OnGlobalTriggerPressDown(ViveEventData eventData) {
            isTriggerPressed = true;
            _B = eventData.module.transform.position;
            Vector3 relapos = new Vector3();
            relapos = (_B - box.position) * 8f / 3f;
            float r = (float)Math.Sqrt(relapos.x * relapos.x + relapos.y * relapos.y + relapos.z * relapos.z);
            if (r < radius) {
                B = new Vector4(relapos.x, relapos.y, relapos.z, (float)Math.Sqrt(radius * radius - relapos.x * relapos.x - relapos.y * relapos.y - relapos.z * relapos.z));
            } else {
                Vector3 Q = (radius / r) * relapos;
                B = new Vector4(Q.x, Q.y, Q.z, 0f);
            }
            A = B;
        }

        public void OnGlobalTriggerPress(ViveEventData eventData) {
            isTriggerPressed = true;
            _B = eventData.module.transform.position;
            // Debug.Log("Trigger Pressed ");
        }

        public void OnGlobalTriggerPressUp(ViveEventData eventData) {
            isTriggerPressed = false;
            _A = eventData.module.transform.position;
            _B = eventData.module.transform.position;
            // Debug.Log("Trigger Pressed up");
        }

        public void OnGlobalTriggerTouchDown(ViveEventData eventData) {
            //throw new NotImplementedException();
        }

        public void OnGlobalTriggerTouch(ViveEventData eventData) {
           // throw new NotImplementedException();
        }

        public void OnGlobalTriggerTouchUp(ViveEventData eventData) {
            //throw new NotImplementedException();
        }


        void IGlobalTouchpadTouchDownHandler.OnGlobalTouchpadTouchDown(ViveEventData eventData) {
            touch = eventData.touchpadAxis;
        }

        void IGlobalTouchpadTouchHandler.OnGlobalTouchpadTouch(ViveEventData eventData) {
            if (Mathf.Abs(eventData.touchpadAxis.x - touch.x )> 0.3) {
                Vector3 a = new Vector3(touch.x, touch.y, 0);
                Vector3 b = new Vector3(eventData.touchpadAxis.x, eventData.touchpadAxis.y, 0);
                if (a.x*b.y - a.y*b.x < 0)
                    faceindex = (faceindex + 1) % 8;
                else
                    faceindex = (faceindex + 7) % 8;
                touch = eventData.touchpadAxis;
            }
        }

        void IGlobalTouchpadTouchUpHandler.OnGlobalTouchpadTouchUp(ViveEventData eventData) {
           // throw new NotImplementedException();
        }
    }
}
