// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/sight" {
    Properties {
    }
	SubShader {
		// Tags { "RenderType" = "Opaque" }
		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		ZWrite Off
		ZTest Always
		Cull Off
		// Blend SrcAlpha One
		Blend SrcAlpha OneMinusSrcAlpha // alpha blending
		
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
 			#pragma target 3.0
 			
 			#include "UnityCG.cginc"

 			struct appdata_custom {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
			};

 			struct v2f {
 				float4 pos : SV_POSITION;
 				fixed4 color:COLOR;
 			};
 			
            v2f vert(appdata_custom v)
            {
            	v2f o;
			    o.pos = UnityObjectToClipPos(float4(v.vertex.xyz,1));
				o.color = v.color;
            	return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
				return i.color;
            }

            ENDCG
        }
    }
}
