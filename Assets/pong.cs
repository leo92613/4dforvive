using UnityEngine;
using System.Collections;
namespace Holojam.IO {
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class pong : MonoBehaviour {
        Vector4[] srcVertices, vertices;
        Vector4 center = new Vector4(0, 0, 0, 0);
        int[] faces = new int[] {4,0,8,12,
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
        public void initvertices(Vector4 A_) {
            srcVertices = new Vector4[16];
            vertices = new Vector4[16];
            int n = 0;
            for (int i = -1; i <= 1; i += 2)
                for (int j = -1; j <= 1; j += 2)
                    for (int k = -1; k <= 1; k += 2)
                        for (int l = -1; l <= 1; l += 2) {
                            vertices[n] = new Vector4((float)l * 0.175f, (float)k * 0.175f, (float)j * 0.175f, (float)i * 0.175f) + center;
                            srcVertices[n++] = new Vector4((float)l * 0.175f, (float)k * 0.175f, (float)j * 0.175f, (float)i * 0.175f) + center;
                        }
        }

        public pong() {
            initvertices(center);
        }
        public pong(Vector4 A_) {
            center = A_;
            initvertices(center);
        }
        public Vector3 get3dver(int i) {
            float factor = 2 / (2 + vertices[i].w);
            Vector3 rst;
            rst = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z)*factor;
            return rst;
        }
        public void updatepoint4(float[] src, int i) {
            vertices[i].x = (float)src[0];
            vertices[i].y = (float)src[1];
            vertices[i].z = (float)src[2];
            vertices[i].w = (float)src[3];
        }
        public void setupmesh(Mesh mesh) {
            Vector3[] _vertices;
            _vertices = new Vector3[16];
            for (int i = 0; i < 16; i++)
                _vertices[i] = get3dver(i);
            mesh.vertices = _vertices;
            mesh.SetIndices(faces, MeshTopology.Quads, 0);
            mesh.RecalculateBounds();
            mesh.Optimize();
            mesh.SetTriangles(mesh.GetTriangles(0), 0);
        }
        public void move(Vector4 movement) {
            for (int i = 0; i< 16; i++) {
                vertices[i] += movement;
            }

        }


        Mesh mesh;
        // Use this for initialization
        void Start() {
            mesh = new Mesh();
            setupmesh(mesh);
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = null;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
