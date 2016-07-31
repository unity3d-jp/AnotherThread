using UnityEngine;
using System.Collections;

namespace UTJ {

public class MyFont {

	// singleton
	static MyFont instance_;
	public static MyFont Instance { get { return instance_ ?? (instance_ = new MyFont()); } }

	public enum Type
	{
		None,
		Red,
		Blue,
		Mazenta,
		Green,
		Yellow,
		Cyan,
		White,
	}


	// font metrics
	const int CHAR_START = 32;	// SP is 32.
	const int CHAR_SIZE = 126 - CHAR_START; // ~ is 126
	const int MAGIC_OFFSET = 39;
	private CharacterInfo[] info_;
	private CharacterInfo info8_;

	// work
	public char[] put_string_work_;

	const int FONT_CHAR_MAX = 64;
	private Vector3[][] vertices_;
	private Vector2[][] uvs_;
	public Material material_;
	private MaterialPropertyBlock material_property_block_;
	private Mesh mesh_;
	private int index_;
	
	public void init(Font font, Material material)
	{
		info_ = new CharacterInfo[CHAR_SIZE];
		for (var i = 0; i < info_.Length; ++i) {
			char ch = (char)(i+CHAR_START);
			font.GetCharacterInfo(ch, out info_[i]);
		}
		info8_ = info_[(int)'8'-CHAR_START];
		put_string_work_ = new char[128];

		vertices_ = new Vector3[2][] { new Vector3[FONT_CHAR_MAX*4], new Vector3[FONT_CHAR_MAX*4], };
		uvs_ = new Vector2[2][] { new Vector2[FONT_CHAR_MAX*4], new Vector2[FONT_CHAR_MAX*4], };
		var triangles = new int[FONT_CHAR_MAX*6];
		for (var i = 0; i < FONT_CHAR_MAX; ++i) {
			triangles[i*6+0] = i*4+0;
			triangles[i*6+1] = i*4+1;
			triangles[i*6+2] = i*4+2;
			triangles[i*6+3] = i*4+2;
			triangles[i*6+4] = i*4+1;
			triangles[i*6+5] = i*4+3;
		}
		mesh_ = new Mesh();
		mesh_.name = "font";
		mesh_.MarkDynamic();
		mesh_.vertices = vertices_[0];
		mesh_.uv = uvs_[0];
		mesh_.triangles = triangles;
		mesh_.bounds = new Bounds(Vector3.zero, Vector3.one * 99999999);
		material_ = material;
		material_property_block_ = new MaterialPropertyBlock();
#if UNITY_5_3
		material_.SetColor("_Colors0", new Color(0f, 0f, 0f)); // None
		material_.SetColor("_Colors1", new Color(1f, 0.4f, 0.4f)); // Red
		material_.SetColor("_Colors2", new Color(0.4f, 0.4f, 1f)); // Blue
		material_.SetColor("_Colors3", new Color(1f, 0.4f, 1f)); // Magenta
		material_.SetColor("_Colors4", new Color(0.4f, 1f, 0.4f)); // Green
		material_.SetColor("_Colors5", new Color(1f, 1f, 0.4f)); // Yellow
		material_.SetColor("_Colors6", new Color(0.4f, 1f, 1f)); // Cyan
		material_.SetColor("_Colors7", new Color(1f, 1f, 1f)); // White
#else
		var col_list = new Vector4[] {
			new Color(0f, 0f, 0f), // None
			new Color(1f, 0.4f, 0.4f), // Red
			new Color(0.4f, 0.4f, 1f), // Blue
			new Color(1f, 0.4f, 1f), // Magenta
			new Color(0.4f, 1f, 0.4f), // Green
			new Color(1f, 1f, 0.4f), // Yellow
			new Color(0.4f, 1f, 1f), // Cyan
			new Color(1f, 1f, 1f), // White
		};
		material_property_block_.SetVectorArray("_Colors", col_list);
#endif
	}

	public void begin()
	{
		index_ = 0;
	}

	public void end(int front)
	{
		for (var i = index_*4; i < vertices_[front].Length; ++i) {
			vertices_[front][i] = new Vector3(0f, 0f, -1f);
		}
	}

	public void putNumber(int front, int num, int keta, float scale, float x, float y, Type type)
	{
		int d = num;
		put_string_work_[keta] = '\0';
		for (var i = 0; i < keta; ++i) {
			long v = d % 10;
			d /= 10;
			put_string_work_[keta - i - 1] = (char)((int)'0' + (char)v);
		}
		put_string(front, put_string_work_, scale, x, y, type);
	}

	public float putChar(int front, char ch, float scale, float cx, float y, Type type, bool touhaba)
	{
		CharacterInfo info = info_[(int)ch-CHAR_START];
		int idx = index_*4;
		int cy = (int)y + info.minY;
		int w = (int)((float)(touhaba ? info8_.glyphWidth : info.glyphWidth) * scale);
		int h = (int)((float)(touhaba ? (float)info8_.glyphHeight : info.glyphHeight) * scale);
		vertices_[front][idx+0] = new Vector3(cx,     cy, (float)type);
		vertices_[front][idx+1] = new Vector3(cx+w,   cy, (float)type);
		vertices_[front][idx+2] = new Vector3(cx,   cy+h, (float)type);
		vertices_[front][idx+3] = new Vector3(cx+w, cy+h, (float)type);
		uvs_[front][idx+0] = info.uvBottomLeft;
		uvs_[front][idx+1] = info.uvBottomRight;
		uvs_[front][idx+2] = info.uvTopLeft;
		uvs_[front][idx+3] = info.uvTopRight;
		++index_;
		return cx + w;
	}

	private void put_string(int front, char[] str, float scale, float x, float y, Type type)
	{
		if (index_ >= FONT_CHAR_MAX) {
			Debug.Log("EXCEED font POOL!");
			return;
		}
		float cx = x;
		for (var i = 0; i < str.Length; ++i) {
			if (str[i] == '\0') {
				break;
			}
			cx = putChar(front, str[i], scale, cx, y, type, true /* touhaba */);
		}
	}

	public void putString(int front, string str, float scale, float x, float y, Type type)
	{
		if (index_ >= FONT_CHAR_MAX) {
			Debug.Log("EXCEED font POOL!");
			return;
		}
		float cx = x;
		for (var i = 0; i < str.Length; ++i) {
			cx = putChar(front, str[i], scale, cx, y, type, false /* touhaba */);
		}
	}

	public void render(int front, Transform transform)
	{
		mesh_.vertices = vertices_[front];
		mesh_.uv = uvs_[front];
		Graphics.DrawMesh(mesh_,
						  transform.position,
						  transform.rotation,
						  material_,
						  8 /* FinalRender layer */,
						  null /* camera */,
						  0 /* submeshIndex */,
						  material_property_block_,
						  false /* castShadows */,
						  false /* receiveShadows */);
    }
}

} // namespace UTJ {
