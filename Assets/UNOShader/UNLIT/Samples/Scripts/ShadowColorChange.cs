using UnityEngine;
using System.Collections;

public class ShadowColorChange : MonoBehaviour 
{
	public Color[] colors;
	
	public int currentIndex = 0;
	private int nextIndex;
	
	public float changeColourTime = 2.0f;
	

	private float timer = 0.0f;
	
	void Start() {
		if (colors == null || colors.Length < 2)
			Debug.Log ("Need to setup colors array in inspector");
		
		nextIndex = (currentIndex + 1) % colors.Length;
	}
	
	void Update() {
		
		timer += Time.deltaTime;
		
		if (timer > changeColourTime) {
			currentIndex = (currentIndex + 1) % colors.Length;
			nextIndex = (currentIndex + 1) % colors.Length;
			timer = 0.0f;
			
		}
		Color colorTemp = Color.Lerp (colors[currentIndex], colors[nextIndex], timer / changeColourTime );
		GetComponent<Renderer>().sharedMaterial.SetColor("_Color", colorTemp);
		Shader.SetGlobalColor ("_UNOShaderShadowColor",colorTemp);
	}
}

