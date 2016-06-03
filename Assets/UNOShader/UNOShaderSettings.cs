using System.Collections;
using System.Collections.Generic;

using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]

public static class UNOShaderSettings
{
	public static bool inited;
	

	//_______________________________________ UNOShader Settings ________________________________________________

	public static bool debugMode = false;
	public static bool checkboxProperties = true;
	public static bool foldoutLights = false;
	public static bool hideWireframeSelected = false;



	public static string UNOShaderDirPath ="UNOShader/";//<<--- Change this path if UNOshader is in a different Directory ( path starts from the Assets directory)
	public static bool UNOShaderDirCorrectPath = true;
	public static string helpLink = "http://www.unoverse.com/unoshader";
		
	// --- UI Colors  ---
	public static Color UIColor = new Color(1f,1f,1f,1f);//white
	public static Color UIColor_White = new Color(1f,1f,1f,1f);//white
	public static Color UIColor_Update = new Color(1f,.85f,.1f,1f);//Yellow
	public static Color UIColor_CreateButton = new Color(.3f,.75f,1.2f,1);//Blue
	public static Color UIColor_CreatePrep = new Color(.4f,.55f,.7f,1);//Green
	public static Color UIColor_Active = new Color(1f,1f,1f,1f);//White
	public static Color UIColor_Inactive = new Color(.1f,.1f,.1f,1f);//Gray
	public static Color UIColor_Error = new Color(1f,.3f,.3f,1f);//White
	public static Color UIColor_Green = new Color(.3f,1.0f,.2f);//Green
	
	//--- UI Textures ---	
	public static Texture2D UNOShaderAnimation;
	static string UNOShaderAnimation_file = "UNOShaderAnimation.psd";
	
	public static Texture2D UNOShaderFoldoutDown;
	static string UNOShaderFoldoutDown_file = "UNOShaderFoldoutDown.psd";
	
	public static Texture2D UNOShaderFoldoutRight;
	static string UNOShaderFoldoutRight_file = "UNOShaderFoldoutRight.psd";
	
	public static Texture2D UNOShaderEye;
	static string UNOShaderEye_file = "UNOShaderEye.psd";
	
	public static Texture2D UNOShaderCheckBoxOff;
	static string UNOShaderCheckBoxOff_file = "UNOShaderCheckBox_Off.psd";
	
	public static Texture2D UNOShaderCheckBoxOn;
	static string UNOShaderCheckBoxOn_file = "UNOShaderCheckBox_On.psd";
	
	public static Texture2D UNOShaderWireframeOn;
	static string UNOShaderWireframeOn_file = "UNOShaderWireframe_On.psd";
	
	public static Texture2D UNOShaderWireframeOff;
	static string UNOShaderWireframeOff_file = "UNOShaderWireframe_Off.psd";
	
	public static Texture2D UNOShaderHelpLink;
	static string UNOShaderHelpLink_file = "UNOShaderHelpLink.psd";
	
	public static Texture2D Icon_DirLightOff;
	static string Icon_DirLightOff_file = "Icon_DirLightOff.psd";
	
	public static Texture2D Icon_DirLightOn;
	static string Icon_DirLightOn_file = "Icon_DirLightOn.psd";
	
	public static Texture2D Icon_ProbeLightsOn;
	static string Icon_ProbeLightsOn_file = "Icon_ProbeLightsOn.psd";
	public static Texture2D Icon_ProbeLightsOff;
	static string Icon_ProbeLightsOff_file = "Icon_ProbeLightsOff.psd";
	
	public static Texture2D Icon_VertexPointLightsOn;
	static string Icon_VertexPointLightsOn_file = "Icon_VertexPointLightsOn.psd";
	public static Texture2D Icon_VertexPointLightsOff;
	static string Icon_VertexPointLightsOff_file = "Icon_VertexPointLightsOff.psd";
	
	public static Texture2D Icon_SpotPointLightsOn;
	static string Icon_SpotPointLightsOn_file = "Icon_SpotPointLightsOn.psd";
	public static Texture2D Icon_SpotPointLightsOff;
	static string Icon_SpotPointLightsOff_file = "Icon_SpotPointLightsOff.psd";
	
	public static Texture2D Icon_ShadowsOn;
	static string Icon_ShadowsOn_file = "Icon_ShadowsOn.psd";
	public static Texture2D Icon_ShadowsOff;
	static string Icon_ShadowsOff_file = "Icon_ShadowsOff.psd";	
		
	//______________________________Objects and Scripts

	public static GameObject UNOShaderDataGameObject;// hidden game object for unoshader global data
	public static UNOShaderData comp_UNOShaderData;
	#if UNITY_EDITOR
	public static void Init()
	{
		if(inited & UNOShaderDataGameObject != null & UNOShaderDirCorrectPath == true)
		{
			return;
		}
		
		inited = true;
		AssetDatabase.Refresh();
		Object tempFolder = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin" ,typeof (Object))as Object;
		
		
		//================================================ Path Checking 
		if( tempFolder == null )
		{
			UNOShaderDirCorrectPath = false;
		}
		else
		{
			UNOShaderDirCorrectPath = true;
		}

		{
			//================================================ Render settings 
			//renderSettings = Object.FindObjectOfType(typeof(RenderSettings)) as RenderSettings;// assigns renderSettings

			//================================================ UnoShader Data
			UNOShaderDataGameObject = GameObject.Find("UNOShaderData");
			if (UNOShaderDataGameObject != null)
			{
				UNOShaderDataGameObject.hideFlags = HideFlags.HideInHierarchy;
				comp_UNOShaderData = (UNOShaderData)UNOShaderDataGameObject.GetComponent<UNOShaderData>();			
			}
			else
			{
				UNOShaderDataGameObject = new GameObject();
				UNOShaderDataGameObject.name = "UNOShaderData";
				UNOShaderDataGameObject.hideFlags = HideFlags.HideInHierarchy;
				UNOShaderDataGameObject.AddComponent<UNOShaderData>();
				comp_UNOShaderData = (UNOShaderData)UNOShaderDataGameObject.GetComponent<UNOShaderData>();
			}

			//================================================ UI texture paths
			UNOShaderFoldoutDown = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + UNOShaderFoldoutDown_file,typeof(Texture2D))as Texture2D;
			UNOShaderFoldoutRight = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + UNOShaderFoldoutRight_file,typeof(Texture2D))as Texture2D;
			UNOShaderEye = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + UNOShaderEye_file,typeof(Texture2D))as Texture2D;
			UNOShaderCheckBoxOff = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + UNOShaderCheckBoxOff_file,typeof(Texture2D))as Texture2D;
			UNOShaderCheckBoxOn = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + UNOShaderCheckBoxOn_file,typeof(Texture2D))as Texture2D;
			UNOShaderWireframeOff = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + UNOShaderWireframeOff_file,typeof(Texture2D))as Texture2D;
			UNOShaderWireframeOn = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + UNOShaderWireframeOn_file,typeof(Texture2D))as Texture2D;
			UNOShaderHelpLink = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + UNOShaderHelpLink_file,typeof(Texture2D))as Texture2D;
			
			//--- Icons ---
			UNOShaderAnimation = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + UNOShaderAnimation_file,typeof(Texture2D))as Texture2D;
			
			Icon_DirLightOn = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + Icon_DirLightOn_file,typeof(Texture2D))as Texture2D;
			Icon_DirLightOff = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + Icon_DirLightOff_file,typeof(Texture2D))as Texture2D;
			
			Icon_ProbeLightsOn = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + Icon_ProbeLightsOn_file,typeof(Texture2D))as Texture2D;
			Icon_ProbeLightsOff = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + Icon_ProbeLightsOff_file,typeof(Texture2D))as Texture2D;
			
			Icon_VertexPointLightsOn = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + Icon_VertexPointLightsOn_file,typeof(Texture2D))as Texture2D;
			Icon_VertexPointLightsOff = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + Icon_VertexPointLightsOff_file,typeof(Texture2D))as Texture2D;
			
			Icon_SpotPointLightsOn = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + Icon_SpotPointLightsOn_file,typeof(Texture2D))as Texture2D;
			Icon_SpotPointLightsOff = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + Icon_SpotPointLightsOff_file,typeof(Texture2D))as Texture2D;
			
			Icon_ShadowsOn = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + Icon_ShadowsOn_file,typeof(Texture2D))as Texture2D;
			Icon_ShadowsOff = AssetDatabase.LoadAssetAtPath("Assets/" + UNOShaderDirPath + "Bin/UI/" + Icon_ShadowsOff_file,typeof(Texture2D))as Texture2D;
		}
	}
	#endif
	static void bah()
	{
		string balalala = 
				Icon_DirLightOff_file + Icon_DirLightOn_file + 
				Icon_ProbeLightsOff_file + Icon_ProbeLightsOn_file +
				Icon_ShadowsOff_file + Icon_ShadowsOn_file +
				Icon_SpotPointLightsOff_file + Icon_SpotPointLightsOn_file +
				Icon_VertexPointLightsOff_file + Icon_VertexPointLightsOn_file +
				UNOShaderAnimation_file + 
				UNOShaderCheckBoxOff_file + UNOShaderCheckBoxOn_file +
				UNOShaderEye_file +
				UNOShaderFoldoutDown_file + UNOShaderFoldoutRight_file +
				UNOShaderHelpLink_file +
				UNOShaderWireframeOff_file + UNOShaderWireframeOn_file;
		Debug.Log ("bah to this variables"+ balalala);
	}
}