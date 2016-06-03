//Version=1.1
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UNOShaderUVAnimate : MonoBehaviour
{
	public UNOShaderHelper compUNOShaderHelper;
	public int matId = 0;
	
	
	void Start () 
	{
		
		//helperComponent =(UNOShaderHelper) gameObject.GetComponent<UNOShaderHelper>();
		// --- Set offset values at start ---
		if (GetComponent<Renderer>().sharedMaterials[matId] != null)
		{			
			for(int i = 0; i <compUNOShaderHelper.materialsList[matId].animate.Count; i++)// go through all the animate list on this Material
			{
				if (compUNOShaderHelper.materialsList[matId].animate[i].AnimateUV == UNOShaderHelper.UNOShaderHelperData.UNOShaderAnimData.AnimateUVs.Scroll)// check if its set to scroll
				{
					if(GetComponent<Renderer>().sharedMaterials[matId].HasProperty(compUNOShaderHelper.materialsList[matId].animate[i].propertyName))// see if property name exists
					{
						compUNOShaderHelper.materialsList[matId].animate[i].UVOffset = GetComponent<Renderer>().sharedMaterials[matId].GetTextureOffset(compUNOShaderHelper.materialsList[matId].animate[i].propertyName);
					}
				}
			}
		}	
	}
	
	void Update () 
	{
	
		
	}
	
	void LateUpdate()
	{	
		if (GetComponent<Renderer>().sharedMaterials[matId] != null)
		{	
			for(int i = 0; i <compUNOShaderHelper.materialsList[matId].animate.Count; i++)// go through all the animate list on this Material
			{
				// ------------------------- Scroll  UVs -----------------------------------
				if (compUNOShaderHelper.materialsList[matId].animate[i].AnimateUV == UNOShaderHelper.UNOShaderHelperData.UNOShaderAnimData.AnimateUVs.Scroll)// if its set to scroll
				{
					if(GetComponent<Renderer>().sharedMaterials[matId].HasProperty(compUNOShaderHelper.materialsList[matId].animate[i].propertyName))// look for diffuse channel
					{
						compUNOShaderHelper.materialsList[matId].animate[i].UVOffset += ( compUNOShaderHelper.materialsList[matId].animate[i].ScrollSpeed * Time.deltaTime);
						
						if(compUNOShaderHelper.materialsList[matId].animate[i].instantiate)//-- instantiate
						{
							GetComponent<Renderer>().materials[matId].SetTextureOffset(compUNOShaderHelper.materialsList[matId].animate[i].propertyName, 
							                                           compUNOShaderHelper.materialsList[matId].animate[i].UVOffset
							                                           );
						}
						else
						{
							GetComponent<Renderer>().sharedMaterials[matId].SetTextureOffset(compUNOShaderHelper.materialsList[matId].animate[i].propertyName,
							                                                 compUNOShaderHelper.materialsList[matId].animate[i].UVOffset
							                                                 );
						}
					}
				}

				// ------------------------- Rotate  UVs -----------------------------------
				if (compUNOShaderHelper.materialsList[matId].animate[i].AnimateUV == UNOShaderHelper.UNOShaderHelperData.UNOShaderAnimData.AnimateUVs.Rotate)// if its set to scroll
				{
					Matrix4x4 t = Matrix4x4.TRS(-compUNOShaderHelper.materialsList[matId].animate[i].RotatePivot, Quaternion.identity, Vector3.one);
					Quaternion rotation = Quaternion.Euler
						(0, 0, compUNOShaderHelper.materialsList[matId].animate[i].UVRotate += compUNOShaderHelper.materialsList[matId].animate[i].RotateSpeed * Time.deltaTime);
					Matrix4x4 r = Matrix4x4.TRS(Vector3.zero, rotation, Vector3.one);  
					Matrix4x4 tInv = Matrix4x4.TRS(compUNOShaderHelper.materialsList[matId].animate[i].RotatePivot, Quaternion.identity, Vector3.one);
					
					if(compUNOShaderHelper.materialsList[matId].animate[i].instantiate)//-- instantiate
					{
						GetComponent<Renderer>().materials[matId].SetMatrix(compUNOShaderHelper.materialsList[matId].animate[i].matrix, tInv*r*t);
					}
					else
					{
						GetComponent<Renderer>().sharedMaterials[matId].SetMatrix(compUNOShaderHelper.materialsList[matId].animate[i].matrix, tInv*r*t);
					}
				}
			}
		}

	}	
}
