using UnityEngine;
using System.Collections;

public class FloatPropertyChange : MonoBehaviour 
{
	//public GameObject[] objects;
	public string propertyName;
	public float[] floatValues;
	public float time = 2.0f;
	
	int currentIndex = 0;
	int nextIndex;
	float timer = 0.0f;
	
	void Start() {
		if (floatValues == null || floatValues.Length < 2)
			Debug.Log ("Need to setup floatValues array in inspector");
		
		nextIndex = (currentIndex + 1) % floatValues.Length;
	}
	
	void Update() {
		
	
		timer += Time.deltaTime;
		
		if (timer > time) {
			currentIndex = (currentIndex + 1) % floatValues.Length;
			nextIndex = (currentIndex + 1) % floatValues.Length;
			timer = 0.0f;
			
		}
		float floatTemp = Mathf.Lerp(floatValues[currentIndex], floatValues[nextIndex], timer / time );
		GetComponent<Renderer>().sharedMaterial.SetFloat(propertyName, floatTemp);

	}
}