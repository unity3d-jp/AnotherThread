using UnityEngine;
using System.Collections;

namespace UTJ {

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class SightTest : MonoBehaviour {

	public Material material_;
	private int cnt_ = 0;

	IEnumerator loop()
	{
		GetComponent<MeshRenderer>().sharedMaterial = material_;
		var range = 200f;
		int id = -1;
		for (;;) {
			Sight.Instance.begin();
			--cnt_;
			if (cnt_ <= 0) {
				id = Sight.Instance.spawn();
				cnt_ = 3;
			}
			if (id >= 0) {
				float x = Random.Range(-range, range);
				float y = Random.Range(-range, range);
				Sight.Instance.renderUpdate(id, x, y);
			}
			Sight.Instance.updateAll(0.0166f /* dt */);
			Sight.Instance.end(0 /* front */);
			yield return null;
		}
	}

	void Start()
	{
		Sight.Instance.init(material_);
		StartCoroutine(loop());
	}
	
	void Update()
	{
		Sight.Instance.testRender(0 /* front */);
		var mesh = Sight.Instance.getMesh();
		GetComponent<MeshFilter>().sharedMesh = mesh;
		var material = Sight.Instance.getMaterial();
		GetComponent<MeshRenderer>().material = material;
	}
}

} // namespace UTJ {
