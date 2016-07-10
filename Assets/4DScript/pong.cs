using UnityEngine;
using System.Collections;
namespace Holojam.IO {
    [RequireComponent(typeof(MeshFilter))]
    [RequireComponent(typeof(MeshRenderer))]
    public class pong : MonoBehaviour {
        float scalefactor = 0.08f;
        Vector4[] srcVertices, vertices;
        Vector4 center = new Vector4(0, 0.5f, 0, 0);
        #region Face
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
        #endregion
        Mesh mesh;
        [SerializeField]
        Vector4 speed;
        [SerializeField]
        Vector4 pos = new Vector4();
        float X, Y, Z, W;
        public GameObject hitobject;


// Methods
        public void initvertices(Vector4 A_) {
            srcVertices = new Vector4[16];
            vertices = new Vector4[16];
            int n = 0;
            for (int i = -1; i <= 1; i += 2)
                for (int j = -1; j <= 1; j += 2)
                    for (int k = -1; k <= 1; k += 2)
                        for (int l = -1; l <= 1; l += 2) {
                            vertices[n] = new Vector4((float)l * scalefactor, (float)k * scalefactor, (float)j * scalefactor, (float)i * scalefactor) + center * scalefactor;
                            srcVertices[n++] = new Vector4((float)l * scalefactor, (float)k * scalefactor, (float)j * scalefactor, (float)i * scalefactor) + center * scalefactor;
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
            rst = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z) *factor;
            return rst;
        }
        public void updatepoint4(float[] src, int i) {
            vertices[i].x = (float)src[0];
            vertices[i].y = (float)src[1];
            vertices[i].z = (float)src[2];
            vertices[i].w = (float)src[3];
        }
        public void setupmesh(Mesh mesh) {
           // Vector3 _position = new Vector3();
            Vector3[] _vertices;
            _vertices = new Vector3[16];
            for (int i = 0; i < 16; i++) {
                _vertices[i] = get3dver(i);
              //  _position += _vertices[i];
            }
            //this.transform.position = _position;// / 16f;
            mesh.vertices = _vertices;
            mesh.SetIndices(faces, MeshTopology.Quads, 0);
            mesh.RecalculateBounds();
            mesh.Optimize();
            mesh.SetTriangles(mesh.GetTriangles(0), 0);
            GetComponent<MeshFilter>().mesh = mesh;
            GetComponent<MeshCollider>().sharedMesh = null;
            GetComponent<MeshCollider>().sharedMesh = mesh;
        }
        public void move(Vector4 movement) {

            for (int i = 0; i < 16; i++) {
                vertices[i] = srcVertices[i]+movement;
            }
            float factor = 2 / (2 + movement.w);
            hitobject.GetComponent<Transform>().position = new Vector3(movement.x, movement.y + 1f, movement.z) * factor;

        }
            void changespeed(Vector4 _speed) {
                speed += _speed;
            }
            void checkboundary(Vector4 _pos) {

              //   Debug.Log(vertices[1]);
            bool isout = false;
                if (_pos.x > X) {
                    pos.x = 2 * X - _pos.x;
                    changespeed(new Vector4(2 * (-speed.x), 0, 0, 0));
                    isout = true;
                }
                if (_pos.x < -X) {
                    pos.x = 2 * (-X) - _pos.x;
                    changespeed(new Vector4(2 * (-speed.x), 0, 0, 0));
                }
                if (_pos.x >= -X && _pos.x <= X)
                    pos.x = _pos.x;
                if (_pos.y > Y) {
                    pos.y = 2 * Y - _pos.y;
                    changespeed(new Vector4(0, 2 * (-speed.y), 0, 0));
                    isout = true;
                }
                if (_pos.y < -Y) {
                    pos.y = 2 * (-Y) - _pos.y;
                    changespeed(new Vector4(0, 2 * (-speed.y), 0, 0));
                    isout = true;
                }
                if (_pos.y >= -Y && _pos.y <= Y)
                    pos.y = _pos.y;
                if (_pos.z > Z) {
                    pos.z = 2 * Z - _pos.z;
                    changespeed(new Vector4(0, 0, 2 * (-speed.z), 0));
                    isout = true;
                }
                if (_pos.z < -Z) {
                    pos.z = 2 * (-Z) - _pos.z;
                    changespeed(new Vector4(0, 0, 2 * (-speed.z), 0));
                    isout = true;
                }
                if (_pos.z >= -Z && _pos.z <= Z)
                    pos.z = _pos.z;
                if (_pos.w > W) {
                    pos.w = 2 * W - _pos.w;
                    changespeed(new Vector4(0, 0, 0, 2 * (-speed.w)));
                    isout = true;
                }
                if (_pos.w < -W) {
                    pos.w = 2 * (-W) - _pos.w;
                    changespeed(new Vector4(0, 0, 0, 2 * (-speed.w)));
                    isout = true;
                }
                if (_pos.w >= -W && _pos.w <= W)
                    pos.w = _pos.w;

                if (isout)
                    StartCoroutine(pulse());

            }
        void initspeed() {
                float x, y, z, w;
                x = Random.Range(0.1f, 0.5f);
                y = Random.Range(0.1f, 0.5f);
                z = Random.Range(0.1f, 0.5f);
                w = Random.Range(0.1f, 0.5f);
                speed = new Vector4(x, y, z, w);
            }
        IEnumerator pulse() {
                SteamVR_Controller.Input(2).TriggerHapticPulse(1500);
                yield return null;
            }
        void hit(Vector3 _speed) {
            speed = new Vector4(_speed.x,_speed.y,_speed.z, -speed.w);
        }
        



        // Use this for initialization
        void Start() {
            mesh = new Mesh();
            setupmesh(mesh);
            X = 1f;
            Y = 1f;
            Z = 1f;
            W = 1f;
            initspeed();

        }

        // Update is called once per frame
        void Update() {
            //float x = Random.Range(0.1f, 0.5f);
            //float y = Random.Range(0.1f, 0.5f);
            // float z = Random.Range(0.1f, 0.5f);
            // float w = Random.Range(0.1f, 0.5f);
            // Vector4 speed = new Vector4(x, y, z, w);
            Vector4 tmp = new Vector4();
            tmp = pos + speed * Time.deltaTime;
            //Debug.Log(tmp);
            checkboundary(tmp);
            move(pos);
            setupmesh(mesh);
        }
        void OnCollisionEnter(Collision other) {
            if (other.gameObject.tag == "Player") {
                Debug.Log("HIT!!!!");
                StartCoroutine(pulse());
                hit(other.rigidbody.velocity);
            }

        }
    }
}
