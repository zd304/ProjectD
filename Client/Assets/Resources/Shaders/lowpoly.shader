Shader "ZDStudio/Lowpoly"
{
    Properties
    {
		_MainTex("Albedo (RGB)", 2D) = "white" {}
		_Color("Color", Color) = (1,1,1,1)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_fog

            #include "UnityCG.cginc"
			#include "UnityLightingCommon.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
				float3 normal : NORMAL;
				float2 uv : TEXCOORD0;
            };

            struct v2f
            {
				float4 vertex : SV_POSITION;
				float3 normal : NORMAL;
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
            };

            sampler2D _MainTex;
			fixed4 _Color;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
				o.normal = mul(float4(v.normal, 1), unity_WorldToObject).xyz;
				o.uv = v.uv;
                UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 texColor = tex2D(_MainTex, i.uv);
				// 法向量标准化
				float3 normal = normalize(i.normal);
				// 获取平行光源方向并标准化
				float3 lightDir = normalize(_WorldSpaceLightPos0.xyz);
				// 计算漫反射
				fixed3 diffuseColor = texColor * _Color * max(0, dot(normal, lightDir)) * _LightColor0.rgb;
                // apply fog
                UNITY_APPLY_FOG(i.fogCoord, col);
                return fixed4(diffuseColor * 1.1f + UNITY_LIGHTMODEL_AMBIENT.xyz, 1.0);
            }
            ENDCG
        }
    }
}
