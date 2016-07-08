using UnityEngine;
using System.Collections;
namespace Holojam.IO {
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class pongface {
        Vector4[] srcVertices, vertices;
        int[] hyperface = new int[] {     1,3,5,7,9,11,13,15,
                                          0,2,4,6,8,10,12,14,
                                          2,3,6,7,10,11,14,15,
                                          0,1,4,5,8,9,12,13,
                                          4,5,6,7,12,13,14,15,
                                          0,1,2,3,8,9,10,11,
                                          8,9,10,11,12,13,14,15,
                                          0,1,2,3,4,5,6,7
        };
        int faceindex = 0;
        int[] faces = new int[24] {
               2,0,4,6,
               3,1,5,7,
               1,0,4,5,
               3,2,6,7,
               1,0,2,3,
               5,4,6,7
            };
        public void initvertices() {
            srcVertices = new Vector4[16];
            vertices = new Vector4[16];
            int n = 0;
            for (int i = -1; i <= 1; i += 2)
                for (int j = -1; j <= 1; j += 2)
                    for (int k = -1; k <= 1; k += 2)
                        for (int l = -1; l <= 1; l += 2) {
                            vertices[n] = new Vector4((float)l, (float)k, (float)j, (float)i);
                            srcVertices[n++] = new Vector4((float)l, (float)k, (float)j, (float)i);
                        }
        }
        public pongface() {
            initvertices();
        }
        public pongface(int i) {
            faceindex = i;
            initvertices();
        }
        public Vector3 get3dver(int i) {
            float factor = 2 / (2+ vertices[i].w);
            Vector3 rst;
            rst = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z)*factor;
            return rst;
        }

        public void setupmesh(Mesh mesh) {
        Vector3[] _vertices;
        _vertices = new Vector3[8];
        for (int i = 0; i< 8; i++) 
        _vertices[i] = get3dver(hyperface[i + faceindex * 8]);
        mesh.vertices = _vertices;
        mesh.SetIndices(faces, MeshTopology.Quads, 0);
        mesh.RecalculateBounds();
        mesh.Optimize();
        mesh.SetTriangles(mesh.GetTriangles(0), 0);
        }      
    }


    public class GameCube : MonoBehaviour {
        private pongface hyperfaces;
        public int faceindex;
        Mesh mesh;
        // Use this for initialization
        void Start() {
            hyperfaces = new pongface(faceindex);
            mesh = new Mesh();
            hyperfaces.setupmesh(mesh);
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = null;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
