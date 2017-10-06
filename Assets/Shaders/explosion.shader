// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/explosion" {
    Properties {
		_MainTex ("Base (RGB)", 2D) = "white" {}
		//_InvFade ("Soft Particles Factor", Range(0.01,3.0)) = 1.0
    }
	SubShader {
   		Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
		ZWrite Off
		Cull Off
		Blend SrcAlpha One
		// Blend SrcAlpha OneMinusSrcAlpha // alpha blending
		
        Pass {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag
 			#pragma target 3.0
 			
 			#include "UnityCG.cginc"

            uniform sampler2D _MainTex;

 			struct appdata_custom {
				float4 vertex : POSITION;
				fixed4 color : COLOR;
				float4 texcoord : TEXCOORD0;
				float4 texcoord2 : TEXCOORD1;
			};

 			struct v2f {
 				float4 pos : SV_POSITION;
				float2 texcoord : TEXCOORD0;
 			};
 			
			float   _CurrentTime;
			float3   _CamUp;
   
            v2f vert(appdata_custom v)
            {
				float4 tv = v.vertex;

				float elapsed = _CurrentTime - v.texcoord2.x;
				float flow_z = 12;
				tv.z += -elapsed * flow_z;

				float size = 3;
				float3 up = _CamUp;
				float3 eye = normalize(ObjSpaceViewDir(tv));
				float3 side = cross(eye, up);

				// rotate
				float3 vec = ((v.texcoord.x-0.5f)*side + (v.texcoord.y-0.5f)*up) *size;
				float3 n = eye;
				float theta = v.texcoord2.y;
				/* rotate matrix for an arbitrary axis
				 * Vx*Vx*(1-cos) + cos  	Vx*Vy*(1-cos) - Vz*sin	Vz*Vx*(1-cos) + Vy*sin;
				 * Vx*Vy*(1-cos) + Vz*sin	Vy*Vy*(1-cos) + cos 	Vy*Vz*(1-cos) - Vx*sin;
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
				float3 rvec;
				rvec.x = ((n.x*nx1c + c) * vec.x +
						  (n.x*ny1c - nzs) * vec.y +
						  (n.z*nx1c + nys) * vec.z);
				rvec.y = ((n.x*ny1c + nzs) * vec.x +
						  (n.y*ny1c + c) * vec.y +
						  (n.y*nz1c - nxs) * vec.z);
				rvec.z = ((n.z*nx1c - nys) * vec.x +
						  (n.y*nz1c + nxs) * vec.y +
						  (n.z*nz1c + c) * vec.z);
				tv.xyz += rvec;


				float rW = 1.0/8.0;
				float rH = 1.0/8.0;
				float fps = 60;
				float loop0 = 1.0/(fps*rW*rH);
				elapsed = clamp(elapsed, 0, loop0);

				float texu = floor(elapsed * fps) * rW - floor(elapsed*fps*rW);
				float texv = 1 - floor(elapsed * fps * rW) * rH;
				texu += v.texcoord.x * rW;
				texv += -v.texcoord.y * rH;
				
            	v2f o;
			    o.pos = UnityObjectToClipPos(float4(tv.xyz,1));
				o.texcoord = MultiplyUV(UNITY_MATRIX_TEXTURE0,
										float4(texu, texv, 0, 0));
            	return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
				return tex2D(_MainTex, i.texcoord);
            }

            ENDCG
        }
    }
}
