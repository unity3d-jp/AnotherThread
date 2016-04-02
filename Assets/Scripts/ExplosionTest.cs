using UnityEngine;
using System.Collections;

namespace UTJ {

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class ExplosionTest : MonoBehaviour {

	public Camera camera_;
	public Material material_;
	private bool ready_ = false;

	IEnumerator loop()
	{
		ready_ = true;

		GetComponent<MeshRenderer>().sharedMaterial = material_;
		var range = 2.5f;
		for (;;) {
			Explosion.Instance.begin();
			var pos = new Vector3(Random.Range(-range, range),
								  Random.Range(-range, range),
								  Random.Range(-range, range));
			Explosion.Instance.spawn(ref pos, Time.time);
			Explosion.Instance.end(0 /* front */);
			yield return null;
		}
	}

	void Awake()
	{
		Explosion.Instance.init(material_);
	}

	void Start()
	{
		StartCoroutine(loop());
	}
	
	void Update()
	{
		if (!ready_) {
			return;
		}
		Explosion.Instance.render(0 /* front */, camera_, Time.time);
		var mesh = Explosion.Instance.getMesh();
		GetComponent<MeshFilter>().sharedMesh = mesh;
		var material = Explosion.Instance.getMaterial();
		GetComponent<MeshRenderer>().material = material;
	}
}

} // namespace UTJ {
