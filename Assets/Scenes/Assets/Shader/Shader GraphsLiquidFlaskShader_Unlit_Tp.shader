Shader "Shader Graphs/LiquidFlaskShader_Unlit_Tp" {
	Properties {
		_Fill ("Fill", Range(0, 1)) = 0.8
		_Offset ("Offset", Vector) = (0,0,0,0)
		_FillMultiplier ("FillMultiplier", Float) = 2
		_LiquidColor ("LiquidColor", Vector) = (0.262,0.07053844,0,1)
		_SurfaceColor ("SurfaceColor", Vector) = (0.34,0.111812,0.004563768,1)
		_FresnelPower ("FresnelPower", Float) = 1
		_FresnelColor ("FresnelColor", Vector) = (0.291,0.1077778,0,1)
		_WobbleX ("WobbleX", Range(-1, 1)) = 0
		_WobbleZ ("WobbleZ", Range(-1, 1)) = 0
		_RefractionScale ("RefractionScale", Float) = 0.2
		_RefractionSpeed ("RefractionSpeed", Float) = 0.1
		_RefractionStrength ("RefractionStrength", Float) = 0.005
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
}