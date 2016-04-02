using UnityEngine;
using System.Collections;

namespace UTJ {

public class TrailTest : MonoBehaviour {

	public Material material_;

	IEnumerator loop()
	{
		yield return null;
		
		// var col = new Color(0.5f, 0.5f, 0.5f);
		var pos0 = Vector3.zero;
		var id0 = Trail.Instance.spawn(ref pos0, 0.1f /* width */, Trail.Type.Player);
		var id1 = Trail.Instance.spawn(ref pos0, 0.1f /* width */, Trail.Type.Player);
		// var vlist = new Vector3[16];

		for (;;) {
			// for (var i = 0; i < vlist.Length; ++i) {
			// 	float phase = Mathf.Repeat(Time.time + i*0.04f, 1f) * Mathf.PI * 2f;
			// 	vlist[i] = new Vector3(Mathf.Cos(phase)*2f, (i-4)*0.4f, Mathf.Sin(phase)*2f);
			// }
			// Trail.Instance.update(id, vlist);


		    {
				float phase = Mathf.Repeat(Time.time*0.8f, 1f) * Mathf.PI * 2f;
				var pos = new Vector3(Mathf.Cos(phase), Mathf.Sin(phase), 0f) * 0.5f;
				Trail.Instance.update(id0, ref pos, 8f/60f, Time.time);
			}
		    {
				float phase = Mathf.Repeat(Time.time*0.8f, 1f) * Mathf.PI * 2f;
				var pos = new Vector3(-Mathf.Sin(phase), Mathf.Cos(phase), 0f) * 0.5f;
				Trail.Instance.update(id1, ref pos, 8f/60f, Time.time);
			}
			yield return null;
		}
	}

	void Start()
	{
		Trail.Instance.init(material_);
		StartCoroutine(loop());
	}
	
}

} // namespace UTJ {
