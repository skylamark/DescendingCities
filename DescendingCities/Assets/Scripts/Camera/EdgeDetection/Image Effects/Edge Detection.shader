
Shader "Custom/EdgeDetection"
{
	Properties
	{
		[HideInInspector] _MainTex("Base (RGB)", 2D) = "white" {}

		[Header(General)][KeywordEnum(Depth, Normal, Both)] _Mode("Edge detection mode", Range(0, 2)) = 0
		[IntRange] _EdgeDistance("Edge distance", Range(0, 25)) = 2
		[Toggle] _EdgesOnly("Edges only", int) = 0

		[Header(Normal variables)] _NormalBias("Normal bias", Range(-5, 5)) = 0.7
		[Header(Depth variables)] _DepthBias("Depth bias", Range(-5, 5)) = 0.25
		_DepthSensitivity("Depth sensitivity", float) = 3000
	}
	Subshader
	{

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			#pragma target 4.0
			
			#include "UnityCG.cginc"

			uniform sampler2D _MainTex;
			uniform int _Mode;
			uniform sampler2D _CameraDepthNormalsTexture;
			uniform float _EdgeDistance;
			uniform float _DepthBias;
			uniform float _NormalBias;
			uniform float _DepthSensitivity;
			uniform int _EdgesOnly;

			float4 depthDifference(float2 uv, int width, float2 direction)
			{
				float thisDepth = Linear01Depth(tex2D(_CameraDepthNormalsTexture, uv));

				float edge = 1;
				for (int i = 0; i < width; i++)
				{
					//Detect depth difference
					float temp = 1;
					float other = Linear01Depth(tex2D(_CameraDepthNormalsTexture, uv + direction * i));
					temp = 1 - distance(thisDepth, other) * _DepthSensitivity;

					if (edge > temp)
						edge = temp;
				}

				if (edge > _DepthBias)
					edge = 1;
				return edge;
			}

			float4 normalDifference(float2 uv, int width, float2 direction)
			{
				float depth;
				float3 normal;
				DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, uv), depth, normal);

				float edge = 1;
				for (int i = 0; i < width; i++)
				{
					float otherDepth;
					float3 otherNormal;
					DecodeDepthNormal(tex2D(_CameraDepthNormalsTexture, uv + direction * i), otherDepth, otherNormal);

					float temp = 1 - distance(normal, otherNormal);

					if (edge > temp)
						edge = temp;
				}

				if (edge > _NormalBias)
					edge = 1;
				else
					edge = 0;
				return edge;
			}

			float4 frag(v2f_img i) : COLOR
			{
				float width = _EdgeDistance;

				float pixelsX = 1 / _ScreenParams.x;
				float pixelsY = 1 / _ScreenParams.y;

				float4 c = tex2D(_MainTex, i.uv);

				float left, right, up, down;
				if (_Mode == 0 || _Mode == 2)
				{
					left = depthDifference(i.uv, width, float2(-pixelsX, 0));
					right = depthDifference(i.uv, width, float2(+pixelsX, 0));
					up = depthDifference(i.uv, width, float2(0, -pixelsY));
					down = depthDifference(i.uv, width, float2(0, +pixelsY));
				}
				if (_Mode == 2)
				{
					left *= normalDifference(i.uv, width, float2(-pixelsX, 0));
					right *= normalDifference(i.uv, width, float2(+pixelsX, 0));
					up *= normalDifference(i.uv, width, float2(0, -pixelsY));
					down *= normalDifference(i.uv, width, float2(0, +pixelsY));
				}
				else if (_Mode == 1)
				{
					left = normalDifference(i.uv, width, float2(-pixelsX, 0));
					right = normalDifference(i.uv, width, float2(+pixelsX, 0));
					up = normalDifference(i.uv, width, float2(0, -pixelsY));
					down = normalDifference(i.uv, width, float2(0, +pixelsY));
				}
				
				float av = (left + right + up + down) / 4;

				if (_EdgesOnly)
					return av;
				return av *c;
			}
			ENDCG
		}
	}
}
