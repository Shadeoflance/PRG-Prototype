Shader "HP Bar"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_HealthPercentage ("Health Percentage", Range(0.0, 1.0)) = 0.5
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
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex : POSITION;
				float2 uv : TEXCOORD0;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float4 vertex : SV_POSITION;
			};

			sampler2D _MainTex;
			float4 _MainTex_ST;
			fixed _HealthPercentage;
			
			v2f vert (appdata v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}
			
			fixed4 frag (v2f i) : SV_Target
			{
				fixed4 col = fixed4(1, 0, 0, 1);
				fixed border = 0.03;
				if(i.uv.x > _HealthPercentage ||
					i.uv.x > 1 - border || i.uv.x < border ||
					i.uv.y > 1 - border * 8 || i.uv.y < border * 8)
				{
					col = fixed4(0.3, 0.3, 0.3, 1);
				}
				return col;
			}
			ENDCG
		}
	}
}
