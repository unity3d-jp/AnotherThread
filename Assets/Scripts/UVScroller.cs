using UnityEngine;
using System.Collections;

namespace UTJ {

[RequireComponent(typeof(MeshRenderer))]
public class UVScroller : MonoBehaviour
{
	public float ScrollSpeed = 2f;
	private MeshRenderer mr_;

	void Start()
	{
		mr_ = GetComponent<MeshRenderer>();
		SystemManager.Instance.registUVScroller(this);
	}

	public void render(float render_time)
	{
		float speed = ScrollSpeed * render_time;
        float offset = Mathf.Repeat(speed, 1f);
        mr_.material.SetTextureOffset("_MainTex", new Vector2(offset, 0f));
	}
}

} // namespace UTJ {
