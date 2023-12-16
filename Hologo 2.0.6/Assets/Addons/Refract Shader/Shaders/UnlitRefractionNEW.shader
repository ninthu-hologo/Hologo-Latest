Shader "Refractive/UnlitRefractionNEW"
{
	Properties
	{
		[Header(Color Params)]
		_Color ("Color", Color) = (1,1,1,1)
		_MainTex ("Color Texture", 2D) = "white" {}
		_Tint ("Tint", range(0,1)) = 0
		[Space]
		[Header(Refraction Masking)]
		_RefractFactor ("Refraction Blend Strength", range(0,1)) = 1
		[Toggle(_ALBEDO_ALPHA_REFRACT)] _Albedo_Alpha_Refract( "Albedo Alpha Refracts", Float ) = 0
		[Toggle(_ALBEDO_ALPHA_REFRACT_INVERT)] _Albedo_Alpha_Refract_invert( "Invert Refract Mask", Float ) = 0
		[Toggle(_ALBEDO_ALPHA_TRANSPARENT)] _Albedo_Alpha_Transparent( "Albedo Alpha Transparent", Float ) = 0
		[Space]
		[Header(Smoothness)]
		_SmoothnessTex("Smoothness Texture", 2D) = "white" {}
		_Smoothness("Smoothness", Range(0,1)) = 0.6
		[Space]
		[Header(Refraction Params)]
		_BlurSamples("Blur Samples", Range(1,128)) = 16
		_IOR("Index of Refraction (IOR)", float) = 1
		_CA("Chromatic Abberation (CA)", range(0,0.1)) = 0.1
		
	}
	SubShader
	{
		Tags { "RenderType"="Transparent" "Queue"="Transparent" }
		blend SrcAlpha OneMinusSrcAlpha
		LOD 100

		GrabPass{
		}

		Pass{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			#include "refract.cginc"
			#pragma shader_feature _INVERT_SMOOTHNESS
			#pragma shader_feature _ALBEDO_ALPHA_REFRACT
			#pragma shader_feature _ALBEDO_ALPHA_REFRACT_INVERT
			#pragma shader_feature _ALBEDO_ALPHA_TRANSPARENT

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uvColor : TEXCOORD0;
				float2 uvSmoothness : TEXCOORD1;
				float4 vertex : SV_POSITION;
				float4 screenPos : TEXCOORD2;
				float2 screenNormal : TEXCOORD3;
				float4 worldPos : TEXCOORD4;
				float3 worldNormal : TEXCOORD5;
			};
			fixed4 _Color;
			sampler2D _GrabTexture;
			sampler2D _MainTex;
			sampler2D _SmoothnessTex;
			float4 _MainTex_ST;
			float4 _SmoothnessTex_ST;
			float _Smoothness;
			int _BlurSamples;
			float _IOR;
			float _CA;
			float _Tint;
			float _RefractFactor;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uvColor = TRANSFORM_TEX(v.uv, _MainTex);
				o.uvSmoothness = TRANSFORM_TEX(v.uv, _SmoothnessTex);
				o.screenPos = ComputeGrabScreenPos(o.vertex);
				o.worldNormal = UnityObjectToWorldNormal(v.normal);
				o.worldPos = mul( unity_ObjectToWorld, v.vertex );
				return o;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				
				fixed4 colorTex = tex2D(_MainTex, i.uvColor);
				fixed4 color = colorTex * _Color;
				float alpha = 1.0;
				float refractionMask = 1.0;

				#if defined (_ALBEDO_ALPHA_TRANSPARENT )
					alpha = colorTex.a;
				#endif
				#if defined (_ALBEDO_ALPHA_REFRACT )
					refractionMask = 1.0-colorTex.a;
				#endif
				#if defined (_ALBEDO_ALPHA_REFRACT_INVERT )
					refractionMask = 1.0-refractionMask;
				#endif

				refractionMask *= _RefractFactor;

				float ior = _IOR - 1;
				float3 worldViewDir = normalize( _WorldSpaceCameraPos - i.worldPos.xyz );
				float roughness = 1.0 - _Smoothness * tex2D(_SmoothnessTex, i.uvSmoothness).r;
				fixed3 bg = fixed3(0.0,0.0,0.0);
				bg = getRefracted( _GrabTexture, worldViewDir, i.worldNormal, i.worldPos.xyz, roughness, ior, _CA, _BlurSamples);
				bg *= lerp( fixed3(1,1,1), color.rgb, _Tint );
				color.rgb = lerp(color.rgb, bg, refractionMask);
				color.a = alpha;
				return color;
				
			}
			ENDCG
		}
	}
}
