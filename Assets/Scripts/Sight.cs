using UnityEngine;
using System.Collections;

namespace UTJ {

public class Sight
{
	// singleton
	static Sight instance_;
	public static Sight Instance { get { return instance_ ?? (instance_ = new Sight()); } }

	const int SIGHT_MAX = 16;
	const float LINE_WIDTH = 3f;

	private Vector2[] positions_;
	private float[] sizes_;

	private Vector3[][] vertices_;
	private int spawn_index_;
	private Mesh mesh_;
	private Material material_;

	public void init(Material material)
	{
		positions_ = new Vector2[SIGHT_MAX];
		sizes_ = new float[SIGHT_MAX];
		for (var i = 0; i < sizes_.Length; ++i) {
			sizes_[i] = 0f;
		}

		vertices_ = new Vector3[2][] { new Vector3[SIGHT_MAX*8], new Vector3[SIGHT_MAX*8], };

		var triangles = new int[SIGHT_MAX * 24];
		for (var i = 0; i < SIGHT_MAX; ++i) {
			triangles[i*24+ 0] = i*8+0;
			triangles[i*24+ 1] = i*8+3;
			triangles[i*24+ 2] = i*8+1;
			triangles[i*24+ 3] = i*8+0;
			triangles[i*24+ 4] = i*8+2;
			triangles[i*24+ 5] = i*8+3;
			triangles[i*24+ 6] = i*8+2;
			triangles[i*24+ 7] = i*8+5;
			triangles[i*24+ 8] = i*8+3;
			triangles[i*24+ 9] = i*8+2;
			triangles[i*24+10] = i*8+4;
			triangles[i*24+11] = i*8+5;
			triangles[i*24+12] = i*8+4;
			triangles[i*24+13] = i*8+7;
			triangles[i*24+14] = i*8+5;
			triangles[i*24+15] = i*8+4;
			triangles[i*24+16] = i*8+6;
			triangles[i*24+17] = i*8+7;
			triangles[i*24+18] = i*8+6;
			triangles[i*24+19] = i*8+1;
			triangles[i*24+20] = i*8+7;
			triangles[i*24+21] = i*8+6;
			triangles[i*24+22] = i*8+0;
			triangles[i*24+23] = i*8+1;
		}

		var colors = new Color[SIGHT_MAX*8];
		for (var i = 0; i < colors.Length; ++i) {
			colors[i] = new Color(0.1f, 1f, 0.2f);
		}
		
		mesh_ = new Mesh();
		mesh_.MarkDynamic();
		mesh_.name = "sight";
		mesh_.vertices = vertices_[0];
		mesh_.triangles = triangles;
		mesh_.colors = colors;
		mesh_.bounds = new Bounds(Vector3.zero, Vector3.one * 99999999);
		material_ = material;

		spawn_index_ = 0;
	}

	public int spawn()
	{
		int id = spawn_index_;
		++spawn_index_;
		if (spawn_index_ >= SIGHT_MAX) {
			spawn_index_ = 0;
		}
		sizes_[id] = 240f;
		return id;
	}

	public void updateAll(float dt)
	{
		for (var i = 0; i < sizes_.Length; ++i) {
			if (sizes_[i] >= 0f) {
				sizes_[i] -= 960f * dt;
			}
		}
	}

	public void renderUpdate(int id, float x, float y)
	{
		positions_[id].x = x;
		positions_[id].y = y;
	}

	public bool isShown(int id)
	{
		return sizes_[id] > 0f;
	}

	public void begin()
	{
	}

	public void end(int front)
	{
		for (var i = 0; i < SIGHT_MAX; ++i) {
			int idx = i*8;
			float size = sizes_[i];
			float size2 = size + LINE_WIDTH;
			float z = size > 0f ? 10f : -999f;
			vertices_[front][idx+0] = new Vector3(positions_[i].x - size2,
												  positions_[i].y - size2,
												  z);
			vertices_[front][idx+2] = new Vector3(positions_[i].x + size2,
												  positions_[i].y - size2,
												  z);
			vertices_[front][idx+4] = new Vector3(positions_[i].x + size2,
												  positions_[i].y + size2,
												  z);
			vertices_[front][idx+6] = new Vector3(positions_[i].x - size2,
												  positions_[i].y + size2,
												  z);
			vertices_[front][idx+1] = new Vector3(positions_[i].x - size,
												  positions_[i].y - size,
												  z);
			vertices_[front][idx+3] = new Vector3(positions_[i].x + size,
												  positions_[i].y - size,
												  z);
			vertices_[front][idx+5] = new Vector3(positions_[i].x + size,
												  positions_[i].y + size,
												  z);
			vertices_[front][idx+7] = new Vector3(positions_[i].x - size,
												  positions_[i].y + size,
												  z);
		}
	}

	public void render(int front, Transform transform)
	{
		mesh_.vertices = vertices_[front];
		Graphics.DrawMesh(mesh_,
						  transform.position,
						  transform.rotation,
						  material_,
						  8 /* FinalRender layer */,
						  null /* camera */,
						  0 /* submeshIndex */,
						  null /* material_property_block_ */,
						  false /* castShadows */,
						  false /* receiveShadows */);
	}


	/*
	 * test
	 */
	public void testRender(int front)
	{
		mesh_.vertices = vertices_[front];
	}
	public Mesh getMesh() { return mesh_; }
	public Material getMaterial() { return material_; }

}

} // namespace UTJ {
