// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/simple" {
    Properties {
		_Color ("Main Color", Color) = (1,1,1,1)
    }
	SubShader {
		Tags { "RenderType" = "Opaque" }
		
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
 			#pragma target 3.0
 			
 			#include "UnityCG.cginc"

			uniform float4 _Color;

 			struct appdata_custom {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

 			struct v2f {
 				float4 pos:SV_POSITION;
 				fixed4 color:COLOR;
 			};
 			
            v2f vert(appdata_custom v)
            {
				v2f o;
			    o.pos = UnityObjectToClipPos(v.vertex);
            	o.color = _Color;
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
