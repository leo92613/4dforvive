using UnityEngine;
using System.Collections;

namespace Holojam {
    public class WiiMoteGlobalTestObject : WiiGlobalReceiver, IGlobalWiiMoteHandler {

        /// <summary>
        /// Test object for global inputs from the wii mote. If the module is populated, then the object will only recieve globals from that object. Otherwise, it will recieve globals from all wiimotes.
        /// </summary>
        /// <remarks>
        /// DO:
        ///     * Extend WiiGlobalReceiver instead of MonoBehaviour.
        /// DO NOT:
        ///     * Fail to use base.OnEnable() and base.OnDisable() if overriding those functions.
        /// </remarks>

        public void OnGlobalAPressDown(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalAPressDown on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalAPress(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalAPress on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalAPressUp(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalAPressUp on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalBPressDown(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalBPressDown on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalBPress(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalBPress on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalBPressUp(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalBPressUp on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalLeftPressDown(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalLeftPressDown on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalLeftPress(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalLeftPress on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalLeftPressUp(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalLeftPressUp on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalRightPressDown(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalRightPressDown on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalRightPress(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalRightPress on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalRightPressUp(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalRightPressUp on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalUpPressDown(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalUpPressDown on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalUpPress(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalUpPress on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalUpPressUp(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalUpPressUp on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalDownPressDown(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalDownPressDown on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalDownPress(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalDownPress on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalDownPressUp(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalDownPressUp on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalPlusPressDown(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalPlusPressDown on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalPlusPress(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalPlusPress on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalPlusPressUp(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalPlusPressUp on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalMinusPressDown(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalMinusPressDown on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalMinusPress(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalMinusPress on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalMinusPressUp(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalMinusPressUp on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalHomePressDown(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalHomePressDown on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalHomePress(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalHomePress on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalHomePressUp(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalHomePressUp on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalOnePressDown(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalOnePressDown on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalOnePress(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalOnePress on " + this.name + " from module " + eventData.module.name);
        }

        public void OnGlobalOnePressUp(WiiMoteEventData eventData) {
            Debug.Log("OnGlobalOnePressUp on " + this.name + " from module " + eventData.module.name);
        }
    }
}

