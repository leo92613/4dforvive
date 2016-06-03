using UnityEngine;
using System.Collections;

public class ColorPropertyChange : MonoBehaviour 
{
	//public GameObject[] objects;
	public string propertyName;
	public Color[] colorValues;
	public float time = 2.0f;
	
	int currentIndex = 0;
	int nextIndex;
	float timer = 0.0f;
	
	void Start() {
		if (colorValues == null || colorValues.Length < 2)
			Debug.Log ("Need to setup colorValues array in inspector");
		
		nextIndex = (currentIndex + 1) % colorValues.Length;
	}
	
	void Update() {
		
	
		timer += Time.deltaTime;

		if (timer > time) {
			currentIndex = (currentIndex + 1) % colorValues.Length;
			nextIndex = (currentIndex + 1) % colorValues.Length;
			timer = 0.0f;
			
		}

		Color colorTemp = Color.Lerp (colorValues[currentIndex], colorValues[nextIndex], timer / time );
		GetComponent<Renderer>().sharedMaterial.SetColor(propertyName, colorTemp);


	}
}