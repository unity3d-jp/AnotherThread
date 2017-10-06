// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/aura" {
    Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
		_MainTex ("Base (RGB)", 2D) = "white" {}
    }
	SubShader {
   		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		ZWrite Off
		Blend SrcAlpha One 				// alpha additive
		
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
 			#pragma target 3.0
 			
 			#include "UnityCG.cginc"

			uniform float4 _Color;
            uniform sampler2D _MainTex;
			uniform float4 _MainTex_ST;

 			struct appdata_custom {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float2 texcoord : TEXCOORD0;
			};

 			struct v2f {
 				float4 pos:SV_POSITION;
 				fixed4 color:COLOR;
				float2 uv:TEXCOORD0;
 			};
 			
            v2f vert(appdata_custom v)
            {
				v2f o;
			    o.pos = UnityObjectToClipPos(v.vertex);
            	// o.color = fixed4(_Color.rgb, _Color.a * v.color.a);
            	o.color = _Color;
				float2 tex = v.texcoord * _MainTex_ST.xy + _MainTex_ST.zw;
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, tex);
            	return o;
			}

            fixed4 frag(v2f i) : SV_Target
            {
				return tex2D(_MainTex, i.uv) * i.color;
            }

            ENDCG
        }
    }
}
