using UnityEngine;
using System.Collections;

namespace UTJ {

public class GUIRenderer : MonoBehaviour {

	// singleton
	static GUIRenderer instance_;
	public static GUIRenderer Instance { get { return instance_ ?? (instance_ = GameObject.Find("FinalCamera").GetComponent<GUIRenderer>()); } }

	public Sprite[] sprites_;
	public Material sight_material_;
	public Material sprite_material_;
	public Font font_;
	public Material font_material_;

	void Awake()
	{
		Sight.Instance.init(sight_material_);
		MySprite.Instance.init(sprites_, sprite_material_);
		MyFont.Instance.init(font_, font_material_);
	}

	void Update()
	{
		int front = SystemManager.Instance.getRenderingFront();
		Sight.Instance.render(front, transform);
		MySprite.Instance.render(front, transform);
		MyFont.Instance.render(front, transform);
	}

	void OnPostRender()
	{
		SystemManager.Instance.endPerformanceMeter2();
    }
}

} // namespace UTJ {