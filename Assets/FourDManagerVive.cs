using UnityEngine;
using System.Collections;
using System;

namespace Holojam.IO {
    public class FourDManagerVive : ViveGlobalReceiver, IGlobalTouchpadPressDownHandler {
        public GameObject[] FourDObjectcs;
        int toggle;
        public GameObject Trackball;
        Transform pre_tran;

        public void updateobject(int toggle) {
            for (int i = 0; i < FourDObjectcs.Length; i++) {
                if (i == toggle) {
                    FourDObjectcs[i].SetActive(true);
                    FourDObjectcs[i].GetComponent<Transform>().position = Trackball.GetComponent<Transform>().position;
                  //Trackball.GetComponent<Transform>().position = FourDObjectcs[i].GetComponent<Transform>().position;
                    Trackball.GetComponent<Transform>().parent = FourDObjectcs[i].GetComponent<Transform>();
                } else {
                    FourDObjectcs[i].SetActive(false);
                }
            }
        }

        // Use this for initialization
        void Start() {
            toggle = 0;
           // pre_tran = FourDObjectcs[0].GetComponent<Transform>();
            updateobject(0);
        }

        public void OnGlobalTouchpadPressDown(ViveEventData eventData) {
           // pre_tran = FourDObjectcs[toggle].GetComponent<Transform>();
            toggle = (toggle + 1) % FourDObjectcs.Length;
            updateobject(toggle);
            Debug.Log("toggle is " + toggle);
        }

    }

}
