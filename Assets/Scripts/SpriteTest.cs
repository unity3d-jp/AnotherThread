using UnityEngine;
using System.Collections;

namespace UTJ {

public class SpriteTest : MonoBehaviour {

	const float ATLAS_WIDTH = 256f;
	public Sprite[] sprites_;
	private MeshFilter mf_;

	void Start()
	{
		mf_ = GetComponent<MeshFilter>();
	}
	
	void Update()
	{
		var sprite = sprites_[1];

		var vertices = new Vector3[4];
		vertices[0] = new Vector3(0f, 0f, 0f);
		vertices[1] = new Vector3(1f, 0f, 0f);
		vertices[2] = new Vector3(0f, 1f, 0f);
		vertices[3] = new Vector3(1f, 1f, 0f);
		var triangles = new int[6];
		triangles[0] = 0;
		triangles[1] = 1;
		triangles[2] = 2;
		triangles[3] = 2;
		triangles[4] = 1;
		triangles[5] = 3;
		var uvs = new Vector2[4];
		var x0 = sprite.textureRect.xMin/ATLAS_WIDTH;
		var x1 = sprite.textureRect.xMax/ATLAS_WIDTH;
		var y0 = sprite.textureRect.yMin/ATLAS_WIDTH;
		var y1 = sprite.textureRect.yMax/ATLAS_WIDTH;
		uvs[0] = new Vector2(x0, y0);
		uvs[1] = new Vector2(x1, y0);
		uvs[2] = new Vector2(x0, y1);
		uvs[3] = new Vector2(x1, y1);
		var colors = new Color[4];
		colors[0] = new Color(1f, 1f, 1f, 1f);
		colors[1] = new Color(1f, 1f, 1f, 1f);
		colors[2] = new Color(1f, 1f, 1f, 1f);
		colors[3] = new Color(1f, 1f, 1f, 1f);

		var mesh = new Mesh();
		mesh.vertices = vertices;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.colors = colors;
		mf_.sharedMesh = mesh;
	}
}

} // namespace UTJ {
