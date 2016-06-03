using UnityEngine;
using System.Collections;

public class DiffuseDecalTint : MonoBehaviour 
{
	public Color[] colorsBase;
	public Color[] colorsDiffuse;
	public Color[] colorsDecal;

	public int currentIndex = 0;
	private int nextIndex;
	
	public float changeColourTime = 2.0f;
	

	private float timer = 0.0f;
	
	void Start() {
		if (colorsBase == null || colorsBase.Length < 2)
			Debug.Log ("Need to setup colors array in inspector");
		if (colorsDiffuse == null || colorsDiffuse.Length < 2)
			Debug.Log ("Need to setup colors array in inspector");
		if (colorsDecal == null || colorsDecal.Length < 2)
			Debug.Log ("Need to setup colors array in inspector");
		
		nextIndex = (currentIndex + 1) % colorsBase.Length;
	}
	
	void Update() {
		
		timer += Time.deltaTime;
		
		if (timer > changeColourTime) {
			currentIndex = (currentIndex + 1) % colorsBase.Length;
			nextIndex = (currentIndex + 1) % colorsBase.Length;
			timer = 0.0f;
			
		}
		Color colorTemp = Color.Lerp (colorsBase[currentIndex], colorsBase[nextIndex], timer / changeColourTime );
		GetComponent<Renderer>().sharedMaterial.SetColor("_ColorBase", colorTemp);

		Color colorDiffuse = Color.Lerp (colorsDiffuse[currentIndex], colorsDiffuse[nextIndex], timer / changeColourTime );
		GetComponent<Renderer>().sharedMaterial.SetColor("_DiffuseColor", colorDiffuse);

		Color colorDecal = Color.Lerp (colorsDecal[currentIndex], colorsDecal[nextIndex], timer / changeColourTime );
		GetComponent<Renderer>().sharedMaterial.SetColor("_DecalColor", colorDecal);

	}
}

