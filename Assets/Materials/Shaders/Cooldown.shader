Shader "Cooldown"
{
	Properties
	{
		[PerRendererData]_MainTex ("Texture", 2D) = "white" {}
		_CooldownPercentage ("Cooldown Percentage", Range(0.0, 1.0)) = 0.6
	}
	SubShader
	{
		Blend One OneMinusSrcAlpha
		Pass
		{
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			
			#include "UnityCG.cginc"

			struct appdata
			{
				float4 vertex   : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				float2 texcoord  : TEXCOORD0;
			};

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			float4 _MainTex_ST;
			fixed _CooldownPercentage;
			
			v2f vert (appdata IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				return OUT;
			}

			fixed4 frag (v2f i) : SV_Target
			{
				const float PI = 3.14159;
				fixed2 vec = normalize(i.texcoord - fixed2(0.5, 0.5));
				float ang1 = acos(vec.x);
				if(vec.y < 0)
					ang1 = PI * 2 - ang1;
				ang1 -= PI / 2;
				if(ang1 < 0)
					ang1 += PI * 2;
				float ang2 = _CooldownPercentage * PI * 2;
				fixed4 c = tex2D(_MainTex, i.texcoord);
				c.rgb *= c.a;
				if(ang1 < ang2)
				{
					c.rgb *= 0.2;
				}
				return c;
			}
			ENDCG
		}
	}
}
