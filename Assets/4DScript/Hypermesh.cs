using UnityEngine;
using System.Collections;
using System;



namespace Holojam.Demo.FourthDimension {
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class HyperCubeMesh : MonoBehaviour {

        public Vector4[] srcVertices;
        public Vector4[] vertices;
        public Vector4 center = new Vector4(0,0,0,0);

        private int size;
        private Trackball trackball = new Trackball(4);
        public HyperCubeMesh (){
            srcVertices = new Vector4[16];
            vertices = new Vector4[16];
            int n = 0;
            for (int i = -1; i <= 1; i += 2)
                for (int j = -1; j <= 1; j += 2)
                    for (int k = -1; k <= 1; k += 2)
                        for (int l = -1; l <= 1; l += 2) {
                            vertices[n] = new Vector4((float)l* 0.175f, (float)k* 0.175f, (float)j* 0.175f, (float)i* 0.175f);
                            srcVertices[n++] = new Vector4((float)l* 0.175f, (float)k* 0.175f, (float)j* 0.175f, (float)i* 0.175f);
                        }
           // Debug.Log("init is finished");
        }
        public HyperCubeMesh( Vector4 A_) {
           // Debug.Log(A_);
            center = A_;
            srcVertices = new Vector4[16];
            vertices = new Vector4[16];
            int n = 0;
            for (int i = -1; i <= 1; i += 2)
                for (int j = -1; j <= 1; j += 2)
                    for (int k = -1; k <= 1; k += 2)
                        for (int l = -1; l <= 1; l += 2) {
                            vertices[n] = new Vector4((float)l * 0.175f, (float)k * 0.175f, (float)j * 0.175f, (float)i * 0.175f) + center;
                            srcVertices[n++] = new Vector4((float)l * 0.175f, (float)k * 0.175f, (float)j * 0.175f, (float)i * 0.175f) +center;
                        }
               // Debug.Log("Re-init is done!");
            }
        
        public Vector3 get3dver(int i) {
            float factor = 1 / (1 + vertices[i].w);
            Vector3 rst;
            rst = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);
            return rst;
        }
        public void updatepoint4(float[] src, int i) {
            vertices[i].x = (float)src[0];
            vertices[i].y = (float)src[1];
            vertices[i].z = (float)src[2];
            vertices[i].w = (float)src[3];
        }
    }


    public class Hypermesh : ViveGlobalReceiver, IGlobalTriggerPressSetHandler
        {
        public Transform box; //Drag in the Cube GameObject
        public GameObject[] children;
        public Vector3 A_;
        public Vector3 B_;
        public Vector4 A, B;
        public Vector4 center = new Vector4(0f,0f,0f,0f);
        public Vector4 center_;
        public GameObject parent;
        public GameObject manager;      //Drag in the Manager Object
        public Vector3 movement;
        public HyperCubeMesh cube;

        private bool isTriggerPressed;
        private float radius;
        private Mesh mesh;
        private Vector3[] vertices;
        private int[] faces;

        void updatevertices(Mesh mesh) {
            for (int i = 0; i < 16; i++) {
                vertices[i] = cube.get3dver(i);
            }
            mesh.vertices = vertices;
            mesh.SetIndices(faces, MeshTopology.Quads, 0);
            mesh.RecalculateBounds();
            mesh.Optimize();
            mesh.SetTriangles(mesh.GetTriangles(0), 0);
            GetComponent<MeshCollider>().sharedMesh = null;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }

       public void Setup( Vector3 B2, Vector4 B1) {
            B_ = B2;
            B = B1;
        }
       public void Reg(GameObject root) {
            parent = root;
        }
       public void Init(Vector4 A_) {
            center = A_;
            cube = new HyperCubeMesh(A_);
            A = new Vector4();
            UpdateRotation(cube);
            A = B;

        }

        void Awake()
        {
           // box = this.GetComponent<Transform>();
            cube = new HyperCubeMesh(center);
            #region Faces
            faces = new int[] { 4,0,8,12,
                                6,2,10,14,
                                5,1,9,13,
                                7,3,11,15,
                                2,0,8,10,
                                6,4,12,14,
                                3,1,9,11,
                                7,5,13,15,
                                2,0,4,6,
                                10,8,12,14,
                                3,1,5,7,
                                11,9,13,15,
                                1,0,8,9,
                                5,4,12,13,
                                3,2,10,11,
                                7,6,14,15,
                                1,0,4,5,
                                9,8,12,13,
                                3,2,6,7,
                                11,10,14,15,
                                1,0,2,3,
                                9,8,10,11,
                                5,4,6,7,
                                13,12,14,15};
            vertices = new Vector3[16];
            for(int i =0; i<16; i++) {
                vertices[i] = cube.get3dver(i);
            }
            Mesh tmp = new Mesh();
            updatevertices(tmp);
            #endregion
            mesh = new Mesh();
            for (int i = 0; i < 16; i++) {
                vertices[i] = cube.get3dver(i);
            }
            mesh.vertices = vertices;
            mesh.SetIndices(faces, MeshTopology.Quads, 0);
            mesh.RecalculateBounds();
            mesh.Optimize();
            mesh.SetTriangles(mesh.GetTriangles(0), 0);
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = mesh;
            A_ = new Vector3();
            B_ = new Vector3();
            isTriggerPressed = false;
            A = new Vector4();
            B = new Vector4();
            radius = 1.0f;
            children = new GameObject[8];
        }

       public void UpdateRotation(HyperCubeMesh cube) {

            center_ = new Vector4();
            for (int i = 0; i < 16; i++) {

                float[] src = new float[4];
                src[0] = cube.srcVertices[i].x;
                src[1] = cube.srcVertices[i].y;
                src[2] = cube.srcVertices[i].z;
                src[3] = cube.srcVertices[i].w;
                float[] dst = new float[4];

                manager.GetComponent<Manager>().trackball.transform(src, dst);
                center_.x += dst[0];
                center_.y += dst[1];
                center_.z += dst[2];
                center_.w += dst[3];
                cube.updatepoint4(dst, i);
            }
            center_ = center_ / 16f;
            updatevertices(mesh);
            
        }

        // Update is called once per frame
        void Update()
        {
                if (isTriggerPressed) {
                    //Vector3 relapos = new Vector3();
                    //relapos = (B_ - box.position) * 8f / 3f;
                    //float r = (float)Math.Sqrt(relapos.x * relapos.x + relapos.y * relapos.y + relapos.z * relapos.z);
                    //if (r < radius) {
                    //    B = new Vector4(relapos.x, relapos.y, relapos.z, (float)Math.Sqrt(radius * radius - relapos.x * relapos.x - relapos.y * relapos.y - relapos.z * relapos.z));
                    //} else {
                    //    //float length = relapos.magnitude;
                    //    Vector3 Q = (radius / r) * relapos;
                    //    relapos = Q + box.position;
                    //    B = new Vector4(Q.x, Q.y, Q.z, 0f);
                    //}
                    UpdateRotation(cube);
                    //A = B;
                
            }
        }

        public void OnGlobalTriggerPressDown(ViveEventData eventData) {
            isTriggerPressed = true;
            //B_ = eventData.module.transform.position;
            //Vector3 relapos = new Vector3();
            //relapos = (B_ - box.position) * 8f / 3f;
            //float r = (float)Math.Sqrt(relapos.x * relapos.x + relapos.y * relapos.y + relapos.z * relapos.z);
            //if (r < radius) {
            //    B = new Vector4(relapos.x, relapos.y, relapos.z, (float)Math.Sqrt(radius * radius - relapos.x * relapos.x - relapos.y * relapos.y - relapos.z * relapos.z));
            //} else {
            //    Vector3 Q = (radius / r) * relapos;
            //    B = new Vector4(Q.x, Q.y, Q.z, 0f);
            //}
            //A = B;
           // Debug.Log("Trigger Pressed Down");
        }

        public void OnGlobalTriggerPress(ViveEventData eventData) {
            isTriggerPressed = true;
            B_ = eventData.module.transform.position;
           // Debug.Log("Trigger Pressed ");
        }

        public void OnGlobalTriggerPressUp(ViveEventData eventData) {
            isTriggerPressed = false;
         //   A_ = eventData.module.transform.position;
            B_ = eventData.module.transform.position;
           // Debug.Log("Trigger Pressed up");
        }

        public void OnGlobalTriggerTouchDown(ViveEventData eventData) {
            //throw new NotImplementedException();
        }

        public void OnGlobalTriggerTouch(ViveEventData eventData) {
           // throw new NotImplementedException();
        }

        public void OnGlobalTriggerTouchUp(ViveEventData eventData) {
           // throw new NotImplementedException();
        }

        public void Explode() {
            Destroy(gameObject, 5);
        }

    }
}