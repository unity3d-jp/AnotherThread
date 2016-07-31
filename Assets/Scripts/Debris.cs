using UnityEngine;
using System.Collections;

namespace UTJ {

public class Debris
{
	// singleton
	static Debris instance_;
	public static Debris Instance { get { return instance_ ?? (instance_ = new Debris()); } }

	const int POINT_MAX = 1024;
	private float range_;
	private float rangeR_;
	private float move_ = 0f;
	private float prev_time_;
	private Matrix4x4 prev_view_matrix_;
	private int delay_start_count_ = 2;
	private Mesh mesh_;
	private Material material_;
	static readonly int material_MoveTotal = Shader.PropertyToID("_MoveTotal");
	static readonly int material_Move = Shader.PropertyToID("_Move");
	static readonly int material_TargetPosition = Shader.PropertyToID("_TargetPosition");
	static readonly int material_PrevInvMatrix = Shader.PropertyToID("_PrevInvMatrix");

	public Mesh getMesh() { return mesh_; }
	public Material getMaterial() { return material_; }

	public void init(Material material)
	{
		range_ = 16;
		rangeR_ = 1.0f/range_;
		var vertices = new Vector3[POINT_MAX*2];
		for (var i = 0; i < POINT_MAX; ++i) {
			float x = Random.Range(-range_, range_);
			float y = Random.Range(-range_, range_);
			float z = Random.Range(-range_, range_);
			var point = new Vector3(x, y, z);
			vertices[i*2+0] = point;
			vertices[i*2+1] = point;
		}
		var indices = new int[POINT_MAX*2];
		for (var i = 0; i < POINT_MAX*2; ++i) {
			indices[i] = i;
		}
		var colors = new Color[POINT_MAX*2];
		float power = 0.4f;
		for (var i = 0; i < POINT_MAX; ++i) {
			colors[i*2+0] = new Color(power, power, power, 1f);
			colors[i*2+1] = new Color(power, power, power, 0f);
		}
		var uvs = new Vector2[POINT_MAX*2];
		for (var i = 0; i < POINT_MAX; ++i) {
			uvs[i*2+0] = new Vector2(1f, 0f);
			uvs[i*2+1] = new Vector2(0f, 1f);
		}
		mesh_ = new Mesh();
		mesh_.name = "debris";
		mesh_.vertices = vertices;
		mesh_.colors = colors;
		mesh_.uv = uvs;
		mesh_.bounds = new Bounds(Vector3.zero, Vector3.one * 99999999);
		mesh_.SetIndices(indices, MeshTopology.Lines, 0);
		mesh_.UploadMeshData(true /* markNoLogerReadable */);
		material_ = material;
		material_.SetFloat("_Range", range_);
		material_.SetFloat("_RangeR", rangeR_);
	}
	
	public void render(int front, Camera camera, float render_time)
	{
		if (material_ == null) {
			return;
		}

		if (delay_start_count_ > 0) {
			prev_view_matrix_ = camera.worldToCameraMatrix;
			prev_time_ = render_time;
			--delay_start_count_;
			return;
		}
		var target_position = camera.transform.TransformPoint(Vector3.forward * range_);
		var matrix = prev_view_matrix_ * camera.cameraToWorldMatrix; // prev-view * inverted-cur-view
		float flow_speed = 60f;
		material_.SetFloat (material_MoveTotal, move_);
		material_.SetFloat (material_Move, flow_speed * SystemManager.Instance.getDT());
		material_.SetVector(material_TargetPosition, target_position);
		material_.SetMatrix(material_PrevInvMatrix, matrix);
		move_ += flow_speed * (render_time - prev_time_);
		move_ = Mathf.Repeat(move_, range_ * 2f);
		prev_time_ = render_time;
		prev_view_matrix_ = camera.worldToCameraMatrix;
	}
}

} // namespace UTJ {
