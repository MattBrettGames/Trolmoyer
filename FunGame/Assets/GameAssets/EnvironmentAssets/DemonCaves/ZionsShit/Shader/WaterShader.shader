﻿Shader "Custom/WaterShader"
{
	Properties
	{
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Roughness("Roughness", Range(0,1)) = 0.5
		_NormalMap("Normal", 2D) = "bump" {}
		_NormalIntensity("NormalIntensity", float) = 1.0
			//_EmissionBool("Emissive", Range(0,1)) = 0
			_EmissionMap("Emission",2D) = "Black"{}
			_EmissionColor("Emission Color", Color) = (0,0,0,0)
			_Metallic("Metallic", Range(0,1)) = 0.0

			_Color2("Color2", Color) = (1, 1, 1, 1)
				// color of the edge effect
			_EdgeColor("Edge Color", Color) = (1, 1, 1, 1)
				// width of the edge effect
			_DepthFactor("Depth Factor", float) = 1.0

			_Height("Height", float) = 0.0
			_EffectOffset("Effect offset", float)=0.0
			_WaveAmp("Wave amp", float) = 0.0
			_WaveSpeed("Wave Speed",float) = 0.0
			_NoiseTex("Noise texture",2D) = "white" {}
	}
		SubShader
	{
		Tags {"Queue" = "Transparent" "RenderType" = "Transparent" }
		LOD 200

		CGPROGRAM
		// Physically based Standard lighting model, and enable shadows on all light types
		#pragma surface surf Standard fullforwardshadows vertex:vert

		// Use shader model 3.0 target, to get nicer looking lighting
		#pragma target 3.0

		sampler2D _MainTex;
	//sampler2D _Roughness;
		sampler2D _EmissionMap;
	struct Input
	{
		float2 uv_MainTex;
		float2 uv_NormalMap;
		float2 uv_RoughnessMap;
		float2 uv_EmissionMap;
	};

	float _NormalIntensity;
	half _Roughness;
	sampler2D _NormalMap;
	half _Metallic;
	fixed4 _Color;
	fixed4 _EmissionColor;

	// Add instancing support for this shader. You need to check 'Enable Instancing' on materials that use the shader.
	// See https://docs.unity3d.com/Manual/GPUInstancing.html for more information about instancing.
	// #pragma instancing_options assumeuniformscaling
	UNITY_INSTANCING_BUFFER_START(Props)
		// put more per-instance properties here
		UNITY_INSTANCING_BUFFER_END(Props)

	float _Height;
	float _WaveSpeed;
	float _WaveAmp;
	sampler2D _NoiseTex;
		void vert(inout appdata_full v)
	{

			// apply wave animation
			float noiseSample = tex2Dlod(_NoiseTex, float4(v.texcoord.xy, 0, 0));
			v.vertex.y = sin(_Time * _WaveSpeed * noiseSample) * _WaveAmp + _Height;
			v.vertex.x += cos(_Time * _WaveSpeed * noiseSample) * _WaveAmp;
			//v.vertex.y += _Height;
		}


		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			//displacement





			// Albedo comes from a texture tinted by color
			fixed4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = c.a;
			// Metallic and smoothness come from slider variables
			o.Metallic = _Metallic;
			o.Smoothness = _Roughness;
			o.Emission = tex2D(_EmissionMap, IN.uv_EmissionMap).rgb * _EmissionColor.rgb;;
			o.Normal = UnpackScaleNormal(tex2D(_NormalMap, IN.uv_NormalMap) ,_NormalIntensity);
			//o.Alpha = c.a;
		}
		ENDCG
			Pass
		{
		Blend SrcAlpha OneMinusSrcAlpha
		ZWrite off
		CGPROGRAM
			// required to use ComputeScreenPos()
			#include "UnityCG.cginc"


			#pragma vertex vert
			#pragma fragment frag
			#pragma glsl
			#pragma target 3.0

			 // Unity built-in - NOT required in Properties
			 sampler2D _CameraDepthTexture;


			struct vertexInput
			 {
			   float4 vertex : POSITION;
			   float4 texCoord : TEXCOORD1;
			 };

			struct vertexOutput
			 {
				float4 pos : SV_POSITION;
				float4 texCoord : TEXCOORD0;
				float4 screenPos : TEXCOORD1;
			 };

			float _Height;
			float _WaveSpeed;
			float _WaveAmp;
			float _EffectOffset;
			sampler2D _NoiseTex;
			vertexOutput vert(vertexInput input)
			  {
				vertexOutput output;

				// convert to camera clip space
				output.pos = UnityObjectToClipPos(input.vertex);

				// apply wave animation



				// convert obj-space position to camera clip space
				output.pos = UnityObjectToClipPos(input.vertex);
				float noiseSample = tex2Dlod(_NoiseTex, float4(input.texCoord.xy, 0, 0));
				output.pos.y -= (sin(_Time * _WaveSpeed * noiseSample) * _WaveAmp * _EffectOffset);
				output.pos.x += cos(_Time * _WaveSpeed * noiseSample) * _WaveAmp;

				// compute depth (screenPos is a float4)
				output.screenPos = ComputeScreenPos(output.pos);
				//output.pos.y += _Height;
				output.texCoord = input.texCoord;

				return output;
			  }
			sampler2D _MainTex;
			fixed4 _Color2;
			fixed4 _EdgeColor;
			float _DepthFactor;

			float4 frag(vertexOutput input) : COLOR
			{

			  float4 depthSample = SAMPLE_DEPTH_TEXTURE_PROJ(_CameraDepthTexture, input.screenPos);
			  float depth = LinearEyeDepth(depthSample).r;

			  // apply the DepthFactor to be able to tune at what depth values
			  // the foam line actually starts
			  float foamLine = 1 - saturate(_DepthFactor * (depth - input.screenPos.w));

			  // multiply the edge color by the foam factor to get the edge,
			  // then add that to the color of the water
			  float4 col = _Color2 + (foamLine * _EdgeColor);

			  if (foamLine < .03)
			  {
				  col.a = 0;
			  }
			  // else return _MainTex.albedo;
				return col;

			  }

				  ENDCG
			  }
	}



		FallBack "Diffuse"
}