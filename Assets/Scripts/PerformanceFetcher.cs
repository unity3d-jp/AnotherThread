using UnityEngine;
using System.Collections;

namespace UTJ {

public class PerformanceFetcher : MonoBehaviour {

	void OnPreRender()
	{
		SystemManager.Instance.beginPerformanceMeter();
	}

	void OnPreCull()
	{
		SystemManager.Instance.endPerformanceMeter();
	}

}

} // namespace UTJ {
