//Version=1.1
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
[System.Serializable]
public class UNOShaderHelper : MonoBehaviour
{
	[System.Serializable]	
	public class UNOShaderHelperData
	{
		public string matName = "";
		public string usingShader ="";
		public bool shaderVisible = true;

		[System.Serializable]	
		public class UNOShaderAnimData
		{
			public string propertyName = "";
			public enum AnimateUVs {No,Scroll,Rotate};
			public AnimateUVs AnimateUV;
			public bool instantiate = true;
			public Vector2 ScrollSpeed = new Vector2( -0.4f, 0.0f );
			public Vector2 UVOffset = Vector2.zero;
			public float RotateSpeed = 100f;
			public float UVRotate = 0f;
			public Vector2 RotatePivot = new Vector2(0.5f, 0.5f);
			public string matrix ="";
		}
		public List<UNOShaderAnimData> animate = new List<UNOShaderAnimData>(); // -- creates a list using class created
	}

	
	public List<UNOShaderHelperData> materialsList = new List<UNOShaderHelperData>(); // -- creates a list using class created


	void Start () 
	{
		for(int i = 0; i < GetComponent<Renderer>().sharedMaterials.Length; i++)
		{
			if (GetComponent<Renderer>().sharedMaterials[i] != null)
			{			
				UNOShaderHelperData cUHD = materialsList[i];// sets it as the current HelperData List 
				for(int ib = 0; ib < cUHD.animate.Count; ib++)// go through all the animate list and see if any is set
				{
					if 	
					(
						(cUHD.animate[ib].AnimateUV == UNOShaderHelperData.UNOShaderAnimData.AnimateUVs.Scroll
						|cUHD.animate[ib].AnimateUV == UNOShaderHelperData.UNOShaderAnimData.AnimateUVs.Rotate)
						& GetComponent<Renderer>().sharedMaterials[i].HasProperty(cUHD.animate[ib].propertyName)
					)
					{
						UNOShaderUVAnimate tempComp = (UNOShaderUVAnimate) gameObject.AddComponent<UNOShaderUVAnimate>();
						tempComp.compUNOShaderHelper = (UNOShaderHelper) gameObject.GetComponent<UNOShaderHelper>();
						tempComp.matId = i;
						break;
					}
				}
			}
		}	
	}
	
	void Update () 
	{

	
		
	}
	public void UpdateMatrixNames()//new in unoshader unlit 1.3+
	{
		if(materialsList.Count > 0)
		{
			for(int i = 0; i < materialsList.Count; i++)
			{
				if(materialsList[i].animate.Count > 0)
				{
					for(int ib = 0; ib < materialsList[i].animate.Count; ib++)
					{
						string propertyNameTemp = "";
						propertyNameTemp = materialsList[i].animate[ib].propertyName;// get property name
						string newMatrixName = propertyNameTemp + "Matrix";
						materialsList[i].animate[ib].matrix = newMatrixName;					
					}
				}
			}
		}

	}
	public void AnimateAdd (int matId,string propTex,string propMatrix) 
	{
		if(materialsList.Count>0)
		{
			bool foundProperty = false;		
			if(materialsList[matId] != null)
			{
				for(int i = 0; i<materialsList[matId].animate.Count;i++) // look to see if property is in animate list
				{
					if(materialsList[matId].animate[i].propertyName == propTex)
					{
						foundProperty = true;
						break;
					}
				}
				if (!foundProperty)// if its not found in animate list add it to the end
				{
					UNOShaderHelperData.UNOShaderAnimData newData =  new UNOShaderHelperData.UNOShaderAnimData();
					newData.propertyName =(propTex);
					newData.matrix = (propMatrix);
					materialsList[matId].animate.Insert(materialsList[matId].animate.Count,newData);// 				
				}
			}
		}
	}
	public void MatchMaterialCount()
	{
		for(int i = 0; i < materialsList.Count; i++)
		{
			if (i < GetComponent<Renderer>().sharedMaterials.Length)
			{
				if (GetComponent<Renderer>().sharedMaterials[i] != null)
				{
					if(materialsList[i] != null)
					{
						materialsList[i].matName = GetComponent<Renderer>().sharedMaterials[i].name;//--- set the name of material in list
					}					
				}
			}
		}


		if(materialsList.Count != GetComponent<Renderer>().sharedMaterials.Length)

		{
			
			if (materialsList.Count < GetComponent<Renderer>().sharedMaterials.Length)
			{
				if (materialsList.Count == 0)
				{
					UNOShaderHelperData[] newUNOs = new UNOShaderHelperData[1];
					materialsList.AddRange(newUNOs);
				}
				else
				{
					int dif = GetComponent<Renderer>().sharedMaterials.Length - materialsList.Count;
					UNOShaderHelperData[] newUNOs = new UNOShaderHelperData[dif];
					materialsList.AddRange(newUNOs);
				}
			}
			else
			{
//				if(materialsList.Count > matLength)
//				{
//					int dif = materialsList.Count - matLength;
//					materialsList.RemoveRange(matLength,dif);	
//				}
//				else
//				{
//					
//				}
			}

		}

	}

}
