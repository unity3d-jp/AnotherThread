// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/hahen" {
    Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
    }
	SubShader {
		Tags { "RenderType" = "Opaque" }
		ZWrite On
		Cull Off
		// Blend SrcAlpha OneMinusSrcAlpha // alpha blending
		// Blend SrcAlpha One 				// alpha additive
		
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
 			#pragma target 3.0
 
 			#include "UnityCG.cginc"

            uniform sampler2D _MainTex;

 			struct appdata_custom {
				float4 vertex : POSITION;
				float4 normal : NORMAL;
				float4 texcoord : TEXCOORD0;
				float4 texcoord2 : TEXCOORD1;
				float4 texcoord3 : TEXCOORD2;
			};

 			struct v2f
 			{
 				float4 pos:SV_POSITION;
				float2 uv:TEXCOORD0;
 			};
 			
			float _CurrentTime;
   
            v2f vert(appdata_custom v)
            {
				float elapsed = (_CurrentTime - v.texcoord3.y);

            	float4 vec = v.vertex;
				float theta = elapsed * 16;
				// float3 n = cross(v.normal, float3(0, 0, 1));
				float3 n = v.normal;
				float3 rvec;
				/* rotate matrix for an arbitrary axis
				 * Vx*Vx*(1-cos) + cos	Vx*Vy*(1-cos) - Vz*sin	Vz*Vx*(1-cos) + Vy*sin;
				 * Vx*Vy*(1-cos) + Vz*sin	Vy*Vy*(1-cos) + cos	Vy*Vz*(1-cos) - Vx*sin;
				 * Vz*Vx*(1-cos) - Vy*sin	Vy*Vz*(1-cos) + Vx*sin	Vz*Vz*(1-cos) + cos;
				 */
				float s = sin(theta);
				float c = cos(theta);
				float nx1c = n.x*(1-c);
				float ny1c = n.y*(1-c);
				float nz1c = n.z*(1-c);
				float nxs = n.x*s;
				float nys = n.y*s;
				float nzs = n.z*s;
				rvec.x = ((n.x*nx1c + c) * vec.x +
						  (n.x*ny1c - nzs) * vec.y +
						  (n.z*nx1c + nys) * vec.z);
				rvec.y = ((n.x*ny1c + nzs) * vec.x +
						  (n.y*ny1c + c) * vec.y +
						  (n.y*nz1c - nxs) * vec.z);
				rvec.z = ((n.z*nx1c - nys) * vec.x +
						  (n.y*nz1c + nxs) * vec.y +
						  (n.z*nz1c + c) * vec.z);

				float4 tv0 = float4(rvec, 1);
				tv0.xyz += v.normal.xyz * (elapsed+0.1) * 2;
				tv0.xy += v.texcoord2.xy;
				tv0.z += v.texcoord3.x;
				tv0.z += elapsed*(-15);
            	tv0 = UnityObjectToClipPos(tv0);
            	
            	v2f o;
            	o.pos = tv0;
				o.uv = MultiplyUV(UNITY_MATRIX_TEXTURE0, v.texcoord);
            	return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
				return tex2D(_MainTex, i.uv);
            }

            ENDCG
        }
    }
}
