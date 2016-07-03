using UnityEngine;
using System.Collections;
namespace Holojam.IO {
    public class pongface {
        Vector4 center;
        Vector4[] srcVertices, vertices;
        int[] hyperface = new int[] {       1,3,5,7,9,11,13,15,
                                          0,2,4,6,8,10,12,14,
                                          2,3,6,7,10,11,14,15,
                                          0,1,4,5,8,9,12,13,
                                          4,5,6,7,12,13,14,15,
                                          0,1,2,3,8,9,10,11,
                                          8,9,10,11,12,13,14,15,
                                          0,1,2,3,4,5,6,7};
        int faceindex = 0;
        int[] faces = new int[24] {
               2,0,4,6,
               3,1,5,7,
               1,0,4,5,
               3,2,6,7,
               1,0,2,3,
               5,4,6,7
            };
        public pongface() {
            center = new Vector4(0f, 0f, 0f, 0f);
            srcVertices = new Vector4[16];
            vertices = new Vector4[16];
            int n = 0;
            for (int i = -1; i <= 1; i += 2)
                for (int j = -1; j <= 1; j += 2)
                    for (int k = -1; k <= 1; k += 2)
                        for (int l = -1; l <= 1; l += 2) {
                            vertices[n] = new Vector4((float)l , (float)k , (float)j , (float)i);
                            srcVertices[n++] = new Vector4((float)l, (float)k , (float)j , (float)i);
                        }
        }
    }


    public class GameCube : MonoBehaviour {
        private Hyperface[] faces;


        void Awake() {

        }
        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
