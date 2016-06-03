//using UnityEngine;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEngine.EventSystems;
//using System;

//public class ViveTransformable : MonoBehaviour, ITriggerPressSetHandler {

//	public static List<ViveTransformable> transforms = new List<ViveTransformable>();

//	public enum PlanetState { IDLE, GRAB, SCALE };
//	public PlanetState state = PlanetState.IDLE;

//	private Transform primaryController;
//	private Transform secondaryController;

//	private Vector3 initialScale;
//	private float initialDistance;
//	private float primaryInitialDistance;
//	private float secondaryInitialDistance;
//	private Vector3 primaryScalePoint;
//	private Vector3 secondaryScalePoint;


//	////////////////////////////////////////
//	//MONOBEHAVIOUR FUNCTIONS
//	////////////////////////////////////////

//	// Use this for initialization
//	void Start() {

//	}

//	void OnEnable() {
//		transforms.Add(this);
//	}

//	void OnDisable() {
//		transforms.Remove(this);
//	}

//	// Update is called once per frame
//	void Update() {
//		if (this.state.Equals(PlanetState.GRAB)) {
//			this.transform.position = primaryController.position + primaryController.forward * primaryInitialDistance;
//		} else if (this.state.Equals(PlanetState.SCALE)) {
//			float dist = Vector3.Distance(primaryController.position, secondaryController.position);
//			this.transform.localScale = initialScale * (dist / initialDistance);
//		}
//	}

//	////////////////////////////////////////
//	//VIVE EVENT SYSTEM FUNCTIONS
//	////////////////////////////////////////

//	public void OnTriggerPressDown(ViveEventData eventData) {
//		if (primaryController == null) {
//			this.state = PlanetState.GRAB;
//			primaryController = eventData.module.transform;
//			primaryInitialDistance = eventData.pointerCurrentRaycast.distance +
//				Vector3.Distance(this.transform.position, eventData.pointerCurrentRaycast.worldPosition); //ADD RADIUS
//			primaryScalePoint = eventData.pointerCurrentRaycast.worldPosition;
//		} else if (secondaryController == null) {
//			this.state = PlanetState.SCALE;
//			secondaryInitialDistance = eventData.pointerCurrentRaycast.distance +
//				Vector3.Distance(this.transform.position, eventData.pointerCurrentRaycast.worldPosition); //ADD RADIUS
//			secondaryScalePoint = eventData.pointerCurrentRaycast.worldPosition;
//			secondaryController = eventData.controllerTransform;
//			initialDistance = Vector3.Distance(primaryController.position, secondaryController.position);
//			initialScale = this.transform.localScale;
//		}
//	}

//	public void OnTriggerPress(ViveEventData eventData) {
//		Transform t = eventData.module.boundObject;
//		if (t == primaryController) {
//			primaryScalePoint = t.position + t.forward * primaryInitialDistance;
//		} else if (t == secondaryController) {
//			secondaryScalePoint = t.position + t.forward * secondaryInitialDistance;
//		}
//	}

//	public void OnTriggerPressUp(ViveEventData eventData) {
//		if (eventData.module.boundObject == primaryController) {
//			primaryController = null;
//			secondaryController = null;

//			this.state = PlanetState.IDLE;

//		} else if (eventData.module.boundObject == secondaryController) {
//			secondaryController = null;
//			this.state = PlanetState.GRAB;
//		}
//	}


//	////////////////////////////////////////
//	//STATIC FUNCTIONS
//	////////////////////////////////////////

//	public static ViveTransformable FindClosest(Vector3 pos, float range) {
//		float c = float.MaxValue;
//		ViveTransformable closest = null;

//		foreach (ViveTransformable t in transforms) {
//			float dist = Vector3.Distance(pos, t.transform.position);
//			if (dist < c && dist < range) {
//				c = dist;
//				closest = t;
//			}
//		}

//		return closest;
//	}


//}