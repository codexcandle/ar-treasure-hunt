Shader "CodeByCandle/FadeShader"
{
	Properties
	{
		_Alpha("Alpha", Range(0.0,1.0)) = 0.0
		_Color("Color", Color) = (1,1,1,1)
		_MainTex("Albedo (RGB)", 2D) = "white" {}
	}
	SubShader
	{
		// Transparent (ordering)
		Tags
		{
			"Queue" = "Transparent"
			"IgnoreProjector" = "True"
			"RenderType" = "Transparent"
		}
		// Removes back geometry
		// Cull Off

		CGPROGRAM
		// Alpha blending
		#pragma surface surf Standard alpha:fade

		float _Alpha;
		fixed4 _Color;
		sampler2D _MainTex;

		struct Input
		{
			float2 uv_MainTex;
		};

		void surf(Input IN, inout SurfaceOutputStandard o)
		{
			float4 c = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			o.Albedo = c.rgb;
			o.Alpha = _Alpha;
		}
		ENDCG
	}

	FallBack "Diffuse"
}