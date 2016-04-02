using UnityEngine;
using System.Collections;

namespace UTJ {

public class UVScrollerTest : MonoBehaviour {

	UVScroller uv_scroller_;
	float time_;

	void Start()
	{
		uv_scroller_ = GetComponent<UVScroller>();
		time_ = 0f;
	}
	
	void Update()
	{
		time_ += Time.deltaTime;
		uv_scroller_.render(time_);
	}
}

} // namespace UTJ {
