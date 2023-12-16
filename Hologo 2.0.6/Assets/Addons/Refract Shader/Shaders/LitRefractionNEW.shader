Shader "Refractive/LitRefractionNEW" {
	Properties {
		[Header(Color Params)]
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Color Texture", 2D) = "white" {}
		_Tint ("Color Tint", range(0,1)) = 0
		
		[Space]
		[Header(Refraction Masking)]
		_RefractFactor ("Refraction Blend Strength", range(0,1)) = 1
		[Toggle(_ALBEDO_ALPHA_REFRACT)] _Albedo_Alpha_Refract( "Albedo Alpha Refracts", Float ) = 0
		[Toggle(_ALBEDO_ALPHA_REFRACT_INVERT)] _Albedo_Alpha_Refract_invert( "Invert Refract Mask", Float ) = 0
		[Toggle(_ALBEDO_ALPHA_TRANSPARENT)] _Albedo_Alpha_Transparent( "Albedo Alpha Transparent", Float ) = 0
		
		[Space]
		[Header(Surface params)]
		_SmoothnessTex("Smoothness Texture", 2D) = "white" {}
		[Toggle(_INVERT_SMOOTHNESS)] _Invert_Smoothness("Invert Smoothness Texture", Float) = 0
		_Smoothness("Smoothness", Range(0,1)) = 0.6
		_Metallic ("Metallic", Range(0,1)) = 0.0
		_MetallicTex("Metallic Texture", 2D) = "black" {}
		[Toggle(_NORMAL_MAP)] _Normal_Map( "Enable Normal Mapping", Float ) = 0
		_NormalTex("Normal Texure", 2D) = "bump" {}
		_NormalFactor("Normal Strength", float) = 1
		
		[Space]
		[Header(Refraction Params)]
		_BlurSamples("Blur Samples", Range(1,128)) = 16
		_IOR("Index of Refraction (IOR)", float) = 1
		_CA("Chromatic Abberation (CA)", range(0,0.1)) = 0.1
		
	}
	SubShader {

		GrabPass{
		}

		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert alpha:fade

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0
		#include "refract.cginc"
		#pragma shader_feature _NORMAL_MAP
		#pragma shader_feature _INVERT_SMOOTHNESS
		#pragma shader_feature _ALBEDO_ALPHA_REFRACT
		#pragma shader_feature _ALBEDO_ALPHA_REFRACT_INVERT
		#pragma shader_feature _ALBEDO_ALPHA_TRANSPARENT

		sampler2D _MainTex;
		sampler2D _SmoothnessTex;
		sampler2D _GrabTexture;
		sampler2D _MetallicTex;
		#if defined(_NORMAL_MAP)
			sampler2D _NormalTex;
			float _NormalFactor;
		#endif
		float _Smoothness;
		int _BlurSamples;
		float _IOR;
		float _CA;
		float _RefractFactor;
		float _Tint;

		struct Input {
			float2 uv_MainTex;
			float2 uv_SmoothnessTex;
			float2 uv_MetallicTex;
			#if defined(_NORMAL_MAP)
				float2 uv_NormalTex;
			#endif
			float3 worldPos;
			float3 worldNormal; 
			INTERNAL_DATA
		};

		half _Glossiness;
		half _Metallic;
		fixed4 _Color;

		void vert(inout appdata_full v, out Input o){
			UNITY_INITIALIZE_OUTPUT(Input,o);
		}

		void surf (Input IN, inout SurfaceOutputStandard o) {
			fixed4 colorTex = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			fixed4 metallicTex = tex2D(_MetallicTex, IN.uv_MetallicTex);
			
			o.Alpha = 1.0;
			#if defined (_ALBEDO_ALPHA_TRANSPARENT )
			o.Alpha = colorTex.a;
			#endif

			clip(o.Alpha-0.01);
			o.Normal = float3(0,0,1);
			float refractionMask = 1.0;
			#if defined (_ALBEDO_ALPHA_REFRACT )
			refractionMask = 1.0-colorTex.a;
			#endif
			#if defined (_ALBEDO_ALPHA_REFRACT_INVERT )
			refractionMask = 1.0-refractionMask;
			#endif

			refractionMask *= _RefractFactor;

			#if defined(_NORMAL_MAP)
				o.Normal = normalize(UnpackNormal( tex2D(_NormalTex, IN.uv_NormalTex) ) );
				o.Normal = lerp( float3(0,0,1), o.Normal, _NormalFactor);
			#endif

			fixed smoothnessTex = tex2D(_SmoothnessTex, IN.uv_SmoothnessTex).r;
			#if defined (_INVERT_SMOOTHNESS)
			smoothnessTex = 1.0-smoothnessTex;
			#endif
			float ior = _IOR - 1;
			float3 worldViewDir = normalize( _WorldSpaceCameraPos - IN.worldPos.xyz );
			
			float roughness = 1.0 - _Smoothness * smoothnessTex;
			fixed3 bg = fixed3(0.0,0.0,0.0);
			bg = getRefracted( _GrabTexture, worldViewDir, WorldNormalVector (IN, o.Normal), IN.worldPos.xyz, roughness, ior, _CA, _BlurSamples);
			bg *= lerp( fixed3(1,1,1), colorTex.rgb, _Tint );
			o.Albedo = fixed4(lerp( colorTex.rgb, bg, refractionMask ),1.0);
			o.Smoothness = 1-roughness;
			o.Metallic = _Metallic * metallicTex.r;
		}
		ENDCG
	}
	FallBack "Diffuse"
}
