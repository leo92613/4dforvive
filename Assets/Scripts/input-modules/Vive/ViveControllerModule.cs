using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using Valve.VR;

namespace Holojam.IO {
	public class ViveEventData : PointerEventData {
		public ViveControllerModule module;
		public SteamVR_TrackedObject controller;
		public GameObject currentRaycast;
		public GameObject previousRaycast;
		public Vector2 touchpadAxis;
		public Vector2 triggerAxis;
		public GameObject applicationMenuPress;
		public GameObject gripPress;
		public GameObject touchpadPress;
		public GameObject triggerPress;
		public GameObject touchpadTouch;
		public GameObject triggerTouch;

		public ViveEventData(EventSystem eventSystem)
			: base(eventSystem) {
		}
	}

	[RequireComponent(typeof(SteamVR_TrackedObject))]
	public class ViveControllerModule : MonoBehaviour {


		/// <summary>
		/// 
		/// </summary>
		/// <TODO>
		///     * Handle a global receiver joining amidst a button being pressed.
		/// </TODO>

		/////Public/////
		//References
		public static List<ViveGlobalReceiver> receivers = new List<ViveGlobalReceiver>();
		public Transform boundObject;
		//Primitives
		//public bool forceModuleActive = false;
		public bool debugMode = false;
		public string interactTag;
		public float interactDistance = 10f;

		/////Private/////
		//References
		private Dictionary<EVRButtonId, GameObject> pressPairings = new Dictionary<EVRButtonId, GameObject>();
		//private Dictionary<ulong, List<ViveGlobalReceiver>> pressReceivers = new Dictionary<ulong, List<ViveGlobalReceiver>>();
		private Dictionary<EVRButtonId, GameObject> touchPairings = new Dictionary<EVRButtonId, GameObject>();
		//private Dictionary<ulong, List<ViveGlobalReceiver>> touchReceivers = new Dictionary<ulong, List<ViveGlobalReceiver>>();
		private SteamVR_TrackedObject controller;
		private ViveEventData eventData;
		//Primitives


		//Steam Controller button and axis ids
		EVRButtonId[] buttonIds = new EVRButtonId[] {
			EVRButtonId.k_EButton_ApplicationMenu,
			EVRButtonId.k_EButton_Grip,
			EVRButtonId.k_EButton_SteamVR_Touchpad,
			EVRButtonId.k_EButton_SteamVR_Trigger
		};

		EVRButtonId[] touchIds = new EVRButtonId[] {
			EVRButtonId.k_EButton_SteamVR_Touchpad,
			EVRButtonId.k_EButton_SteamVR_Trigger
		};

		EVRButtonId[] axisIds = new EVRButtonId[] {
			EVRButtonId.k_EButton_SteamVR_Touchpad,
			EVRButtonId.k_EButton_SteamVR_Trigger
		};




		void Awake() {
			eventData = new ViveEventData (EventSystem.current);

			controller = this.GetComponent<SteamVR_TrackedObject>();
			eventData.module = this;
			eventData.controller = this.controller;

			foreach (EVRButtonId button in buttonIds) {
				pressPairings.Add(button, null);
			}

			foreach (EVRButtonId button in touchIds) {
				touchPairings.Add(button, null);
			}
		}

		void OnDisable() {
			foreach (EVRButtonId button in buttonIds) {
				this.ExecutePressUp(button);
				this.ExecuteGlobalPressUp(button);
			}

			foreach (EVRButtonId button in touchIds) {
				this.ExecuteTouchUp(button);
				this.ExecuteGlobalTouchUp(button);
			}

			eventData.currentRaycast = null;
			this.UpdateCurrentObject();

		}

		void Update() {
			this.Process();
		}

		void Process() {
			this.PositionBoundObject();
			this.CastRayFromBoundObject();
			this.UpdateCurrentObject();
			//this.PlaceCursor();
			this.HandleButtons();
		}

		void PositionBoundObject() {
			if (boundObject == null)
				boundObject = this.transform;
			boundObject.localPosition = controller.transform.localPosition;
			boundObject.localRotation = controller.transform.localRotation;
		}

		private List<RaycastHit> hits = new List<RaycastHit>();
		private Ray ray;
		void CastRayFromBoundObject() {
			hits.Clear();

			//CAST RAY
			Vector3 v = boundObject.position;
			Quaternion q = boundObject.rotation;
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

		void HandlePointerExitAndEnter(ViveEventData eventData) {
			if (eventData.previousRaycast != eventData.currentRaycast) {
				ExecuteEvents.Execute<IPointerEnterHandler>(eventData.currentRaycast, eventData, ExecuteEvents.pointerEnterHandler);
				ExecuteEvents.Execute<IPointerExitHandler>(eventData.previousRaycast, eventData, ExecuteEvents.pointerExitHandler);
			}
		}

		void PlaceCursor() {
			//TODO.
		}

		void HandleButtons() {
			int index = (int)controller.index;

			eventData.touchpadAxis = SteamVR_Controller.Input(index).GetAxis(axisIds[0]);
			eventData.triggerAxis = SteamVR_Controller.Input(index).GetAxis(axisIds[1]);

			//Press
			foreach (EVRButtonId button in buttonIds) {
				if (this.GetPressDown(index, button)) {
					this.ExecutePressDown(button);
					this.ExecuteGlobalPressDown(button);
				} else if (this.GetPress(index, button)) {
					this.ExecutePress(button);
					this.ExecuteGlobalPress(button);
				} else if (this.GetPressUp(index, button)) {
					this.ExecutePressUp(button);
					this.ExecuteGlobalPressUp(button);
				}
			}

			//Touch
			foreach (EVRButtonId button in touchIds) {
				if (this.GetTouchDown(index, button)) {
					this.ExecuteTouchDown(button);
					this.ExecuteGlobalTouchDown(button);
				} else if (this.GetTouch(index, button)) {
					this.ExecuteTouch(button);
					this.ExecuteGlobalTouch(button);
				} else if (this.GetTouchUp(index, button)) {
					this.ExecuteTouchUp(button);
					this.ExecuteGlobalTouchUp(button);
				}
			}
		}

		private void ExecutePressDown(EVRButtonId id) {
			GameObject go = eventData.currentRaycast;
			if (go == null)
				return;

			switch (id) {
			case EVRButtonId.k_EButton_ApplicationMenu:
				eventData.applicationMenuPress = go;
				ExecuteEvents.Execute<IApplicationMenuPressDownHandler>(eventData.applicationMenuPress, eventData,
					(x, y) => x.OnApplicationMenuPressDown(eventData));
				break;
			case EVRButtonId.k_EButton_Grip:
				eventData.gripPress = go;
				ExecuteEvents.Execute<IGripPressDownHandler>(eventData.gripPress, eventData,
					(x, y) => x.OnGripPressDown(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Touchpad:
				eventData.touchpadPress = go;
				ExecuteEvents.Execute<ITouchpadPressDownHandler>(eventData.touchpadPress, eventData,
					(x, y) => x.OnTouchpadPressDown(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Trigger:
				eventData.triggerPress = go;
				ExecuteEvents.Execute<ITriggerPressDownHandler>(eventData.triggerPress, eventData,
					(x, y) => x.OnTriggerPressDown(eventData));
				break;
			}

			//Add pairing.
			pressPairings[id] = go;
		}

		private void ExecutePress(EVRButtonId id) {
			if (pressPairings[id] == null)
				return;

			switch (id) {
			case EVRButtonId.k_EButton_ApplicationMenu:
				ExecuteEvents.Execute<IApplicationMenuPressHandler>(eventData.applicationMenuPress, eventData,
					(x, y) => x.OnApplicationMenuPress(eventData));
				break;
			case EVRButtonId.k_EButton_Grip:
				ExecuteEvents.Execute<IGripPressHandler>(eventData.gripPress, eventData,
					(x, y) => x.OnGripPress(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Touchpad:
				ExecuteEvents.Execute<ITouchpadPressHandler>(eventData.touchpadPress, eventData,
					(x, y) => x.OnTouchpadPress(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Trigger:
				ExecuteEvents.Execute<ITriggerPressHandler>(eventData.triggerPress, eventData,
					(x, y) => x.OnTriggerPress(eventData));
				break;
			}
		}

		private void ExecutePressUp(EVRButtonId id) {
			if (pressPairings[id] == null)
				return;

			switch (id) {
			case EVRButtonId.k_EButton_ApplicationMenu:
				ExecuteEvents.Execute<IApplicationMenuPressUpHandler>(eventData.applicationMenuPress, eventData,
					(x, y) => x.OnApplicationMenuPressUp(eventData));
				eventData.applicationMenuPress = null;
				break;
			case EVRButtonId.k_EButton_Grip:
				ExecuteEvents.Execute<IGripPressUpHandler>(eventData.gripPress, eventData,
					(x, y) => x.OnGripPressUp(eventData));
				eventData.gripPress = null;
				break;
			case EVRButtonId.k_EButton_SteamVR_Touchpad:
				ExecuteEvents.Execute<ITouchpadPressUpHandler>(eventData.touchpadPress, eventData,
					(x, y) => x.OnTouchpadPressUp(eventData));
				eventData.touchpadPress = null;
				break;
			case EVRButtonId.k_EButton_SteamVR_Trigger:
				ExecuteEvents.Execute<ITriggerPressUpHandler>(eventData.triggerPress, eventData,
					(x, y) => x.OnTriggerPressUp(eventData));
				eventData.triggerPress = null;
				break;
			}

			pressPairings[id] = null;
		}

		private void ExecuteTouchDown(EVRButtonId id) {
			GameObject go = eventData.currentRaycast;
			if (go == null)
				return;

			switch (id) {
			case EVRButtonId.k_EButton_SteamVR_Touchpad:
				eventData.touchpadTouch = go;
				ExecuteEvents.Execute<ITouchpadTouchDownHandler>(eventData.touchpadTouch, eventData,
					(x, y) => x.OnTouchpadTouchDown(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Trigger:
				eventData.triggerTouch = go;
				ExecuteEvents.Execute<ITriggerTouchDownHandler>(eventData.triggerTouch, eventData,
					(x, y) => x.OnTriggerTouchDown(eventData));
				break;
			}

			touchPairings[id] = go;
		}

		private void ExecuteTouch(EVRButtonId id) {
			if (touchPairings[id] == null)
				return;

			switch (id) {
			case EVRButtonId.k_EButton_SteamVR_Touchpad:
				ExecuteEvents.Execute<ITouchpadTouchHandler>(eventData.touchpadTouch, eventData,
					(x, y) => x.OnTouchpadTouch(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Trigger:
				ExecuteEvents.Execute<ITriggerTouchHandler>(eventData.triggerTouch, eventData,
					(x, y) => x.OnTriggerTouch(eventData));
				break;
			}
		}

		private void ExecuteTouchUp(EVRButtonId id) {
			if (touchPairings[id] == null)
				return;

			switch (id) {
			case EVRButtonId.k_EButton_SteamVR_Touchpad:
				ExecuteEvents.Execute<ITouchpadTouchUpHandler>(eventData.touchpadTouch, eventData,
					(x, y) => x.OnTouchpadTouchUp(eventData));
				eventData.touchpadTouch = null;
				break;
			case EVRButtonId.k_EButton_SteamVR_Trigger:
				ExecuteEvents.Execute<ITriggerTouchUpHandler>(eventData.triggerTouch, eventData,
					(x, y) => x.OnTriggerTouchUp(eventData));
				eventData.triggerTouch = null;
				break;
			}
		}

		private void ExecuteGlobalPressDown(EVRButtonId id) {
			switch (id) {
			case EVRButtonId.k_EButton_ApplicationMenu:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalApplicationMenuPressDownHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalApplicationMenuPressDown(eventData));
				break;
			case EVRButtonId.k_EButton_Grip:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalGripPressDownHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalGripPressDown(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Touchpad:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalTouchpadPressDownHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalTouchpadPressDown(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Trigger:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalTriggerPressDownHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalTriggerPressDown(eventData));
				break;
			}
		}

		private void ExecuteGlobalPress(EVRButtonId id) {
			switch (id) {
			case EVRButtonId.k_EButton_ApplicationMenu:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalApplicationMenuPressHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalApplicationMenuPress(eventData));
				break;
			case EVRButtonId.k_EButton_Grip:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalGripPressHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalGripPress(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Touchpad:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalTouchpadPressHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalTouchpadPress(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Trigger:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalTriggerPressHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalTriggerPress(eventData));
				break;
			}
		}

		private void ExecuteGlobalPressUp(EVRButtonId id) {
			switch (id) {
			case EVRButtonId.k_EButton_ApplicationMenu:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalApplicationMenuPressUpHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalApplicationMenuPressUp(eventData));
				break;
			case EVRButtonId.k_EButton_Grip:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalGripPressUpHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalGripPressUp(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Touchpad:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalTouchpadPressUpHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalTouchpadPressUp(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Trigger:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalTriggerPressUpHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalTriggerPressUp(eventData));
				break;
			}
		}

		private void ExecuteGlobalTouchDown(EVRButtonId id) {
			switch (id) {
			case EVRButtonId.k_EButton_SteamVR_Touchpad:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalTouchpadTouchDownHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalTouchpadTouchDown(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Trigger:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalTriggerTouchDownHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalTriggerTouchDown(eventData));
				break;
			}
		}

		private void ExecuteGlobalTouch(EVRButtonId id) {
			switch (id) {
			case EVRButtonId.k_EButton_SteamVR_Touchpad:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalTouchpadTouchHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalTouchpadTouch(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Trigger:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalTriggerTouchHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalTriggerTouch(eventData));
				break;
			}
		}

		private void ExecuteGlobalTouchUp(EVRButtonId id) {
			switch (id) {
			case EVRButtonId.k_EButton_SteamVR_Touchpad:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalTouchpadTouchUpHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalTouchpadTouchUp(eventData));
				break;
			case EVRButtonId.k_EButton_SteamVR_Trigger:
				foreach (ViveGlobalReceiver r in receivers)
					if (!r.module || r.module.Equals(this))
						ExecuteEvents.Execute<IGlobalTriggerTouchUpHandler>(r.gameObject, eventData,
							(x, y) => x.OnGlobalTriggerTouchUp(eventData));
				break;
			}
		}



		bool GetPressDown(int index, EVRButtonId button) {
			return SteamVR_Controller.Input(index).GetPressDown(button);
		}

		bool GetPress(int index, EVRButtonId button) {
			return SteamVR_Controller.Input(index).GetPress(button);
		}

		bool GetPressUp(int index, EVRButtonId button) {
			return SteamVR_Controller.Input(index).GetPressUp(button);
		}

		bool GetTouchDown(int index, EVRButtonId button) {
			return SteamVR_Controller.Input(index).GetTouchDown(button);
		}

		bool GetTouch(int index, EVRButtonId button) {
			return SteamVR_Controller.Input(index).GetTouch(button);
		}

		bool GetTouchUp(int index, EVRButtonId button) {
			return SteamVR_Controller.Input(index).GetTouchUp(button);
		}
	}

}

