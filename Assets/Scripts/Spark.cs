using UnityEngine;
using System.Collections;

namespace UTJ {

public class Spark
{
	// singleton
	static Spark instance_;
	public static Spark Instance { get { return instance_ ?? (instance_ = new Spark()); } }

	public enum Type
	{
		None,
		NoneA,
		Bullet,
		BulletA,
		EnemyBullet,
		EnemyBulletA,
		Orange,
		OrangeA,
	}

	const int SPARK_MAX = 16;
	const int PARTICLE_NUM = 32;
	const int POINT_MAX = PARTICLE_NUM * SPARK_MAX;

	private Vector3[] positions_;
	private Vector2[] uv2_list_;

	private Vector3[][] normals_;
	private Vector2[][] uv2s_;
	private Matrix4x4 prev_view_matrix_;
	private int spawn_index_;
	private Mesh mesh_;
	private Material material_;
	private MaterialPropertyBlock material_property_block_;

	static readonly int material_CurrentTime = Shader.PropertyToID("_CurrentTime");
	static readonly int material_PreviousTime = Shader.PropertyToID("_PreviousTime");
	static readonly int material_PrevInvMatrix = Shader.PropertyToID("_PrevInvMatrix");

	private bool just_after_reset_;

	public Mesh getMesh() { return mesh_; }
	public Material getMaterial() { return material_; }
	public MaterialPropertyBlock getMaterialPropertyBlock() { return material_property_block_; }

	public void init(Material material)
	{
		positions_ = new Vector3[SPARK_MAX];
		uv2_list_ = new Vector2[SPARK_MAX];

		normals_ = new Vector3[2][] { new Vector3[POINT_MAX*2], new Vector3[POINT_MAX*2], };
		uv2s_ = new Vector2[2][] { new Vector2[POINT_MAX*2], new Vector2[POINT_MAX*2], };

		var vertices = new Vector3[POINT_MAX*2];
		float range = 1f;
		for (var i = 0; i < POINT_MAX; ++i) {
			float x = Random.Range(-1f, 1f);
			float y = Random.Range(-1f, 1f);
			float z = Random.Range(-1f, 1f);
			float len2 = x*x + y*y + z*z;
			float len = Mathf.Sqrt(len2);
			float rlen = 1.0f/len;
			var point = new Vector3(x*rlen*range, y*rlen*range, z*rlen*range);
			vertices[i*2+0] = point;
			vertices[i*2+1] = point;
		}
		var indices = new int[POINT_MAX*2];
		for (var i = 0; i < POINT_MAX*2; ++i) {
			indices[i] = i;
		}
		var uvs = new Vector2[POINT_MAX*2];
		for (var i = 0; i < POINT_MAX; ++i) {
			uvs[i*2+0] = new Vector2(1f, 0f);
			uvs[i*2+1] = new Vector2(0f, 1f);
		}

		spawn_index_ = 0;

		// mesh setup
		mesh_ = new Mesh();
		mesh_.MarkDynamic();
		mesh_.name = "spark";
		mesh_.vertices = vertices;
		mesh_.normals = normals_[0];
		mesh_.uv = uvs;
		mesh_.uv2 = uv2s_[0];
		mesh_.bounds = new Bounds(Vector3.zero, Vector3.one * 99999999);
		mesh_.SetIndices(indices, MeshTopology.Lines, 0);
		material_ = material;
		material_property_block_ = new MaterialPropertyBlock();
#if UNITY_5_3
		material_.SetColor("_Colors0", new Color(0f, 0f, 0f, 0f)); // None
		material_.SetColor("_Colors1", new Color(0f, 0f, 0f, 0f)); // NoneA
		material_.SetColor("_Colors2", new Color(0f, 1f, 0.5f, 1f)); // Bullet
		material_.SetColor("_Colors3", new Color(0f, 1f, 0.5f, 0f)); // BulletA
		material_.SetColor("_Colors4", new Color(1f, 0.5f, 0.25f, 1f)); // EnemyBullet
		material_.SetColor("_Colors5", new Color(1f, 0.5f, 0.25f, 0f)); // EnemyBulletA
		material_.SetColor("_Colors6", new Color(1f, 0.75f, 0.25f, 1f)); // Orange
		material_.SetColor("_Colors7", new Color(1f, 0.75f, 0.25f, 0f)); // OrangeA
#else
		var col_list = new Vector4[] {
			new Color(0f, 0f, 0f, 0f), // None
			new Color(0f, 0f, 0f, 0f), // NoneA
			new Color(0f, 1f, 0.5f, 1f), // Bullet
			new Color(0f, 1f, 0.5f, 0f), // BulletA
			new Color(1f, 0.5f, 0.25f, 1f), // EnemyBullet
			new Color(1f, 0.5f, 0.25f, 0f), // EnemyBulletA
			new Color(1f, 0.75f, 0.25f, 1f), // Orange
			new Color(1f, 0.75f, 0.25f, 0f), // OrangeA
		};
		material_property_block_.SetVectorArray("_Colors", col_list);
#endif

		just_after_reset_ = true;
	}
	
	public void spawn(ref Vector3 pos, Type type, float update_time)
	{
		positions_[spawn_index_] = pos;
		uv2_list_[spawn_index_] = new Vector2(update_time, (float)type);
		++spawn_index_;
		if (spawn_index_ >= SPARK_MAX) {
			spawn_index_ = 0;
		}
	}

	public void begin()
	{
	}

	public void end(int front)
	{
		for (var s = 0; s < SPARK_MAX; ++s) {
			for (var i = 0; i < PARTICLE_NUM; ++i) {
				int idx = ((PARTICLE_NUM * s) + i) *2;
				normals_[front][idx+0] = positions_[s];
				normals_[front][idx+1] = positions_[s];
				uv2s_[front][idx+0].x = uv2_list_[s].x;
				uv2s_[front][idx+0].y = uv2_list_[s].y;
				uv2s_[front][idx+1].x = uv2_list_[s].x;
				uv2s_[front][idx+1].y = uv2_list_[s].y + 1f;
			}
		}
	}


	public void render(int front, Camera camera, float render_time)
	{
		if (material_ == null) {
			return;
		}
		if (just_after_reset_) {
			just_after_reset_ = false;
			prev_view_matrix_ = camera.worldToCameraMatrix;
			return;
		}

		mesh_.normals = normals_[front];
		mesh_.uv2 = uv2s_[front];
		var matrix = prev_view_matrix_ * camera.cameraToWorldMatrix; // prev-view * inverted-cur-view
		material_.SetFloat(material_CurrentTime, render_time);
		material_.SetFloat(material_PreviousTime, render_time - (1f/60f));
		material_.SetMatrix (material_PrevInvMatrix, matrix);
		prev_view_matrix_ = camera.worldToCameraMatrix;
	}
}

} // namespace UTJ {
