// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/zako"
{
    Properties {
		_MainTex ("Texture", 2D) = "white" {}
		_FogColor ("Fog Color", Color) = (0, 0, 0, 1.0)
	}
    SubShader {
		Tags { "RenderType" = "Opaque" }

		CGPROGRAM
#pragma surface surf Lambert finalcolor:mycolor vertex:myvert


		struct Input {
			float2 uv_MainTex;
			half fog;
		};

		sampler2D _MainTex;
		fixed4 _FogColor;

		void myvert (inout appdata_full v, out Input data)
		{
			UNITY_INITIALIZE_OUTPUT(Input,data);
			float4 hpos = UnityObjectToClipPos (v.vertex);
			float cameraVertDist = length(mul(UNITY_MATRIX_MV, v.vertex).xyz);
			float fog_start = 10;
			float fog_end = 300;
 			data.fog = saturate((fog_start - cameraVertDist) / (fog_start - fog_end));
		}
		void mycolor (Input IN, SurfaceOutput o, inout fixed4 color)
		{
			fixed3 fogColor = _FogColor.rgb;
			#ifdef UNITY_PASS_FORWARDADD
				fogColor = 0;
			#endif
			color.rgb = lerp (color.rgb, fogColor, IN.fog);
		}
		void surf (Input IN, inout SurfaceOutput o) {
			o.Albedo = tex2D (_MainTex, IN.uv_MainTex).rgb;
		}
		ENDCG
	} 
    Fallback "Diffuse"
}
