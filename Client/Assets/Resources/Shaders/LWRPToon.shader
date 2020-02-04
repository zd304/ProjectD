Shader "ZDStudio/ToonLWRP"
{
    Properties
    {
		[NoScaleOffset] _BaseTex("Texture", 2D) = "white" {}
		[NoScaleOffset] _Ramp("Ramp", 2D) = "white" {}
    }
    SubShader
	{
		Tags
		{
			"RenderPipeline" = "LightweightPipeline"
			"RenderType" = "Opaque"
			"Queue" = "Geometry+0"
		}

		Pass
		{
			Tags{"LightMode" = "LightweightForward"}

			Blend One Zero

			Cull Back

			ZTest LEqual

			ZWrite On

			HLSLPROGRAM
			#pragma vertex vert
			#pragma fragment frag

			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS
			#pragma multi_compile _ _MAIN_LIGHT_SHADOWS_CASCADE
			#pragma multi_compile _ _ADDITIONAL_LIGHTS_VERTEX _ADDITIONAL_LIGHTS
			#pragma multi_compile _ _ADDITIONAL_LIGHT_SHADOWS
			#pragma multi_compile _ _SHADOWS_SOFT
			#pragma multi_compile _ _MIXED_LIGHTING_SUBTRACTIVE
			
			#pragma multi_compile_instancing
			#pragma multi_compile_fog

			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.lightweight/Shaders/UnlitInput.hlsl"

			TEXTURE2D(_BaseTex); SAMPLER(sampler_BaseTex); float4 _BaseTex_TexelSize;
			TEXTURE2D(_Ramp); SAMPLER(sampler_Ramp); float4 _Ramp_TexelSize;

			struct appdata
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct v2f
			{
				float4 position : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 WorldSpaceNormal : TEXCOORD1;

				float4 shadowCoord		: TEXCOORD2;
				half4 fogFactorAndVertexLight : TEXCOORD3;

				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			v2f vert(appdata v)
			{
				v2f o = (v2f)0;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				o.position = TransformObjectToHClip(v.vertex.xyz);
				o.WorldSpaceNormal = normalize(mul(v.normal, (float3x3)UNITY_MATRIX_I_M));
				o.uv = v.uv;

				VertexPositionInputs vertexInput = GetVertexPositionInputs(v.vertex.xyz);

				#ifdef _MAIN_LIGHT_SHADOWS
					o.shadowCoord = GetShadowCoord(vertexInput);
				#endif

				return o;
			}

			half4 frag(v2f i) : SV_Target
			{
				UNITY_SETUP_INSTANCE_ID(i);

				Light l = GetMainLight(i.shadowCoord);

				half d = dot(i.WorldSpaceNormal, l.direction) * 0.5 + 0.5;
				half3 ramp = SAMPLE_TEXTURE2D(_Ramp, sampler_Ramp, float2(d, d)).rgb;
				half3 c = l.color * ramp;// *(atten * 2);

				half4 col = SAMPLE_TEXTURE2D(_BaseTex, sampler_BaseTex, i.uv.xy);
				col.rgb *= c;
				col.rgb = lerp(col.rgb * 0.5, col.rgb, l.distanceAttenuation * l.shadowAttenuation);
				return col;
			}
			ENDHLSL
		}
		Pass
		{
			Name "ShadowCaster"
			Tags{"LightMode" = "ShadowCaster"}

			ZWrite On ZTest LEqual

			Cull Back

			HLSLPROGRAM
			#pragma multi_compile_instancing

			#pragma vertex ShadowPassVertex
			#pragma fragment ShadowPassFragment

			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Core.hlsl"
			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/Lighting.hlsl"
			#include "Packages/com.unity.render-pipelines.lightweight/ShaderLibrary/ShaderGraphFunctions.hlsl"
			#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"

			struct GraphVertexInput
			{
				float4 vertex : POSITION;
				float3 normal : NORMAL;
				UNITY_VERTEX_INPUT_INSTANCE_ID
			};

			struct VertexOutput
			{
				float4 clipPos      : SV_POSITION;
				UNITY_VERTEX_INPUT_INSTANCE_ID
				UNITY_VERTEX_OUTPUT_STEREO
			};

			float4 _ShadowBias;
			float3 _LightDirection;

			VertexOutput ShadowPassVertex(GraphVertexInput v)
			{
				VertexOutput o;
				UNITY_SETUP_INSTANCE_ID(v);
				UNITY_TRANSFER_INSTANCE_ID(v, o);
				UNITY_INITIALIZE_VERTEX_OUTPUT_STEREO(o);

				float3 WorldSpacePosition = mul(UNITY_MATRIX_M, v.vertex).xyz;
				float3 ObjectSpacePosition = mul(UNITY_MATRIX_I_M, float4(WorldSpacePosition, 1.0)).xyz;

				v.vertex.xyz = ObjectSpacePosition;

				float3 positionWS = TransformObjectToWorld(v.vertex.xyz);
				float3 normalWS = TransformObjectToWorldNormal(v.normal);

				float invNdotL = 1.0 - saturate(dot(_LightDirection, normalWS));
				float scale = invNdotL * _ShadowBias.y;

				positionWS = normalWS * scale.xxx + positionWS;
				float4 clipPos = TransformWorldToHClip(positionWS);

				clipPos.z += _ShadowBias.x;

				#if UNITY_REVERSED_Z
					clipPos.z = min(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#else
					clipPos.z = max(clipPos.z, clipPos.w * UNITY_NEAR_CLIP_VALUE);
				#endif
				o.clipPos = clipPos;

				return o;
			}
			half4 ShadowPassFragment(VertexOutput IN) : SV_TARGET
			{
				UNITY_SETUP_INSTANCE_ID(IN);
				return 0;
			}

			ENDHLSL
		}
    }
}
