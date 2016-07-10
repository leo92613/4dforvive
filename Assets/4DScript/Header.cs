using UnityEngine;
using System.Collections;
using System;
namespace Holojam.IO {
    public class Trackball {
        int size;
        float[,] mat, rot, tmp, err;
        bool isDebug = false;

        public Trackball(int size) {
            this.size = size;
            mat = new float[size, size];
            rot = new float[size, size];
            tmp = new float[size, size];
            err = new float[size, size];
            identity();
        }

        public string toString() {
            return toString(mat);
        }

        public string toString(float[,] mat) {
            string s = "{ ";
            for (int row = 0; row < size; row++) {
                s += "{";
                for (int col = 0; col < size; col++)
                    s += round(mat[row, col]) + ",";
                s += "},";
            }
            s += " }";
            return s;
        }

        public void identity() {
            identity(mat);
        }

        public void identity(float[,] mat) {
            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++)
                    mat[row, col] = row == col ? 1.0f : 0.0f;
        }

        // Compute rotation that brings unit length A to nearby unit length B.

        public void rotate(float[] A, float[] B) {
            computeRotation(rot, A, B);
            multiply(rot);
        }

        public void computeRotation(float[,] rot, float[] A, float[] B) {

            // Start with matrix I + product ( 2*transpose(B-A) , A )

            identity(rot);
            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++)
                    rot[row, col] += 2 * (B[row] - A[row]) * A[col];

            // Iterate until matrix is numerically orthonormal:

            for (float totalError = 1.0f; totalError >= 0.00001f;) {

                // Initialize each row error to 0:

                for (int i = 0; i < size; i++)
                    for (int k = 0; k < size; k++)
                        err[i, k] = 0.0f;

                // Add to error between each pair of rows:

                for (int i = 0; i < size - 1; i++) {
                    for (int j = i + 1; j < size; j++) {
                        float[] row1, row2;
                        row1 = new float[size];
                        row2 = new float[size];
                        for (int k = 0; k < size; k++) {
                            row1[k] = rot[i, k];
                            row2[k] = rot[j, k];
                        }
                        float t = dot(row1, row2);
                        for (int k = 0; k < size; k++) {
                            err[i, k] += rot[j, k] * t / 2.0f;
                            err[j, k] += rot[i, k] * t / 2.0f;
                        }
                    }
                }

                // For each row, subtract errors and normalize:

                totalError = 0.0f;
                for (int i = 0; i < size; i++) {
                    for (int k = 0; k < size; k++) {
                        rot[i, k] -= err[i, k];
                        totalError += err[i, k] * err[i, k];
                    }
                    float[] row = new float[size];
                    for (int k = 0; k < size; k++) {
                        row[k] = rot[i, k];
                    }
                    normalize(rot, i, row);
                }
            }
        }

        public void multiply(float[,] src) {
            multiply(src, mat, tmp);
            copy(tmp, mat);
        }

        public void multiply(float[,] a, float[,] b, float[,] dst) {
            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++) {
                    dst[row, col] = 0.0f;
                    for (int k = 0; k < size; k++)
                        dst[row, col] += a[row, k] * b[k, col];
                }
        }

        public void transform(float[] src, float[] dst) {
            transform(mat, src, dst);
        }

        public void transform(float[,] mat, float[] src, float[] dst) {
            for (int row = 0; row < size; row++) {
                dst[row] = 0.0f;
                for (int col = 0; col < size; col++)
                    dst[row] += mat[row, col] * src[col];
            }
        }

        public void copy(float[,] src, float[,] dst) {
            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++)
                    dst[row, col] = src[row, col];
        }

        public float dot(float[] a, float[] b) {
            float t = 0.0f;
            for (int k = 0; k < size; k++)
                t += a[k] * b[k];
            return t;
        }

        public void normalize(float[,] a, int i, float[] b) {
            float s = (float)Math.Sqrt(dot(b, b));
            for (int k = 0; k < size; k++)
                a[i, k] /= s;
        }

        public string round(float t) {
            return "" + ((int)(t * 1000) / 1000.0f);
        }

        public void transpose(float[,] src, float[,] dst) {
            for (int row = 0; row < size; row++)
                for (int col = 0; col < size; col++)
                    dst[col, row] = src[row, col];
        }
    }

    public class HyperCube {
        int size;
        GameObject[] edges;
        public Vector4[] srcVertices;
        public Vector4[] vertices;
        Vector2[] index;
        Transform parentobj;
        GameObject[] spheres;

        void setparent(Transform par) {
            for (int i = 0; i < size; i++) {
                edges[i].transform.parent = par;
            }
        }

        public HyperCube(Transform par) {
            parentobj = par;
            size = 32;
            srcVertices = new Vector4[16];
            vertices = new Vector4[16];
            int n = 0;
            for (int i = -1; i <= 1; i += 2)
                for (int j = -1; j <= 1; j += 2)
                    for (int k = -1; k <= 1; k += 2)
                        for (int l = -1; l <= 1; l += 2) {
                            vertices[n] = new Vector4((float)l * 0.175f, (float)k * 0.175f, (float)j * 0.175f, (float)i * 0.175f);
                            srcVertices[n++] = new Vector4((float)l * 0.175f, (float)k * 0.175f, (float)j * 0.175f, (float)i * 0.175f);
                        }
            index = new Vector2[32];
            index[0] = new Vector2(0, 1);
            index[1] = new Vector2(2, 3);
            index[2] = new Vector2(4, 5);
            index[3] = new Vector2(6, 7);
            index[4] = new Vector2(8, 9);
            index[5] = new Vector2(10, 11);
            index[6] = new Vector2(12, 13);
            index[7] = new Vector2(14, 15);
            index[8] = new Vector2(0, 2);
            index[9] = new Vector2(1, 3);
            index[10] = new Vector2(4, 6);
            index[11] = new Vector2(5, 7);
            index[12] = new Vector2(8, 10);
            index[13] = new Vector2(9, 11);
            index[14] = new Vector2(12, 14);
            index[15] = new Vector2(13, 15);
            index[16] = new Vector2(0, 4);
            index[17] = new Vector2(1, 5);
            index[18] = new Vector2(2, 6);
            index[19] = new Vector2(3, 7);
            index[20] = new Vector2(8, 12);
            index[21] = new Vector2(9, 13);
            index[22] = new Vector2(10, 14);
            index[23] = new Vector2(11, 15);
            index[24] = new Vector2(0, 8);
            index[25] = new Vector2(1, 9);
            index[26] = new Vector2(2, 10);
            index[27] = new Vector2(3, 11);
            index[28] = new Vector2(4, 12);
            index[29] = new Vector2(5, 13);
            index[30] = new Vector2(6, 14);
            index[31] = new Vector2(7, 15);

            edges = new GameObject[32];
            spheres = new GameObject[16];

            for (int i = 0; i < 16; i++) {
                Vector3 pos = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);
                GameObject verObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                verObj.transform.position = pos;
                verObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                spheres[i] = verObj;
                spheres[i].transform.parent = par;

            }

            for (int i = 0; i < size; i++) {
                int start, end;
                start = (int)index[i].x;
                end = (int)index[i].y;
                Vector3 beginpoint_ = new Vector3(vertices[start].x, vertices[start].y, vertices[start].z);
                Vector3 endpoint_ = new Vector3(vertices[end].x, vertices[end].y, vertices[end].z);
                Vector3 pos = Vector3.Lerp(beginpoint_, endpoint_, (float)0.5);
                float distance = Vector3.Distance(beginpoint_, endpoint_);
                GameObject segObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                segObj.transform.position = pos;
                segObj.transform.LookAt(endpoint_);
                segObj.transform.Rotate(new Vector3(1.0f, 0, 0), 90);
                segObj.transform.localScale = new Vector3(0.01f, distance, 0.01f);
                edges[i] = segObj;
            }

            // This is for testing the algorithm



            setparent(par);
        }

        public Vector3 get3dver(int i) {
            Vector3 rst;
            rst = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);
            return rst;
        }
        public void returnpoint4(float[] src, int i) {
            src[0] = (float)vertices[i].x;
            src[1] = (float)vertices[i].y;
            src[2] = (float)vertices[i].z;
            src[3] = (float)vertices[i].w;
        }

        public void updatepoint4(float[] src, int i) {
            vertices[i].x = (float)src[0];
            vertices[i].y = (float)src[1];
            vertices[i].z = (float)src[2];
            vertices[i].w = (float)src[3];
        }

        public void update_edges() {

            for (int i = 0; i < 16; i++) {
                Vector3 pos = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);
                spheres[i].transform.localPosition = pos;
                spheres[i].transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            }


            for (int i = 0; i < 32; i++) {
                int start, end;
                start = (int)index[i].x;
                end = (int)index[i].y;
                Vector3 beginpoint_ = new Vector3(vertices[start].x, vertices[start].y, vertices[start].z);
                Vector3 endpoint_ = new Vector3(vertices[end].x, vertices[end].y, vertices[end].z);
                Vector3 pos = Vector3.Lerp(beginpoint_, endpoint_, (float)0.5);
                float distance = Vector3.Distance(beginpoint_, endpoint_);
                Quaternion rot = new Quaternion();
                if (distance != 0) {
                    rot = Quaternion.LookRotation(beginpoint_ - endpoint_);
                    //Debug.Log(rot);
                }
                edges[i].transform.localPosition = pos;
                edges[i].transform.LookAt(endpoint_);
                edges[i].transform.rotation = rot;
                edges[i].transform.localScale = new Vector3(0.01f, 0.01f, distance);
            }
        }

    }
    public class FourDshape {
        int size;
        GameObject[] edges;
        public Vector4[] srcVertices;
        public Vector4[] vertices;
        Vector3[] index;
        Transform parentobj;
        GameObject[] spheres;

        void setparent(Transform par) {
            for (int i = 0; i < size; i++) {
                edges[i].transform.parent = par;
            }
        }

        public FourDshape(Transform par) {
            parentobj = par;
            size = 24;
            srcVertices = new Vector4[8];
            vertices = new Vector4[8];
            vertices[0] = new Vector4(-1, 0, 0, 0) * 0.2f;
            vertices[1] = new Vector4(1, 0, 0, 0) * 0.2f;
            vertices[2] = new Vector4(0, -1, 0, 0) * 0.2f;
            vertices[3] = new Vector4(0, 1, 0, 0) * 0.2f;
            vertices[4] = new Vector4(0, 0, -1, 0) * 0.2f;
            vertices[5] = new Vector4(0, 0, 1, 0) * 0.2f;
            vertices[6] = new Vector4(0, 0, 0, -1) * 0.2f;
            vertices[7] = new Vector4(0, 0, 0, 1) * 0.2f;
            vertices[0] = new Vector4(-1, 0, 0, 0) * 0.2f;
            vertices[1] = new Vector4(1, 0, 0, 0) * 0.2f;
            vertices[2] = new Vector4(0, -1, 0, 0) * 0.2f;
            vertices[3] = new Vector4(0, 1, 0, 0) * 0.2f;
            vertices[4] = new Vector4(0, 0, -1, 0) * 0.2f;
            vertices[5] = new Vector4(0, 0, 1, 0) * 0.2f;
            vertices[6] = new Vector4(0, 0, 0, -1) * 0.2f;
            vertices[7] = new Vector4(0, 0, 0, 1) * 0.2f;
            srcVertices[0] = new Vector4(-1, 0, 0, 0) * 0.2f;
            srcVertices[1] = new Vector4(1, 0, 0, 0) * 0.2f;
            srcVertices[2] = new Vector4(0, -1, 0, 0) * 0.2f;
            srcVertices[3] = new Vector4(0, 1, 0, 0) * 0.2f;
            srcVertices[4] = new Vector4(0, 0, -1, 0) * 0.2f;
            srcVertices[5] = new Vector4(0, 0, 1, 0) * 0.2f;
            srcVertices[6] = new Vector4(0, 0, 0, -1) * 0.2f;
            srcVertices[7] = new Vector4(0, 0, 0, 1) * 0.2f;
            srcVertices[0] = new Vector4(-1, 0, 0, 0) * 0.2f;
            srcVertices[1] = new Vector4(1, 0, 0, 0) * 0.2f;
            srcVertices[2] = new Vector4(0, -1, 0, 0) * 0.2f;
            srcVertices[3] = new Vector4(0, 1, 0, 0) * 0.2f;
            srcVertices[4] = new Vector4(0, 0, -1, 0) * 0.2f;
            srcVertices[5] = new Vector4(0, 0, 1, 0) * 0.2f;
            srcVertices[6] = new Vector4(0, 0, 0, -1) * 0.2f;
            srcVertices[7] = new Vector4(0, 0, 0, 1) * 0.2f;
            index = new Vector3[24];
            index[0] = new Vector3(0, 2, 1);
            index[1] = new Vector3(1, 2, 1);
            index[2] = new Vector3(0, 3, 1);
            index[3] = new Vector3(1, 3, 1);
            index[4] = new Vector3(0, 4, 3);
            index[5] = new Vector3(1, 4, 3);
            index[6] = new Vector3(0, 5, 3);
            index[7] = new Vector3(1, 5, 3);
            index[8] = new Vector3(0, 6, 5);
            index[9] = new Vector3(1, 6, 5);
            index[10] = new Vector3(0, 7, 5);
            index[11] = new Vector3(1, 7, 5);
            index[12] = new Vector3(2, 4, 2);
            index[13] = new Vector3(3, 4, 2);
            index[14] = new Vector3(2, 5, 2);
            index[15] = new Vector3(3, 5, 2);
            index[16] = new Vector3(2, 6, 4);
            index[17] = new Vector3(3, 6, 4);
            index[18] = new Vector3(2, 7, 4);
            index[19] = new Vector3(3, 7, 4);
            index[20] = new Vector3(4, 6, 6);
            index[21] = new Vector3(5, 6, 6);
            index[22] = new Vector3(4, 7, 6);
            index[23] = new Vector3(5, 7, 6);

            edges = new GameObject[24];
            spheres = new GameObject[8];

            for (int i = 0; i < 8; i++) {
                Vector3 pos = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);
                GameObject verObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                verObj.transform.position = pos;
                verObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                spheres[i] = verObj;
                spheres[i].transform.parent = par;

            }

            for (int i = 0; i < size; i++) {
                int start, end, color;
                start = (int)index[i].x;
                end = (int)index[i].y;
                color = (int)index[i].z;
                Vector3 beginpoint_ = new Vector3(vertices[start].x, vertices[start].y, vertices[start].z);
                Vector3 endpoint_ = new Vector3(vertices[end].x, vertices[end].y, vertices[end].z);
                Vector3 pos = Vector3.Lerp(beginpoint_, endpoint_, (float)0.5);
                float distance = Vector3.Distance(beginpoint_, endpoint_);
                GameObject segObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                segObj.transform.position = pos;
                segObj.transform.LookAt(endpoint_);
                segObj.transform.Rotate(new Vector3(1.0f, 0, 0), 90);
                segObj.transform.localScale = new Vector3(0.01f, distance, 0.01f);
                edges[i] = segObj;
                coloredge(i, color);
            }

            // This is for testing the algorithm



            setparent(par);
        }

        public void coloredge(int i, int index) {
            if (index == 1) {
                edges[i].GetComponent<Renderer>().material.color = Color.green;
            }
            if (index == 2) {
                edges[i].GetComponent<Renderer>().material.color = Color.red;
            }
            if (index == 3) {
                edges[i].GetComponent<Renderer>().material.color = Color.blue;
            }
            if (index == 4) {
                edges[i].GetComponent<Renderer>().material.color = Color.yellow;
            }
            if (index == 5) {
                edges[i].GetComponent<Renderer>().material.color = Color.black;
            }
            if (index == 6) {
                edges[i].GetComponent<Renderer>().material.color = Color.grey;
            }
        }


        public void returnpoint4(float[] src, int i) {
            src[0] = (float)vertices[i].x;
            src[1] = (float)vertices[i].y;
            src[2] = (float)vertices[i].z;
            src[3] = (float)vertices[i].w;
        }

        public void updatepoint4(float[] src, int i) {
            vertices[i].x = (float)src[0];
            vertices[i].y = (float)src[1];
            vertices[i].z = (float)src[2];
            vertices[i].w = (float)src[3];
        }

        public void update_edges() {

            for (int i = 0; i < 8; i++) {
                Vector3 pos = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);
                spheres[i].transform.localPosition = pos;
                spheres[i].transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            }


            for (int i = 0; i < 24; i++) {
                int start, end;
                start = (int)index[i].x;
                end = (int)index[i].y;
                Vector3 beginpoint_ = new Vector3(vertices[start].x, vertices[start].y, vertices[start].z);
                Vector3 endpoint_ = new Vector3(vertices[end].x, vertices[end].y, vertices[end].z);
                Vector3 pos = Vector3.Lerp(beginpoint_, endpoint_, (float)0.5);
                float distance = Vector3.Distance(beginpoint_, endpoint_);
                Quaternion rot = Quaternion.LookRotation(beginpoint_ - endpoint_);
                edges[i].transform.localPosition = pos;
                edges[i].transform.LookAt(endpoint_);
                edges[i].transform.rotation = rot;
                edges[i].transform.localScale = new Vector3(0.01f, 0.01f, distance);
            }
        }

    }
    public class Cell {
        int size;
        GameObject[] edges;
        public Vector4[] srcVertices;
        public Vector4[] vertices;
        Vector4[] index;
        Transform parentobj;
        GameObject[] spheres;

        void setparent(Transform par) {
            for (int i = 0; i < size; i++) {
                edges[i].transform.parent = par;
            }
        }

        public Cell(Transform par) {
            parentobj = par;
            size = 96;
            srcVertices = new Vector4[24];
            vertices = new Vector4[24];
            srcVertices[0] = new Vector4(-1, -1, 0, 0) * 0.2f;
            srcVertices[1] = new Vector4(1, -1, 0, 0) * 0.2f;
            srcVertices[2] = new Vector4(-1, 1, 0, 0) * 0.2f;
            srcVertices[3] = new Vector4(1, 1, 0, 0) * 0.2f;
            srcVertices[4] = new Vector4(-1, 0, -1, 0) * 0.2f;
            srcVertices[5] = new Vector4(1, 0, -1, 0) * 0.2f;
            srcVertices[6] = new Vector4(-1, 0, 1, 0) * 0.2f;
            srcVertices[7] = new Vector4(1, 0, 1, 0) * 0.2f;
            srcVertices[8] = new Vector4(-1, 0, 0, -1) * 0.2f;
            srcVertices[9] = new Vector4(1, 0, 0, -1) * 0.2f;
            srcVertices[10] = new Vector4(-1, 0, 0, 1) * 0.2f;
            srcVertices[11] = new Vector4(1, 0, 0, 1) * 0.2f;
            srcVertices[12] = new Vector4(0, -1, -1, 0) * 0.2f;
            srcVertices[13] = new Vector4(0, 1, -1, 0) * 0.2f;
            srcVertices[14] = new Vector4(0, -1, 1, 0) * 0.2f;
            srcVertices[15] = new Vector4(0, 1, 1, 0) * 0.2f;
            srcVertices[16] = new Vector4(0, -1, 0, -1) * 0.2f;
            srcVertices[17] = new Vector4(0, 1, 0, -1) * 0.2f;
            srcVertices[18] = new Vector4(0, -1, 0, 1) * 0.2f;
            srcVertices[19] = new Vector4(0, 1, 0, 1) * 0.2f;
            srcVertices[20] = new Vector4(0, 0, -1, -1) * 0.2f;
            srcVertices[21] = new Vector4(0, 0, 1, -1) * 0.2f;
            srcVertices[22] = new Vector4(0, 0, -1, 1) * 0.2f;
            srcVertices[23] = new Vector4(0, 0, 1, 1) * 0.2f;
            vertices[0] = new Vector4(-1, -1, 0, 0) * 0.2f;
            vertices[1] = new Vector4(1, -1, 0, 0) * 0.2f;
            vertices[2] = new Vector4(-1, 1, 0, 0) * 0.2f;
            vertices[3] = new Vector4(1, 1, 0, 0) * 0.2f;
            vertices[4] = new Vector4(-1, 0, -1, 0) * 0.2f;
            vertices[5] = new Vector4(1, 0, -1, 0) * 0.2f;
            vertices[6] = new Vector4(-1, 0, 1, 0) * 0.2f;
            vertices[7] = new Vector4(1, 0, 1, 0) * 0.2f;
            vertices[8] = new Vector4(-1, 0, 0, -1) * 0.2f;
            vertices[9] = new Vector4(1, 0, 0, -1) * 0.2f;
            vertices[10] = new Vector4(-1, 0, 0, 1) * 0.2f;
            vertices[11] = new Vector4(1, 0, 0, 1) * 0.2f;
            vertices[12] = new Vector4(0, -1, -1, 0) * 0.2f;
            vertices[13] = new Vector4(0, 1, -1, 0) * 0.2f;
            vertices[14] = new Vector4(0, -1, 1, 0) * 0.2f;
            vertices[15] = new Vector4(0, 1, 1, 0) * 0.2f;
            vertices[16] = new Vector4(0, -1, 0, -1) * 0.2f;
            vertices[17] = new Vector4(0, 1, 0, -1) * 0.2f;
            vertices[18] = new Vector4(0, -1, 0, 1) * 0.2f;
            vertices[19] = new Vector4(0, 1, 0, 1) * 0.2f;
            vertices[20] = new Vector4(0, 0, -1, -1) * 0.2f;
            vertices[21] = new Vector4(0, 0, 1, -1) * 0.2f;
            vertices[22] = new Vector4(0, 0, -1, 1) * 0.2f;
            vertices[23] = new Vector4(0, 0, 1, 1) * 0.2f;
            index = new Vector4[96];
            index[0] = new Vector4(0, 4, 1, 4);
            index[1] = new Vector4(0, 6, 1, 4);
            index[2] = new Vector4(0, 8, 1, 3);
            index[3] = new Vector4(0, 10, 1, 3);
            index[4] = new Vector4(0, 12, 2, 4);
            index[5] = new Vector4(0, 14, 2, 4);
            index[6] = new Vector4(0, 16, 2, 3);
            index[7] = new Vector4(0, 18, 2, 3);
            index[8] = new Vector4(1, 5, 1, 4);
            index[9] = new Vector4(1, 7, 1, 4);
            index[10] = new Vector4(1, 9, 1, 3);
            index[11] = new Vector4(1, 11, 1, 3);
            index[12] = new Vector4(1, 12, 2, 4);
            index[13] = new Vector4(1, 14, 2, 4);
            index[14] = new Vector4(1, 16, 2, 3);
            index[15] = new Vector4(1, 18, 2, 3);
            index[16] = new Vector4(2, 4, 1, 4);
            index[17] = new Vector4(2, 6, 1, 4);
            index[18] = new Vector4(2, 8, 1, 3);
            index[19] = new Vector4(2, 10, 1, 3);
            index[20] = new Vector4(2, 13, 2, 4);
            index[21] = new Vector4(2, 15, 2, 4);
            index[22] = new Vector4(2, 17, 2, 3);
            index[23] = new Vector4(2, 19, 2, 3);
            index[24] = new Vector4(3, 5, 1, 4);
            index[25] = new Vector4(3, 7, 1, 4);
            index[26] = new Vector4(3, 9, 1, 3);
            index[27] = new Vector4(3, 11, 1, 3);
            index[28] = new Vector4(3, 13, 2, 4);
            index[29] = new Vector4(3, 15, 2, 4);
            index[30] = new Vector4(3, 17, 2, 3);
            index[31] = new Vector4(3, 19, 2, 3);
            index[32] = new Vector4(4, 8, 1, 2);
            index[33] = new Vector4(4, 10, 1, 2);
            index[34] = new Vector4(4, 12, 3, 4);
            index[35] = new Vector4(4, 13, 3, 4);
            index[36] = new Vector4(4, 20, 3, 2);
            index[37] = new Vector4(4, 22, 3, 2);
            index[38] = new Vector4(5, 9, 1, 2);
            index[39] = new Vector4(5, 11, 1, 2);
            index[40] = new Vector4(5, 12, 3, 4);
            index[41] = new Vector4(5, 13, 3, 4);
            index[42] = new Vector4(5, 20, 3, 2);
            index[43] = new Vector4(5, 22, 3, 2);
            index[44] = new Vector4(6, 8, 1, 2);
            index[45] = new Vector4(6, 10, 1, 2);
            index[46] = new Vector4(6, 14, 3, 4);
            index[47] = new Vector4(6, 15, 3, 4);
            index[48] = new Vector4(6, 21, 3, 2);
            index[49] = new Vector4(6, 23, 3, 2);
            index[50] = new Vector4(7, 9, 1, 2);
            index[51] = new Vector4(7, 11, 1, 2);
            index[52] = new Vector4(7, 14, 3, 4);
            index[53] = new Vector4(7, 15, 3, 4);
            index[54] = new Vector4(7, 21, 3, 2);
            index[55] = new Vector4(7, 23, 3, 2);
            index[56] = new Vector4(8, 16, 4, 3);
            index[57] = new Vector4(8, 17, 4, 3);
            index[58] = new Vector4(8, 20, 4, 2);
            index[59] = new Vector4(8, 21, 4, 2);
            index[60] = new Vector4(9, 16, 4, 3);
            index[61] = new Vector4(9, 17, 4, 3);
            index[62] = new Vector4(9, 20, 4, 2);
            index[63] = new Vector4(9, 21, 4, 2);
            index[64] = new Vector4(10, 18, 4, 3);
            index[65] = new Vector4(10, 19, 4, 3);
            index[66] = new Vector4(10, 22, 4, 2);
            index[67] = new Vector4(10, 23, 4, 2);
            index[68] = new Vector4(11, 18, 4, 3);
            index[69] = new Vector4(11, 19, 4, 3);
            index[70] = new Vector4(11, 22, 4, 2);
            index[71] = new Vector4(11, 23, 4, 2);
            index[72] = new Vector4(12, 16, 2, 1);
            index[73] = new Vector4(12, 18, 2, 1);
            index[74] = new Vector4(12, 20, 3, 1);
            index[75] = new Vector4(12, 22, 3, 1);
            index[76] = new Vector4(13, 17, 2, 1);
            index[77] = new Vector4(13, 19, 2, 1);
            index[78] = new Vector4(13, 20, 3, 1);
            index[79] = new Vector4(13, 22, 3, 1);
            index[80] = new Vector4(14, 16, 2, 1);
            index[81] = new Vector4(14, 18, 2, 1);
            index[82] = new Vector4(14, 21, 3, 1);
            index[83] = new Vector4(14, 23, 3, 1);
            index[84] = new Vector4(15, 17, 2, 1);
            index[85] = new Vector4(15, 19, 2, 1);
            index[86] = new Vector4(15, 21, 3, 1);
            index[87] = new Vector4(15, 23, 3, 1);
            index[88] = new Vector4(16, 20, 4, 1);
            index[89] = new Vector4(16, 21, 4, 1);
            index[90] = new Vector4(17, 20, 4, 1);
            index[91] = new Vector4(17, 21, 4, 1);
            index[92] = new Vector4(18, 22, 4, 1);
            index[93] = new Vector4(18, 23, 4, 1);
            index[94] = new Vector4(19, 22, 4, 1);
            index[95] = new Vector4(19, 23, 4, 1);


            edges = new GameObject[96];
            spheres = new GameObject[24];

            for (int i = 0; i < 24; i++) {
                Vector3 pos = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);
                GameObject verObj = GameObject.CreatePrimitive(PrimitiveType.Sphere);
                verObj.transform.position = pos;
                verObj.transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);
                spheres[i] = verObj;
                spheres[i].transform.parent = par;

            }

            for (int i = 0; i < size; i++) {
                int start, end, color;
                start = (int)index[i].x;
                end = (int)index[i].y;
                color = (int)index[i].z;
                Vector3 beginpoint_ = new Vector3(vertices[start].x, vertices[start].y, vertices[start].z);
                Vector3 endpoint_ = new Vector3(vertices[end].x, vertices[end].y, vertices[end].z);
                Vector3 pos = Vector3.Lerp(beginpoint_, endpoint_, (float)0.5);
                float distance = Vector3.Distance(beginpoint_, endpoint_);
                GameObject segObj = GameObject.CreatePrimitive(PrimitiveType.Cube);
                segObj.transform.position = pos;
                segObj.transform.LookAt(endpoint_);
                segObj.transform.Rotate(new Vector3(1.0f, 0, 0), 90);
                segObj.transform.localScale = new Vector3(0.01f, distance, 0.01f);
                edges[i] = segObj;
                coloredge(i, color);
            }

            // This is for testing the algorithm



            setparent(par);
        }

        public void coloredge(int i, int index) {
            if (index == 1) {
                edges[i].GetComponent<Renderer>().material.color = Color.green;
            }
            if (index == 2) {
                edges[i].GetComponent<Renderer>().material.color = Color.red;
            }
            if (index == 3) {
                edges[i].GetComponent<Renderer>().material.color = Color.blue;
            }
            if (index == 4) {
                edges[i].GetComponent<Renderer>().material.color = Color.yellow;
            }

        }


        public void returnpoint4(float[] src, int i) {
            src[0] = (float)vertices[i].x;
            src[1] = (float)vertices[i].y;
            src[2] = (float)vertices[i].z;
            src[3] = (float)vertices[i].w;
        }

        public void updatepoint4(float[] src, int i) {
            vertices[i].x = (float)src[0];
            vertices[i].y = (float)src[1];
            vertices[i].z = (float)src[2];
            vertices[i].w = (float)src[3];
        }

        public void update_edges() {

            for (int i = 0; i < 24; i++) {
                Vector3 pos = new Vector3(vertices[i].x, vertices[i].y, vertices[i].z);
                spheres[i].transform.localPosition = pos;
                spheres[i].transform.localScale = new Vector3(0.01f, 0.01f, 0.01f);

            }


            for (int i = 0; i < 96; i++) {
                int start, end;
                start = (int)index[i].x;
                end = (int)index[i].y;
                Vector3 beginpoint_ = new Vector3(vertices[start].x, vertices[start].y, vertices[start].z);
                Vector3 endpoint_ = new Vector3(vertices[end].x, vertices[end].y, vertices[end].z);
                Vector3 pos = Vector3.Lerp(beginpoint_, endpoint_, (float)0.5);
                float distance = Vector3.Distance(beginpoint_, endpoint_);
                Quaternion rot = Quaternion.LookRotation(beginpoint_ - endpoint_);
                edges[i].transform.localPosition = pos;
                edges[i].transform.LookAt(endpoint_);
                edges[i].transform.rotation = rot;
                edges[i].transform.localScale = new Vector3(0.01f, 0.01f, distance);
            }
        }

    }
    public class Header : MonoBehaviour {

        // Use this for initialization
        void Start() {

        }

        // Update is called once per frame
        void Update() {

        }
    }
}
