using UnityEngine;
using System.Collections;

namespace UTJ {

[RequireComponent(typeof(MeshFilter))]
public class Tube : MonoBehaviour
{
	const int THETA_STEP = 32;
	const int FORWARD_STEP = 64;
	const int POINT_MAX = (THETA_STEP+1) * FORWARD_STEP;
	const float REPEAT = 2f;
	public const float RADIUS = 16f;
	public const float RADIUS_SQR = RADIUS * RADIUS;
	const float LENGTH = 512;
	
	void Start()
	{
		RenderSettings.fogColor = new Color(0, 0, 0);

		var vertices = new Vector3[POINT_MAX];
	    {
			for (var fstep = 0; fstep < FORWARD_STEP; ++fstep) {
				for (var tstep = 0; tstep < THETA_STEP+1; ++tstep) {
					int i = fstep * (THETA_STEP+1) + tstep;
					vertices[i].x = Mathf.Cos((float)tstep/(float)THETA_STEP * Mathf.PI * 2f) * RADIUS;
					vertices[i].y = Mathf.Sin((float)tstep/(float)THETA_STEP * Mathf.PI * 2f) * RADIUS;
					vertices[i].z = (float)fstep/(float)FORWARD_STEP * LENGTH;
				}
			}
		}

		var triangles = new int[(FORWARD_STEP-1)*THETA_STEP * 6];
	    {
			int i = 0;
			for (int y = 0; y < FORWARD_STEP-1; ++y) {
				for (int x = 0; x < THETA_STEP; ++x) {
					triangles[i] = (y + 0) * (THETA_STEP+1) + x + 0; ++i;
					triangles[i] = (y + 1) * (THETA_STEP+1) + x + 0; ++i;
					triangles[i] = (y + 0) * (THETA_STEP+1) + x + 1; ++i;
					triangles[i] = (y + 1) * (THETA_STEP+1) + x + 0; ++i;
					triangles[i] = (y + 1) * (THETA_STEP+1) + x + 1; ++i;
					triangles[i] = (y + 0) * (THETA_STEP+1) + x + 1; ++i;
				}
			}
		}

		var uvs = new Vector2[POINT_MAX];
	    {
			int i = 0;
			for (int y = 0; y < FORWARD_STEP; ++y) {
				for (int x = 0; x < THETA_STEP+1; ++x) {
					uvs[i].x = (float)x/(float)THETA_STEP * REPEAT;
					uvs[i].y = (float)y/(float)FORWARD_STEP * REPEAT;
					++i;
				}
			}
		}

		var mesh = new Mesh ();
		mesh.name = "tube_unit";
		mesh.vertices = vertices;
		mesh.triangles = triangles;		
		mesh.uv = uvs;
		mesh.RecalculateNormals();
		mesh.RecalculateBounds();
		mesh.UploadMeshData(true /* markNoLogerReadable */);
		var mf = GetComponent<MeshFilter> ();
		mf.sharedMesh = mesh;
	}

	public static float GetRadiusSqr(float x, float y)
	{
		float phase = x == 0f ? 0f : Mathf.Atan(y / x); 
		phase += Mathf.PI * 0.125f;
		phase = Mathf.Repeat(phase, 2 * Mathf.PI);
		phase = phase * 4f / Mathf.PI;
		bool is_deep = ((int)phase) % 2 == 1;
		return is_deep ? Tube.RADIUS_SQR*4f : Tube.RADIUS_SQR;
	}

}

} // namespace UTJ {
