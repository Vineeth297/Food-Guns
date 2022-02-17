Shader "Shader Graphs/TextureMover" {
	Properties {
		_Smoothness ("Smoothness", Float) = 0
		[NoScaleOffset] _BaseMap ("BaseMap", 2D) = "white" {}
		Color_2563832a8664414b9bb03a17163151bf ("BaseColor", Vector) = (1,1,1,1)
		[NoScaleOffset] _EmissionMap ("EmissionMap", 2D) = "white" {}
		[HDR] _EmissionColor ("EmissionColor", Vector) = (1,1,1,1)
		_Speed ("Speed", Vector) = (0,1,0,0)
		[ToggleUI] Boolean_43027bdee0c74d4eafe6f92089f9f809 ("UseEmission", Float) = 0
		[HideInInspector] [NoScaleOffset] unity_Lightmaps ("unity_Lightmaps", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_LightmapsInd ("unity_LightmapsInd", 2DArray) = "" {}
		[HideInInspector] [NoScaleOffset] unity_ShadowMasks ("unity_ShadowMasks", 2DArray) = "" {}
	}
	
	//DummyShaderTextExporter
	SubShader{
		Tags { "RenderType" = "Opaque" }
		LOD 200
		CGPROGRAM
#pragma surface surf Standard fullforwardshadows
#pragma target 3.0
		sampler2D _MainTex;
		struct Input
		{
			float2 uv_MainTex;
		};
		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex);
			o.Albedo = c.rgb;
		}
		ENDCG
	}
	Fallback "Hidden/Shader Graph/FallbackError"
	//CustomEditor "ShaderGraph.PBRMasterGUI"
}