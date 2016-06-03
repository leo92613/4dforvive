using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;

namespace Holojam {

    public class WiiMoteEventData : PointerEventData {
        public WiiMoteModule module;
        public GameObject previousRaycast;
        public GameObject currentRaycast;
        public GameObject aPress, bPress, leftPress, rightPress, upPress, downPress, plusPress, minusPress, homePress, onePress;

        public WiiMoteEventData(EventSystem eventSystem)
            : base(eventSystem) {

        }
    }

    public static class ButtonConstants {
        public const int A = 2048;
        public const int B = 1024;
        public const int LEFT = 1;
        public const int RIGHT = 2;
        public const int DOWN = 4;
        public const int UP = 8;
        public const int PLUS = 16;
        public const int MINUS = 4096;
        public const int HOME = 32768;
        public const int ONE = 512;
    }

    public class WiiMoteModule : MonoBehaviour {

        public static List<WiiGlobalReceiver> receivers = new List<WiiGlobalReceiver>();

        [HideInInspector]
        public MasterStream stream;
        public string label;

        public string interactTag;
        public float interactDistance = 10f;

        private Vector3 loPosition = Vector3.zero;
        private Quaternion loRotation = Quaternion.identity;
        private int loButtonBits = 0;
        private int prev_loButtonBits = 0;

        WiiMoteEventData eventData = new WiiMoteEventData(EventSystem.current);

        int[] buttons = new int[] {ButtonConstants.A,ButtonConstants.B,ButtonConstants.LEFT,ButtonConstants.RIGHT,ButtonConstants.UP,ButtonConstants.DOWN,
                                   ButtonConstants.PLUS,ButtonConstants.MINUS,ButtonConstants.HOME,ButtonConstants.ONE };
        Dictionary<int, GameObject> buttonPairings = new Dictionary<int, GameObject>();

        Dictionary<int, List<WiiGlobalReceiver>> buttonReceivers = new Dictionary<int, List<WiiGlobalReceiver>>();

        // Use this for initialization
        void Start() {
            stream = MasterStream.Instance;
            eventData.module = this;


            //initialize button pairings and global objects
            foreach (int button in buttons) {
                buttonPairings.Add(button, null);
                buttonReceivers.Add(button, new List<WiiGlobalReceiver>());
            }
        }

        void Update() {
            this.Process();
        }

        void OnDisable() {
            foreach (int button in buttons) {
                this.ExecuteButtonUp(button);
                this.ExecuteGlobalButtonUp(button);
            }
        }
        void Process() {
            this.UpdateLiveValues();
            this.PositionTransform();
            this.CastRayFromBoundObject();
            this.UpdateCurrentObject();
            //this.PlaceCursor();
            this.HandleButtons();
        }

        void UpdateLiveValues() {
            this.loPosition = stream.getLiveObjectPosition(label);
            this.loRotation = stream.getLiveObjectRotation(label);
            this.prev_loButtonBits = this.loButtonBits;
            this.loButtonBits = stream.getLiveObjectButtonBits(label);
            //Debug.Log(loButtonBits);
        }

        void PositionTransform() {
            transform.localPosition = this.loPosition;
            transform.localRotation = this.loRotation;
        }


        private List<RaycastHit> hits = new List<RaycastHit>();
        private Ray ray;
        void CastRayFromBoundObject() {
            hits.Clear();

            //CAST RAY
            Vector3 v = transform.position;
            Quaternion q = transform.rotation;
            ray = new Ray(v, q * Vector3.forward);
            hits.AddRange(Physics.RaycastAll(ray, interactDistance));
            eventData.previousRaycast = eventData.currentRaycast;

            if (hits.Count == 0) {
                eventData.currentRaycast = null;
                return;
            }

            //FIND THE CLOSEST OBJECT
            RaycastHit minHit = hits[0];
            for (int i = 0; i < hits.Count; i++) {
                if (hits[i].distance < minHit.distance) {
                    minHit = hits[i];
                }
            }

            //MAKE SURE CLOSEST OBJECT IS INTERACTABLE
            if (interactTag != null && interactTag.Length > 1 && !minHit.transform.tag.Equals(interactTag)) {
                eventData.currentRaycast = null;
                return;
            } else {
                eventData.currentRaycast = minHit.transform.gameObject;
            }
        }

        void UpdateCurrentObject() {
            this.HandlePointerExitAndEnter(eventData);
        }

        void HandlePointerExitAndEnter(WiiMoteEventData eventData) {
            if (eventData.previousRaycast != eventData.currentRaycast) {
                ExecuteEvents.Execute<IPointerEnterHandler>(eventData.currentRaycast, eventData, ExecuteEvents.pointerEnterHandler);
                ExecuteEvents.Execute<IPointerExitHandler>(eventData.previousRaycast, eventData, ExecuteEvents.pointerExitHandler);
            }
        }

        void PlaceCursor() {
            //TODO.
        }

        void HandleButtons() {
            foreach (int button in buttons) {
                if (this.GetButtonDown(button)) {
                    this.ExecuteButtonDown(button);
                    this.ExecuteGlobalButtonDown(button);
                } else if (this.GetButton(button)) {
                    this.ExecuteButton(button);
                    this.ExecuteGlobalButton(button);
                } else if (this.GetButtonUp(button)) {
                    this.ExecuteButtonUp(button);
                    this.ExecuteGlobalButtonUp(button);
                }
            }
        }

        void ExecuteButtonDown(int button) {
            GameObject go = eventData.currentRaycast;
            if (go == null)
                return;

            switch (button) {
                case ButtonConstants.A:
                    eventData.aPress = go;
                    ExecuteEvents.Execute<IWiiMoteAPressDownHandler>(go, eventData, (x, y) => x.OnAPressDown(eventData));
                    break;
                case ButtonConstants.B:
                    eventData.bPress = go;
                    ExecuteEvents.Execute<IWiiMoteBPressDownHandler>(go, eventData, (x, y) => x.OnBPressDown(eventData));
                    break;
                case ButtonConstants.LEFT:
                    eventData.leftPress = go;
                    ExecuteEvents.Execute<IWiiMoteLeftPressDownHandler>(go, eventData, (x, y) => x.OnLeftPressDown(eventData));
                    break;
                case ButtonConstants.RIGHT:
                    eventData.rightPress = go;
                    ExecuteEvents.Execute<IWiiMoteRightPressDownHandler>(go, eventData, (x, y) => x.OnRightPressDown(eventData));
                    break;
                case ButtonConstants.UP:
                    eventData.upPress = go;
                    ExecuteEvents.Execute<IWiiMoteUpPressDownHandler>(go, eventData, (x, y) => x.OnUpPressDown(eventData));
                    break;
                case ButtonConstants.DOWN:
                    eventData.downPress = go;
                    ExecuteEvents.Execute<IWiiMoteDownPressDownHandler>(go, eventData, (x, y) => x.OnDownPressDown(eventData));
                    break;
                case ButtonConstants.PLUS:
                    eventData.plusPress = go;
                    ExecuteEvents.Execute<IWiiMotePlusPressDownHandler>(go, eventData, (x, y) => x.OnPlusPressDown(eventData));
                    break;
                case ButtonConstants.MINUS:
                    eventData.minusPress = go;
                    ExecuteEvents.Execute<IWiiMoteMinusPressDownHandler>(go, eventData, (x, y) => x.OnMinusPressDown(eventData));
                    break;
                case ButtonConstants.ONE:
                    eventData.onePress = go;
                    ExecuteEvents.Execute<IWiiMoteOnePressDownHandler>(go, eventData, (x, y) => x.OnOnePressDown(eventData));
                    break;
            }
            //ADD THE PAIRING
            buttonPairings[button] = go;
        }

        void ExecuteButton(int button) {
            if (buttonPairings[button] == null)
                return;

            switch (button) {
                case ButtonConstants.A:
                    ExecuteEvents.Execute<IWiiMoteAPressHandler>(eventData.aPress, eventData, (x, y) => x.OnAPress(eventData));
                    break;
                case ButtonConstants.B:
                    ExecuteEvents.Execute<IWiiMoteBPressHandler>(eventData.bPress, eventData, (x, y) => x.OnBPress(eventData));
                    break;
                case ButtonConstants.LEFT:
                    ExecuteEvents.Execute<IWiiMoteLeftPressHandler>(eventData.leftPress, eventData, (x, y) => x.OnLeftPress(eventData));
                    break;
                case ButtonConstants.RIGHT:
                    ExecuteEvents.Execute<IWiiMoteRightPressHandler>(eventData.rightPress, eventData, (x, y) => x.OnRightPress(eventData));
                    break;
                case ButtonConstants.UP:
                    ExecuteEvents.Execute<IWiiMoteUpPressHandler>(eventData.upPress, eventData, (x, y) => x.OnUpPress(eventData));
                    break;
                case ButtonConstants.DOWN:
                    ExecuteEvents.Execute<IWiiMoteDownPressHandler>(eventData.downPress, eventData, (x, y) => x.OnDownPress(eventData));
                    break;
                case ButtonConstants.PLUS:
                    ExecuteEvents.Execute<IWiiMotePlusPressHandler>(eventData.plusPress, eventData, (x, y) => x.OnPlusPress(eventData));
                    break;
                case ButtonConstants.MINUS:
                    ExecuteEvents.Execute<IWiiMoteMinusPressHandler>(eventData.minusPress, eventData, (x, y) => x.OnMinusPress(eventData));
                    break;
                case ButtonConstants.ONE:
                    ExecuteEvents.Execute<IWiiMoteOnePressHandler>(eventData.onePress, eventData, (x, y) => x.OnOnePress(eventData));
                    break;
            }
        }

        void ExecuteButtonUp(int button) {
            if (buttonPairings[button] == null)
                return;

            switch (button) {
                case ButtonConstants.A:
                    ExecuteEvents.Execute<IWiiMoteAPressUpHandler>(eventData.aPress, eventData, (x, y) => x.OnAPressUp(eventData));
                    eventData.aPress = null;
                    break;
                case ButtonConstants.B:
                    ExecuteEvents.Execute<IWiiMoteBPressUpHandler>(eventData.bPress, eventData, (x, y) => x.OnBPressUp(eventData));
                    eventData.bPress = null;
                    break;
                case ButtonConstants.LEFT:
                    ExecuteEvents.Execute<IWiiMoteLeftPressUpHandler>(eventData.leftPress, eventData, (x, y) => x.OnLeftPressUp(eventData));
                    eventData.leftPress = null;
                    break;
                case ButtonConstants.RIGHT:
                    ExecuteEvents.Execute<IWiiMoteRightPressUpHandler>(eventData.rightPress, eventData, (x, y) => x.OnRightPressUp(eventData));
                    eventData.rightPress = null;
                    break;
                case ButtonConstants.UP:
                    ExecuteEvents.Execute<IWiiMoteUpPressUpHandler>(eventData.upPress, eventData, (x, y) => x.OnUpPressUp(eventData));
                    eventData.upPress = null;
                    break;
                case ButtonConstants.DOWN:
                    ExecuteEvents.Execute<IWiiMoteDownPressUpHandler>(eventData.downPress, eventData, (x, y) => x.OnDownPressUp(eventData));
                    eventData.downPress = null;
                    break;
                case ButtonConstants.PLUS:
                    ExecuteEvents.Execute<IWiiMotePlusPressUpHandler>(eventData.plusPress, eventData, (x, y) => x.OnPlusPressUp(eventData));
                    eventData.plusPress = null;
                    break;
                case ButtonConstants.MINUS:
                    ExecuteEvents.Execute<IWiiMoteMinusPressUpHandler>(eventData.minusPress, eventData, (x, y) => x.OnMinusPressUp(eventData));
                    eventData.minusPress = null;
                    break;
                case ButtonConstants.ONE:
                    ExecuteEvents.Execute<IWiiMoteOnePressUpHandler>(eventData.onePress, eventData, (x, y) => x.OnOnePressUp(eventData));
                    eventData.onePress = null;
                    break;
            }
            //REMOVE THE PAIRING
            buttonPairings[button] = null;
        }

        void ExecuteGlobalButtonDown(int button) {
            buttonReceivers[button] = receivers.GetRange(0, receivers.Count);

            if (buttonReceivers[button].Count == 0)
                return;

            switch (button) {
                case ButtonConstants.A:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteAPressDownHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalAPressDown(eventData));
                    break;
                case ButtonConstants.B:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteBPressDownHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalBPressDown(eventData));
                    break;
                case ButtonConstants.LEFT:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteLeftPressDownHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalLeftPressDown(eventData));
                    break;
                case ButtonConstants.RIGHT:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteRightPressDownHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalRightPressDown(eventData));
                    break;
                case ButtonConstants.UP:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteUpPressDownHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalUpPressDown(eventData));
                    break;
                case ButtonConstants.DOWN:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteDownPressDownHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalDownPressDown(eventData));
                    break;
                case ButtonConstants.PLUS:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMotePlusPressDownHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalPlusPressDown(eventData));
                    break;
                case ButtonConstants.MINUS:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteMinusPressDownHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalMinusPressDown(eventData));
                    break;
                case ButtonConstants.ONE:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteOnePressDownHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalOnePressDown(eventData));
                    break;
            }
        }

        void ExecuteGlobalButton(int button) {

            if (buttonReceivers[button].Count == 0)
                return;

            switch (button) {
                case ButtonConstants.A:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteAPressHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalAPress(eventData));
                    break;
                case ButtonConstants.B:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteBPressHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalBPress(eventData));
                    break;
                case ButtonConstants.LEFT:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteLeftPressHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalLeftPress(eventData));
                    break;
                case ButtonConstants.RIGHT:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteRightPressHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalRightPress(eventData));
                    break;
                case ButtonConstants.UP:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteUpPressHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalUpPress(eventData));
                    break;
                case ButtonConstants.DOWN:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteDownPressHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalDownPress(eventData));
                    break;
                case ButtonConstants.PLUS:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMotePlusPressHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalPlusPress(eventData));
                    break;
                case ButtonConstants.MINUS:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteMinusPressHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalMinusPress(eventData));
                    break;
                case ButtonConstants.ONE:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteOnePressHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalOnePress(eventData));
                    break;
            }
        }

        void ExecuteGlobalButtonUp(int button) {
            if (buttonReceivers[button].Count == 0)
                return;

            switch (button) {
                case ButtonConstants.A:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteAPressUpHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalAPressUp(eventData));
                    break;
                case ButtonConstants.B:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteBPressUpHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalBPressUp(eventData));
                    break;
                case ButtonConstants.LEFT:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteLeftPressUpHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalLeftPressUp(eventData));
                    break;
                case ButtonConstants.RIGHT:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteRightPressUpHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalRightPressUp(eventData));
                    break;
                case ButtonConstants.UP:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteUpPressUpHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalUpPressUp(eventData));
                    break;
                case ButtonConstants.DOWN:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteDownPressUpHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalDownPressUp(eventData));
                    break;
                case ButtonConstants.PLUS:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMotePlusPressUpHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalPlusPressUp(eventData));
                    break;
                case ButtonConstants.MINUS:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteMinusPressUpHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalMinusPressUp(eventData));
                    break;
                case ButtonConstants.ONE:
                    foreach (WiiGlobalReceiver w in buttonReceivers[button])
                        if (!w.module || w.module.Equals(this))
                            ExecuteEvents.Execute<IGlobalWiiMoteOnePressUpHandler>(w.gameObject, eventData, (x, y) => x.OnGlobalOnePressUp(eventData));
                    break;
            }

            buttonReceivers[button].Clear();
        }

        bool GetButtonDown(int button) {
            if ((this.prev_loButtonBits & button) == 0 && (this.loButtonBits & button) > 0) {
                return true;
            } else {
                return false;
            }
        }

        bool GetButton(int button) {
            if ((this.prev_loButtonBits & button) > 0 && (this.loButtonBits & button) > 0) {
                return true;
            } else {
                return false;
            }
        }

        bool GetButtonUp(int button) {
            if ((this.prev_loButtonBits & button) > 0 && (this.loButtonBits & button) == 0) {
                return true;
            } else {
                return false;
            }
        }

        void OnDrawGizmos() {
            Gizmos.color = Color.cyan;

            Gizmos.DrawLine(this.transform.position, this.transform.position + this.transform.forward * interactDistance);
        }
    }
}

