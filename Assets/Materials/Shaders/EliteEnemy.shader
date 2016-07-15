Shader "EliteEnemy"
{
	Properties
	{
		[PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		[MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
		_Radius("Radius", Range(0.0, 1.0)) = 0.05
		_Dist("Distance", Range(0.0, 1.0)) = 0.2
	}

	SubShader
	{
		Tags
		{ 
			"Queue"="Overlay" 
			"IgnoreProjector"="True" 
			"RenderType"="Transparent" 
			"PreviewType"="Plane"
			"CanUseSpriteAtlas"="True"
		}

		Cull Off
		Lighting Off
		ZWrite Off
		Blend One OneMinusSrcAlpha

		Pass
		{
		CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma multi_compile _ PIXELSNAP_ON
			#pragma shader_feature ETC1_EXTERNAL_ALPHA
			#include "UnityCG.cginc"
			
			struct appdata_t
			{
				float4 vertex   : POSITION;
				float4 color    : COLOR;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f
			{
				float4 vertex   : SV_POSITION;
				fixed4 color    : COLOR;
				float2 texcoord  : TEXCOORD0;
			};
			

			v2f vert(appdata_t IN)
			{
				v2f OUT;
				OUT.vertex = mul(UNITY_MATRIX_MVP, IN.vertex);
				OUT.texcoord = IN.texcoord;
				OUT.color = IN.color;
				#ifdef PIXELSNAP_ON
				OUT.vertex = UnityPixelSnap (OUT.vertex);
				#endif

				return OUT;
			}

			sampler2D _MainTex;
			sampler2D _AlphaTex;
			fixed _Radius;
			fixed _Dist;
			fixed4 _Color;


			fixed4 SampleSpriteTexture (float2 uv)
			{
				fixed4 color = tex2D (_MainTex, uv);
				return color;
			}

			fixed4 frag(v2f IN) : SV_Target
			{
				fixed4 c = SampleSpriteTexture (IN.texcoord) * IN.color;
				c.rgb *= c.a;
				fixed2 i = fixed2(IN.texcoord.x / _Dist, IN.texcoord.y / _Dist);
				fixed2 v1 = fixed2(floor(i.x) * _Dist, floor(i.y) * _Dist) + fixed2(_Dist / 2, _Dist / 2);
				fixed2 v2 = fixed2(ceil(i.x) * _Dist, ceil(i.y) * _Dist) + fixed2(_Dist / 2, _Dist / 2);
				fixed2 v3 = fixed2(floor(i.x) * _Dist, ceil(i.y) * _Dist) + fixed2(_Dist / 2, _Dist / 2);
				fixed2 v4 = fixed2(ceil(i.x) * _Dist, floor(i.y) * _Dist) + fixed2(_Dist / 2, _Dist / 2);
				fixed t = min(min(distance(v1, IN.texcoord), distance(v2, IN.texcoord)),
					min(distance(v3, IN.texcoord), distance(v4, IN.texcoord)));
				if (t < _Radius)
				{
					c = _Color * c.a;
				}
				return c;
			}
		ENDCG
		}
	}
}
