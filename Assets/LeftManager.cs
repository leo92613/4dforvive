﻿using UnityEngine;
using System.Collections;
using System;

namespace Holojam.IO {
    public class LeftManager : ViveGlobalReceiver, IGlobalTriggerHandler {
        LineRenderer line;
        public GameObject rightcontroller;
        public Transform trackball;
        Manager right;
        RaycastHit hit;
        GameObject tmp;
        bool chosen;
        public void OnGlobalTriggerPress(ViveEventData eventData) {
            right.root.GetComponent<Renderer>().material = right.mat[0];
            Ray choose = new Ray(eventData.module.transform.position, eventData.module.transform.forward);
            if (Physics.Raycast(choose, out hit)) {
                if (hit.transform.gameObject != tmp) {
                    tmp.GetComponent<Renderer>().material = right.mat[0];
                    tmp = hit.transform.gameObject;
                }
                line.enabled = true;
                line.SetPosition(0, eventData.module.transform.position);
                line.SetPosition(1, hit.point);
                hit.transform.gameObject.GetComponent<Renderer>().material = right.mat[1];

                chosen = true;
            }
            else {
              
                chosen = false;
                line.enabled = true;
                line.SetPosition(0, eventData.module.transform.position);
                line.SetPosition(1, eventData.module.transform.position+1000* eventData.module.transform.forward);
            }
            
            //line.enabled = false;
        }

        public void OnGlobalTriggerPressDown(ViveEventData eventData) {
           // throw new NotImplementedException();
        }

        public void OnGlobalTriggerPressUp(ViveEventData eventData) {

            line.enabled = false;
            if (chosen) {
                hit.transform.gameObject.GetComponent<Renderer>().material = right.mat[0];
                right.root = hit.transform.gameObject;
                right.root.GetComponent<Renderer>().material = right.mat[1];
                right.Sethyperface();
                //trackball.localPosition = right.root.GetComponent<BoxCollider>().center;
                right.ball = trackball;
            } else {
                right.root.GetComponent<Renderer>().material = right.mat[1];
            }
        }

        public void OnGlobalTriggerTouch(ViveEventData eventData) {
            //throw new NotImplementedException();
        }

        public void OnGlobalTriggerTouchDown(ViveEventData eventData) {
           // throw new NotImplementedException();
        }

        public void OnGlobalTriggerTouchUp(ViveEventData eventData) {
           // throw new NotImplementedException();
        }



        // Use this for initialization
        void Start() {
            right = rightcontroller.GetComponent<Manager>();
            line = GetComponent<LineRenderer>();
            line.SetVertexCount(2);
            line.SetWidth(0.001f, 0.005f);
            line.enabled = false;
            tmp = right.root;
        }

        // Update is called once per frame
        void Update() {

        }
    }
}
