using UnityEngine;
using System.Collections;

namespace UTJ {

public class Hahen
{
	// singleton
	static Hahen instance_;
	public static Hahen Instance { get { return instance_ ?? (instance_ = new Hahen()); } }

	const int HAHEN_MAX = 32;
	const int PIECE_NUM = 8;

	private Vector3[] positions_;
	private float[] time_list_;

	private Vector2[][] uv2s_;
	private Vector2[][] uv3s_;
	private int spawn_index_;
	private Mesh mesh_;
	private Material material_;
	static readonly int material_CurrentTime = Shader.PropertyToID("_CurrentTime");

	public Mesh getMesh() { return mesh_; }
	public Material getMaterial() { return material_; }

	public void init(Material material)
	{
		positions_ = new Vector3[HAHEN_MAX];
		for (var i = 0; i < positions_.Length; ++i) {
			positions_[i] = new Vector3(0f, 0f, -99999f);
		}
		time_list_ = new float[HAHEN_MAX];

		uv2s_ = new Vector2[2][] { new Vector2[HAHEN_MAX*PIECE_NUM*6], new Vector2[HAHEN_MAX*PIECE_NUM*6], };
		uv3s_ = new Vector2[2][] { new Vector2[HAHEN_MAX*PIECE_NUM*6], new Vector2[HAHEN_MAX*PIECE_NUM*6], };

		var vertices = new Vector3[HAHEN_MAX*PIECE_NUM*6];
		float scale = 0.25f;
		for (var i = 0; i < HAHEN_MAX*PIECE_NUM; ++i) {
			vertices[i*6+0] = new Vector3(-0.25f,  -1f,  0.0f) * scale;
			vertices[i*6+1] = new Vector3( 0.25f,  -1f,  0.5f) * scale;
			vertices[i*6+2] = new Vector3(-0.25f,   0f, -0.5f) * scale;
			vertices[i*6+3] = new Vector3( 0.25f,   0f, -0.5f) * scale;
			vertices[i*6+4] = new Vector3(-0.25f,0.25f,  0.5f) * scale;
			vertices[i*6+5] = new Vector3( 0.25f,0.25f,  0.0f) * scale;
		}

		var normals = new Vector3[HAHEN_MAX*PIECE_NUM*6];
		for (var i = 0; i < HAHEN_MAX*PIECE_NUM; ++i) {
			float x = Random.Range(-1f, 1f);
			float y = Random.Range(-1f, 1f);
			float z = Random.Range(-1f, 1f);
			float len2 = x*x + y*y + z*z;
			float len = Mathf.Sqrt(len2);
			float rlen = 1.0f/len;
			var point = new Vector3(x*rlen, y*rlen, z*rlen);
			normals[i*6+0] = point;
			normals[i*6+1] = point;
			normals[i*6+2] = point;
			normals[i*6+3] = point;
			normals[i*6+4] = point;
			normals[i*6+5] = point;
		}

		var triangles = new int[HAHEN_MAX*PIECE_NUM*12];
		for (var i = 0; i < HAHEN_MAX*PIECE_NUM; ++i) {
			triangles[i*12+ 0] = i*6+0;
			triangles[i*12+ 1] = i*6+1;
			triangles[i*12+ 2] = i*6+2;
			triangles[i*12+ 3] = i*6+2;
			triangles[i*12+ 4] = i*6+1;
			triangles[i*12+ 5] = i*6+3;
			triangles[i*12+ 6] = i*6+2;
			triangles[i*12+ 7] = i*6+3;
			triangles[i*12+ 8] = i*6+4;
			triangles[i*12+ 9] = i*6+4;
			triangles[i*12+10] = i*6+3;
			triangles[i*12+11] = i*6+5;
		}

		var uvs = new Vector2[HAHEN_MAX*PIECE_NUM*6];
		for (var i = 0; i < HAHEN_MAX*PIECE_NUM; ++i) {
			uvs[i*6+0] = new Vector2(0f, 0f);
			uvs[i*6+1] = new Vector2(1f, 0f);
			uvs[i*6+2] = new Vector2(0f, 0.5f);
			uvs[i*6+3] = new Vector2(1f, 0.5f);
			uvs[i*6+4] = new Vector2(0f, 1f);
			uvs[i*6+5] = new Vector2(1f, 1f);
		}

		mesh_ = new Mesh();
		mesh_.MarkDynamic();
		mesh_.name = "hahen";
		mesh_.vertices = vertices;
		mesh_.normals = normals;
		mesh_.triangles = triangles;
		mesh_.uv = uvs;
		mesh_.uv2 = uv2s_[0];
		mesh_.uv3 = uv3s_[0];
		mesh_.bounds = new Bounds(Vector3.zero, Vector3.one * 99999999);
		material_ = material;

		spawn_index_ = 0;
	}

	public void begin()
	{
	}

	public void end(int front)
	{
		for (var i = 0; i < HAHEN_MAX; ++i) {
			int idx = i*PIECE_NUM*6;
			for (var j = idx; j < idx + PIECE_NUM*6; ++j) {
				uv2s_[front][j].x = positions_[i].x;
				uv2s_[front][j].y = positions_[i].y;
				uv3s_[front][j].x = positions_[i].z;
				uv3s_[front][j].y = time_list_[i];
			}
		}
	}

	public void render(int front, float render_time)
	{
		mesh_.uv2 = uv2s_[front];
		mesh_.uv3 = uv3s_[front];
		material_.SetFloat(material_CurrentTime, render_time);
	}

	public void spawn(ref Vector3 pos, float update_time)
	{
		int id = spawn_index_;
		++spawn_index_;
		if (spawn_index_ >= HAHEN_MAX) {
			spawn_index_ = 0;
		}

		positions_[id] = pos;
		time_list_[id] = update_time;
	}

}

} // namespace UTJ {
