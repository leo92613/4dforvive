using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;
using System.Collections.Generic;
using System;

namespace Holojam {

	public class HandEventData : PointerEventData {
		public HandModule module;
		public GameObject currentRaycast, previousRaycast;
		public Transform thumb, index, middle, ring, pinky;
		public GameObject one, two, three, four, open, closed, rockin;
		public HandEventData(EventSystem eventSystem) : base(eventSystem) { }
	}

	public enum HandStatus {
		Null, One, Two, Three, Four, Open, Closed, Rockin
	}

	public class HandModule : MonoBehaviour {

		/////Public/////
		//Static
		public static List<HandGlobalReceiver> receivers = new List<HandGlobalReceiver>();
		//References
		//Primitives
		public string hand_label;
		public string root_label;
		public string thumb_label;
		public string index_label;
		public string middle_label;
		public string ring_label;
		public string pinky_label;

		public string interactTag;
		public float interactDistance = 10f;

		/////Protected/////
		//Static
		protected static float OPEN_ANGLE = 60f;
		//References
		protected MasterStream stream;
        protected HandEventData eventData;
		protected Dictionary<HandStatus, GameObject> statusPairings = new Dictionary<HandStatus, GameObject>();
		//Primitives
		protected HandStatus previousHandStatus = HandStatus.Null;
		protected HandStatus currentHandStatus = HandStatus.Null;
		protected Vector3 loPosition = Vector3.zero;
		protected Quaternion loRotation = Quaternion.identity;
		protected Transform hand, thumb, index, middle, ring, pinky;

		/////Private/////
		//Static
		//References
		private RaycastHit hit;
		private Ray ray;
		//Primitives

		///////////////////////////////////////////////////////////////////////////
		//
		// Inherited from MonoBehaviour
		//

		void Awake() {
            eventData = new HandEventData(EventSystem.current);
			//make finger objects
            ObjectController o = new GameObject().AddComponent<ObjectController>();
            o.label = hand_label;
            hand = o.transform;
			thumb = MakeFinger(thumb_label, "_thumb");
			index = MakeFinger(index_label, "_index");
			middle = MakeFinger(middle_label, "_middle");
			ring = MakeFinger(ring_label, "_ring");
			pinky = MakeFinger(pinky_label, "_pinky");

			eventData.module = this;
			eventData.thumb = thumb;
			eventData.index = index;
			eventData.middle = middle;
			eventData.ring = ring;
			eventData.pinky = pinky;

			foreach(HandStatus status in Enum.GetValues(typeof(HandStatus))) {
				statusPairings.Add(status, null);
			}
		}

		void OnDisable() {
			foreach (HandStatus status in Enum.GetValues(typeof(HandStatus))) {
				this.ExecutePoseUp(status);
				this.ExecuteGlobalPoseUp(status);
			}

			eventData.currentRaycast = null;
			this.UpdateCurrentObject();
		}

		void Start() {
            stream = MasterStream.Instance;
		}

		void Update() {
			this.Process();
		}


		///////////////////////////////////////////////////////////////////////////
		//
		// HandModule Functions
		//

		public HandStatus GetCurrentHandStatus() {
			return this.currentHandStatus;
		}

		protected void Process() {
			this.UpdateLiveValues();
			this.PositionTransform();
			this.UpdateHandStatus();
			this.CastRayFromHand();
			this.UpdateCurrentObject();
			//this.PlaceCursor();
			this.HandleHandStatus();
		}

		protected void UpdateLiveValues() {
			this.loPosition = stream.getLiveObjectPosition(hand_label);
			this.loRotation = stream.getLiveObjectRotation(hand_label);
		}

		protected void PositionTransform() {
			hand.localPosition = this.loPosition;
			hand.localRotation = this.loRotation;
		}

		protected void UpdateHandStatus() {
			this.previousHandStatus = this.currentHandStatus;
			this.currentHandStatus = HandStatus.Null;

			bool openThumb = IsFingerOpen(thumb, 3f);
			bool openIndex = IsFingerOpen(index);
			bool openMiddle = IsFingerOpen(middle);
			bool openRing = IsFingerOpen(ring);
			bool openPinky = IsFingerOpen(pinky);

			if (openThumb && openIndex && openMiddle && openRing && openPinky) {
				this.currentHandStatus = HandStatus.Open;
			} else if (openIndex) {
				if (!openMiddle && !openRing && !openPinky) {
					this.currentHandStatus = HandStatus.One;
				} if (openMiddle && !openRing && !openPinky) {
					this.currentHandStatus = HandStatus.Two;
				} else if (openMiddle && ((openRing && !openPinky) || (!openRing && openPinky))) {
					this.currentHandStatus = HandStatus.Three;
				} else if (openMiddle && openRing && openPinky) {
					this.currentHandStatus = HandStatus.Four;
				} else if (!openMiddle && !openRing && openPinky) {
					this.currentHandStatus = HandStatus.Rockin;
				}
			}
		}

		protected void CastRayFromHand() {
			eventData.previousRaycast = eventData.currentRaycast;

			//cast ray
			Vector3 v = transform.position;
			Quaternion q = transform.rotation;
			ray = new Ray(v, q * Vector3.forward);
			if (Physics.Raycast(ray, out hit, interactDistance)) {
				if (interactTag != null && interactTag.Length > 0 && !hit.transform.tag.Equals(interactTag)) {
					eventData.currentRaycast = null;
					return;
				} else {
					eventData.currentRaycast = hit.transform.gameObject;
				}
			} else {
				eventData.currentRaycast = null;
			}
		}

		protected void UpdateCurrentObject() {
			this.HandlePointerExitAndEnter(eventData);
		}

		protected void HandlePointerExitAndEnter(HandEventData eventData) {
			if (eventData.previousRaycast != eventData.currentRaycast) {
				ExecuteEvents.Execute<IPointerEnterHandler>(eventData.currentRaycast, eventData, ExecuteEvents.pointerEnterHandler);
				ExecuteEvents.Execute<IPointerExitHandler>(eventData.previousRaycast, eventData, ExecuteEvents.pointerExitHandler);
			}
		}

		protected void PlaceCursor() {
			//To-Do.
		}

		protected void HandleHandStatus() {
			foreach (HandStatus status in Enum.GetValues(typeof(HandStatus))) {
				if (this.GetPoseDown(status)) {
					ExecutePoseDown(status);
					ExecuteGlobalPoseDown(status);
				} else if (this.GetPose(status)) {
					ExecutePose(status);
					ExecuteGlobalPose(status);
				} else if (this.GetPoseUp(status)) {
					ExecutePoseUp(status);
					ExecuteGlobalPoseUp(status);
				}
			}
		}

		///////////////////////////////////////////////////////////////////////////
		//
		// Execute Functions
		//

		protected void ExecutePoseDown(HandStatus status) {
			GameObject go = eventData.currentRaycast;
			if (go == null)
				return;

			switch (status) {
				case HandStatus.One:
					eventData.one = go;
					ExecuteEvents.Execute<IHandOneDownHandler>(go, eventData, (x, y) => x.OnHandOneDown(eventData));
					break;
				case HandStatus.Two:
					eventData.two = go;
					ExecuteEvents.Execute<IHandTwoDownHandler>(go, eventData, (x, y) => x.OnHandTwoDown(eventData));
					break;
				case HandStatus.Three:
					eventData.three = go;
					ExecuteEvents.Execute<IHandThreeDownHandler>(go, eventData, (x, y) => x.OnHandThreeDown(eventData));
					break;
				case HandStatus.Four:
					eventData.four = go;
					ExecuteEvents.Execute<IHandFourDownHandler>(go, eventData, (x, y) => x.OnHandFourDown(eventData));
					break;
				case HandStatus.Open:
					eventData.open = go;
					ExecuteEvents.Execute<IHandOpenDownHandler>(go, eventData, (x, y) => x.OnHandOpenDown(eventData));
					break;
				case HandStatus.Closed:
					eventData.closed = go;
					ExecuteEvents.Execute<IHandClosedDownHandler>(go, eventData, (x, y) => x.OnHandClosedDown(eventData));
					break;
				case HandStatus.Rockin:
					eventData.rockin = go;
					ExecuteEvents.Execute<IHandRockinDownHandler>(go, eventData, (x, y) => x.OnHandRockinDown(eventData));
					break;
			}

			statusPairings[status] = go;
		}

		protected void ExecutePose(HandStatus status) {
			if (statusPairings[status] == null)
				return;

			switch (status) {
				case HandStatus.One:
					ExecuteEvents.Execute<IHandOneHandler>(eventData.one, eventData, (x, y) => x.OnHandOne(eventData));
					break;
				case HandStatus.Two:
					ExecuteEvents.Execute<IHandTwoHandler>(eventData.two, eventData, (x, y) => x.OnHandTwo(eventData));
					break;
				case HandStatus.Three:
					ExecuteEvents.Execute<IHandThreeHandler>(eventData.three, eventData, (x, y) => x.OnHandThree(eventData));
					break;
				case HandStatus.Four:
					ExecuteEvents.Execute<IHandFourHandler>(eventData.four, eventData, (x, y) => x.OnHandFour(eventData));
					break;
				case HandStatus.Open:
					ExecuteEvents.Execute<IHandOpenHandler>(eventData.open, eventData, (x, y) => x.OnHandOpen(eventData));
					break;
				case HandStatus.Closed:
					ExecuteEvents.Execute<IHandClosedHandler>(eventData.closed, eventData, (x, y) => x.OnHandClosed(eventData));
					break;
				case HandStatus.Rockin:
					ExecuteEvents.Execute<IHandRockinHandler>(eventData.rockin, eventData, (x, y) => x.OnHandRockin(eventData));
					break;
			}
		}

		protected void ExecutePoseUp(HandStatus status) {
            if (statusPairings[status] == null)
                return;

			switch (status) {
				case HandStatus.One:
					ExecuteEvents.Execute<IHandOneUpHandler>(eventData.one, eventData, (x, y) => x.OnHandOneUp(eventData));
					eventData.one = null;
					break;
				case HandStatus.Two:
					ExecuteEvents.Execute<IHandTwoUpHandler>(eventData.two, eventData, (x, y) => x.OnHandTwoUp(eventData));
					eventData.two = null;
					break;
				case HandStatus.Three:
					ExecuteEvents.Execute<IHandThreeUpHandler>(eventData.three, eventData, (x, y) => x.OnHandThreeUp(eventData));
					eventData.three = null;
					break;
				case HandStatus.Four:
					ExecuteEvents.Execute<IHandFourUpHandler>(eventData.four, eventData, (x, y) => x.OnHandFourUp(eventData));
					eventData.four = null;
					break;
				case HandStatus.Open:
					ExecuteEvents.Execute<IHandOpenUpHandler>(eventData.open, eventData, (x, y) => x.OnHandOpenUp(eventData));
					eventData.open = null;
					break;
				case HandStatus.Closed:
					ExecuteEvents.Execute<IHandClosedUpHandler>(eventData.closed, eventData, (x, y) => x.OnHandClosedUp(eventData));
					eventData.closed = null;
					break;
				case HandStatus.Rockin:
					ExecuteEvents.Execute<IHandRockinUpHandler>(eventData.rockin, eventData, (x, y) => x.OnHandRockinUp(eventData));
					eventData.rockin = null;
					break;
			}
			statusPairings[status] = null;
		}

		protected void ExecuteGlobalPoseDown(HandStatus status) {
			switch (status) {
				case HandStatus.One:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandOneDownHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandOneDown(eventData));
					break;
				case HandStatus.Two:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandTwoDownHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandTwoDown(eventData));
					break;
				case HandStatus.Three:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandThreeDownHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandThreeDown(eventData));
					break;
				case HandStatus.Four:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandFourDownHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandFourDown(eventData));
					break;
				case HandStatus.Open:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandOpenDownHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandOpenDown(eventData));
					break;
				case HandStatus.Closed:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandClosedDownHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandClosedDown(eventData));
					break;
				case HandStatus.Rockin:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandRockinDownHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandRockinDown(eventData));
					break;
			}
		}


		protected void ExecuteGlobalPose(HandStatus status) {
			switch (status) {
				case HandStatus.One:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandOneHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandOne(eventData));
					break;
				case HandStatus.Two:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandTwoHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandTwo(eventData));
					break;
				case HandStatus.Three:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandThreeHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandThree(eventData));
					break;
				case HandStatus.Four:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandFourHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandFour(eventData));
					break;
				case HandStatus.Open:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandOpenHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandOpen(eventData));
					break;
				case HandStatus.Closed:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandClosedHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandClosed(eventData));
					break;
				case HandStatus.Rockin:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandRockinHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandRockin(eventData));
					break;
			}
		}

		protected void ExecuteGlobalPoseUp(HandStatus status) {
			switch (status) {
				case HandStatus.One:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandOneUpHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandOneUp(eventData));
					break;
				case HandStatus.Two:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandTwoUpHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandTwoUp(eventData));
					break;
				case HandStatus.Three:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandThreeUpHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandThreeUp(eventData));
					break;
				case HandStatus.Four:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandFourUpHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandFourUp(eventData));
					break;
				case HandStatus.Open:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandOpenUpHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandOpenUp(eventData));
					break;
				case HandStatus.Closed:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandClosedUpHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandClosedUp(eventData));
					break;
				case HandStatus.Rockin:
					foreach (HandGlobalReceiver r in receivers)
						if (!r.module || r.module.Equals(this))
							ExecuteEvents.Execute<IGlobalHandRockinUpHandler>(r.gameObject, eventData,
								(x, y) => x.OnGlobalHandRockinUp(eventData));
					break;
			}
		}




		///////////////////////////////////////////////////////////////////////////
		//
		// Helper Functions
		//

		bool GetPoseDown(HandStatus status) {
			if (this.previousHandStatus != status && this.currentHandStatus == status) {
				return true;
			} else {
				return false;
			}
		}

		bool GetPose(HandStatus status) {
			if (this.previousHandStatus == status && this.currentHandStatus == status) {
				return true;
			} else {
				return false;
			}
		}

		bool GetPoseUp(HandStatus status) {
			if (this.previousHandStatus == status && this.currentHandStatus != status) {
				return true;
			} else {
				return false;
			}
		}

		protected Transform MakeFinger(string fingerLabel, string name) {
			Transform t = new GameObject().transform;
			t.SetParent(this.transform);
			t.name = hand_label + name;
			ObjectWithOffsetController c = t.gameObject.AddComponent<ObjectWithOffsetController>();
			c.label = fingerLabel;
			c.root_label = root_label;
			return t;
		}

		protected bool IsFingerOpen(Transform finger, float div = 1f) {
			return (Vector3.Angle(transform.up, finger.up) < OPEN_ANGLE / div);
		}
	}
}

