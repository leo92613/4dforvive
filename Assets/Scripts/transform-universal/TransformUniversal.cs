using UnityEngine;
using System.Collections;

public class TransformUniversal : MonoBehaviour {

	public float globalTimeScale = 1;

	public bool doTranslate = false;
	public Vector3 translateSpeed = Vector3.zero;
	public bool translateUseBounds = false;
	public bool pingPong = false;
	private Vector3 direction = Vector3.one;
	public Vector3 translateOffset = Vector3.zero;
	public Vector3 translateUpperBounds = Vector3.zero;
	public Vector3 translateLowerBounds = Vector3.zero;
	private Vector3 translate = Vector3.zero;

	private float translateBounceFix = 3;

	public bool doTranslateOscillate = false;
	public Vector3 translateOscillateUpperBounds = Vector3.zero;
	public Vector3 translateOscillateLowerBounds = Vector3.zero;
	public Vector3 translateOscillateSpeed = Vector3.zero;
	public Vector3 translateOscillateOffset = Vector3.zero;
	private Vector3 translateOscillateCounter = Vector3.zero;
	private Vector3 translateOscillate = Vector3.zero;
	
	public bool doTranslateNoise = false;
	public Vector3 translateNoiseUpperBounds = Vector3.zero;
	public Vector3 translateNoiseLowerBounds = Vector3.zero;
	public Vector3 translateNoiseSpeed = Vector3.zero;
	public Vector3 translateNoiseOffset = Vector3.zero;
	private Vector3 translateNoiseCounter = Vector3.zero;
	private Vector3 translateNoise = Vector3.zero;

	public bool doRotate = false;
	public Vector3 rotate = Vector3.zero;
	private Vector3 rotation = Vector3.zero;

	public bool doRotateOscillate = false;
	public Vector3 rotateOscillateUpperBounds = Vector3.zero;
	public Vector3 rotateOscillateLowerBounds = Vector3.zero;
	public Vector3 rotateOscillateSpeed = Vector3.zero;
	public Vector3 rotateOscillateOffset = Vector3.zero;
	private Vector3 rotateOscillateCounter = Vector3.zero;
	private Vector3 rotationOscillate = Vector3.zero;

	public bool doRotateNoise = false;
	public Vector3 rotateNoiseUpperBounds = Vector3.zero;
	public Vector3 rotateNoiseLowerBounds = Vector3.zero;
	public Vector3 rotateNoiseSpeed = Vector3.zero;
	public Vector3 rotateNoiseOffset = Vector3.zero;
	private Vector3 rotateNoiseCounter = Vector3.zero;
	private Vector3 rotationNoise = Vector3.zero;

	public bool doScale = false;
	public bool useInitialScale = true;
	public Vector3 scale = Vector3.zero;
	public Vector3 scaleOffset = Vector3.zero;
	private Vector3 scalar = Vector3.zero;

	public bool scaleUseBounds = false;
	public bool scalePingPong = false;
	public Vector3 scaleDirection = Vector3.one;
	public Vector3 scaleUpperBounds = Vector3.zero;
	public Vector3 scaleLowerBounds = Vector3.zero;
	
	public bool doScaleOscillate = false;
	public Vector3 scaleOscillateUpperBounds = Vector3.zero;
	public Vector3 scaleOscillateLowerBounds = Vector3.zero;
	public Vector3 scaleOscillateSpeed = Vector3.zero;
	public Vector3 scaleOscillateOffset = Vector3.zero;
	private Vector3 scaleOscillateCounter = Vector3.zero;
	private Vector3 scaleOscillate = Vector3.zero;

	public bool doScaleNoise = false;
	public Vector3 scaleNoiseUpperBounds = Vector3.zero;
	public Vector3 scaleNoiseLowerBounds = Vector3.zero;
	public Vector3 scaleNoiseSpeed = Vector3.zero;
	public Vector3 scaleNoiseOffset = Vector3.zero;
	private Vector3 scaleNoiseCounter = Vector3.zero;
	private Vector3 scaleNoise = Vector3.zero;

	private Vector3 initialRotation;
	private Vector3 initialPosition;
	private Vector3 initialScale;

	// Use this for initialization
	void Start () {
		initialPosition = transform.localPosition;
		initialRotation = transform.localEulerAngles;
		initialScale = transform.localScale;
		scalar += scaleOffset;
		translate += translateOffset;
	}
	
	// Update is called once per frame
	void Update () {

		if (doTranslate) {

			translate += Vector3.Scale(translateSpeed,direction)*Time.deltaTime * globalTimeScale;

			if(translateBounceFix>0)
				translateBounceFix-=.5f;

			if(translateUseBounds){// && !(translateBounceFix>0)){
				if(translate.x>=translateUpperBounds.x){
					if(pingPong)
						direction.x=-1;
					else translate.x = translateLowerBounds.x;
					translateBounceFix=3;
				}
				else if(translate.x<=translateLowerBounds.x){
					if(pingPong)
						direction.x=1;
					else translate.x = translateUpperBounds.x;
				}
				if(translate.y>=translateUpperBounds.y){
					if(pingPong)
						direction.y=-1;
					else translate.y = translateLowerBounds.y;
					translateBounceFix=3;
				}
				else if(translate.y<=translateLowerBounds.y){
					if(pingPong)
						direction.y=1;
					else translate.y = translateUpperBounds.y;
				}
				if(translate.z>=translateUpperBounds.z){
					if(pingPong)
						direction.z=-1;
					else translate.z = translateLowerBounds.z;
					translateBounceFix=3;
				}
				else if(translate.z<=translateLowerBounds.z){
					if(pingPong)
						direction.z=1;
					else translate.z = translateUpperBounds.z;
				}
			}
		}

		if (doTranslateOscillate) {
			translateOscillateCounter += translateOscillateSpeed * Time.deltaTime * globalTimeScale;

			translateOscillate = new Vector3(
				Remap(Mathf.Sin(translateOscillateOffset.x+translateOscillateCounter.x),-1,1,translateOscillateLowerBounds.x,translateOscillateUpperBounds.x),
				Remap(Mathf.Sin(translateOscillateOffset.y+translateOscillateCounter.y),-1,1,translateOscillateLowerBounds.y,translateOscillateUpperBounds.y),
				Remap(Mathf.Sin(translateOscillateOffset.z+translateOscillateCounter.z),-1,1,translateOscillateLowerBounds.z,translateOscillateUpperBounds.z));
		}

		if (doTranslateNoise) {
			translateNoiseCounter += translateNoiseSpeed * Time.deltaTime * globalTimeScale;
			
			translateNoise = new Vector3(
				Remap(Noise(translateNoiseOffset.x+translateNoiseCounter.x),0,1,translateNoiseLowerBounds.x,translateNoiseUpperBounds.x),
				Remap(Noise(translateNoiseOffset.y+translateNoiseCounter.y),0,1,translateNoiseLowerBounds.y,translateNoiseUpperBounds.y),
				Remap(Noise(translateNoiseOffset.z+translateNoiseCounter.z),0,1,translateNoiseLowerBounds.z,translateNoiseUpperBounds.z));
		}


		transform.localPosition = translate + initialPosition + translateOscillate + translateNoise;

		if(doRotate){
			rotation += rotate*Time.deltaTime * globalTimeScale;
		}

		if (doRotateOscillate) {
			rotateOscillateCounter += rotateOscillateSpeed * Time.deltaTime * globalTimeScale;
			
			rotationOscillate = new Vector3(
				Remap(Mathf.Sin(rotateOscillateOffset.x+rotateOscillateCounter.x),-1,1,rotateOscillateLowerBounds.x,rotateOscillateUpperBounds.x),
				Remap(Mathf.Sin(rotateOscillateOffset.y+rotateOscillateCounter.y),-1,1,rotateOscillateLowerBounds.y,rotateOscillateUpperBounds.y),
				Remap(Mathf.Sin(rotateOscillateOffset.z+rotateOscillateCounter.z),-1,1,rotateOscillateLowerBounds.z,rotateOscillateUpperBounds.z));
		}
		
		if (doRotateNoise) {
			rotateNoiseCounter += rotateNoiseSpeed * Time.deltaTime * globalTimeScale;
			
			rotationNoise = new Vector3(
				Remap(Noise(rotateNoiseOffset.x+rotateNoiseCounter.x),0,1,rotateNoiseLowerBounds.x,rotateNoiseUpperBounds.x),
				Remap(Noise(rotateNoiseOffset.y+rotateNoiseCounter.y),0,1,rotateNoiseLowerBounds.y,rotateNoiseUpperBounds.y),
				Remap(Noise(rotateNoiseOffset.z+rotateNoiseCounter.z),0,1,rotateNoiseLowerBounds.z,rotateNoiseUpperBounds.z));
		}

		transform.localEulerAngles = initialRotation + rotation + rotationOscillate + rotationNoise;

		if(doScale){

			scalar += Vector3.Scale(scale,scaleDirection)*Time.deltaTime * globalTimeScale;

			if(scaleUseBounds){

				if(scalar.x>=scaleUpperBounds.x){
					scalar.x=scaleUpperBounds.x;
					if(scalePingPong)
						scaleDirection.x*=-1;
					else scalar.x = scaleLowerBounds.x;
				}
				else if(scalar.x<=scaleLowerBounds.x){
					scalar.x=scaleLowerBounds.x;
					if(scalePingPong)
						scaleDirection.x*=-1;
					else scalar.x = scaleUpperBounds.x;
				}
				if(scalar.y>=scaleUpperBounds.y){
					scalar.y=scaleUpperBounds.y;
					if(scalePingPong)
						scaleDirection.y*=-1;
					else scalar.y = scaleLowerBounds.y;
				}
				else if(scalar.y<=scaleLowerBounds.y){
					scalar.y=scaleLowerBounds.y;
					if(scalePingPong)
						scaleDirection.y*=-1;
					else scalar.y = scaleUpperBounds.y;
				}
				if(scalar.z>=scaleUpperBounds.z){
					scalar.z=scaleUpperBounds.z;
					if(scalePingPong)
						scaleDirection.z*=-1;
					else scalar.z = scaleLowerBounds.z;
				}
				else if(scalar.z<=scaleLowerBounds.z){
					scalar.z=scaleLowerBounds.z;
					if(scalePingPong)
						scaleDirection.z*=-1;
					else scalar.z = scaleUpperBounds.z;
				}
			}
		}
		
		if (doScaleOscillate) {
			scaleOscillateCounter += scaleOscillateSpeed * Time.deltaTime * globalTimeScale;
			
			scaleOscillate = new Vector3(
				Remap(Mathf.Sin(scaleOscillateOffset.x+scaleOscillateCounter.x),-1,1,scaleOscillateLowerBounds.x,scaleOscillateUpperBounds.x),
				Remap(Mathf.Sin(scaleOscillateOffset.y+scaleOscillateCounter.y),-1,1,scaleOscillateLowerBounds.y,scaleOscillateUpperBounds.y),
				Remap(Mathf.Sin(scaleOscillateOffset.z+scaleOscillateCounter.z),-1,1,scaleOscillateLowerBounds.z,scaleOscillateUpperBounds.z));
		}
		
		if (doScaleNoise) {
			scaleNoiseCounter += scaleNoiseSpeed * Time.deltaTime * globalTimeScale;
			
			scaleNoise = new Vector3(
				Remap(Noise(scaleNoiseOffset.x+scaleNoiseCounter.x),0,1,scaleNoiseLowerBounds.x,scaleNoiseUpperBounds.x),
				Remap(Noise(scaleNoiseOffset.y+scaleNoiseCounter.y),0,1,scaleNoiseLowerBounds.y,scaleNoiseUpperBounds.y),
				Remap(Noise(scaleNoiseOffset.z+scaleNoiseCounter.z),0,1,scaleNoiseLowerBounds.z,scaleNoiseUpperBounds.z));
		}

		if(useInitialScale)
			transform.localScale = initialScale + scalar + scaleOscillate + scaleNoise;
		else
			transform.localScale = scalar + scaleOscillate + scaleNoise;

	}

	public float Remap (float value, float from1, float to1, float from2, float to2) {
		return (value - from1) / (to1 - from1) * (to2 - from2) + from2;
	}

	public float Noise (float t){
		return Mathf.PerlinNoise(t,t*1.5f);
	}
}
