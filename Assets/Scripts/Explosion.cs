using UnityEngine;
using System.Collections;

namespace UTJ {

public class Explosion
{
	// singleton
	static Explosion instance_;
	public static Explosion Instance { get { return instance_ ?? (instance_ = new Explosion()); } }

	const int EXPLOSION_MAX = 32;

	private Vector3[] positions_;
	private Vector2[] uv2_list_;

	private Vector3[][] vertices_;
	private Vector2[][] uv2s_;
	private int spawn_index_;
	private Mesh mesh_;
	private Material material_;
	static readonly int material_CamUp = Shader.PropertyToID("_CamUp");
	static readonly int material_CurrentTime = Shader.PropertyToID("_CurrentTime");

	public Mesh getMesh() { return mesh_; }
	public Material getMaterial() { return material_; }

	public void init(Material material)
	{
		positions_ = new Vector3[EXPLOSION_MAX];
		for (var i = 0; i < positions_.Length; ++i) {
			positions_[i].z = -99999f;
		}
		uv2_list_ = new Vector2[EXPLOSION_MAX];

		vertices_ = new Vector3[2][] { new Vector3[EXPLOSION_MAX*4], new Vector3[EXPLOSION_MAX*4], };
		uv2s_ = new Vector2[2][] { new Vector2[EXPLOSION_MAX*4], new Vector2[EXPLOSION_MAX*4], };

		var triangles = new int[EXPLOSION_MAX * 6];
		for (var i = 0; i < EXPLOSION_MAX; ++i) {
			triangles[i*6+0] = i*4+0;
			triangles[i*6+1] = i*4+1;
			triangles[i*6+2] = i*4+2;
			triangles[i*6+3] = i*4+2;
			triangles[i*6+4] = i*4+1;
			triangles[i*6+5] = i*4+3;
		}

		var uvs = new Vector2[EXPLOSION_MAX*4];
		for (var i = 0; i < EXPLOSION_MAX; ++i) {
			uvs[i*4+0] = new Vector2(0f, 0f);
			uvs[i*4+1] = new Vector2(1f, 0f);
			uvs[i*4+2] = new Vector2(0f, 1f);
			uvs[i*4+3] = new Vector2(1f, 1f);
		}

		mesh_ = new Mesh();
		mesh_.MarkDynamic();
		mesh_.name = "explosion";
		mesh_.vertices = vertices_[0];
		mesh_.triangles = triangles;
		mesh_.uv = uvs;
		mesh_.uv2 = uv2s_[0];
		mesh_.bounds = new Bounds(Vector3.zero, Vector3.one * 99999999);
		material_ = material;

		spawn_index_ = 0;
	}

	public void begin()
	{
	}

	public void end(int front)
	{
		for (var i = 0; i < EXPLOSION_MAX; ++i) {
			int idx = i*4;
			vertices_[front][idx+0] = positions_[i];
			vertices_[front][idx+1] = positions_[i];
			vertices_[front][idx+2] = positions_[i];
			vertices_[front][idx+3] = positions_[i];
			uv2s_[front][idx+0] = uv2_list_[i];
			uv2s_[front][idx+1] = uv2_list_[i];
			uv2s_[front][idx+2] = uv2_list_[i];
			uv2s_[front][idx+3] = uv2_list_[i];
		}
	}

	public void render(int front, Camera camera, float render_time)
	{
		mesh_.vertices = vertices_[front];
		mesh_.uv2 = uv2s_[front];
		material_.SetVector(material_CamUp, camera.transform.up);
		material_.SetFloat(material_CurrentTime, render_time);
	}

	public void spawn(ref Vector3 pos, float update_time)
	{
		int id = spawn_index_;
		++spawn_index_;
		if (spawn_index_ >= EXPLOSION_MAX) {
			spawn_index_ = 0;
		}

		positions_[id] = pos;
		var rot = MyRandom.Range(0, Mathf.PI*2f);
		uv2_list_[id] = new Vector2(update_time, rot);
	}
}

} // namespace UTJ {
