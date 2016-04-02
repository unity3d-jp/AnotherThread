using UnityEngine;
using System.Collections;

namespace UTJ {

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class SparkTest : MonoBehaviour {

	public Material material_;
	private bool ready_ = false;
	private int cnt_ = 0;

	IEnumerator loop()
	{
		ready_ = true;

		GetComponent<MeshRenderer>().sharedMaterial = material_;
		var range = 2.5f;
		for (;;) {
			var pos = new Vector3(Random.Range(-range, range),
								  Random.Range(-range, range),
								  Random.Range(-range, range));
			// var col = Color.HSVToRGB(Random.Range(0, 1f), 1f, 1f);
			Spark.Instance.begin();
			--cnt_;
			if (cnt_ <= 0) {
				Spark.Instance.spawn(ref pos, Spark.Type.Orange, Time.time);
				cnt_ = 1;
			}
			Spark.Instance.end(0 /* front */);
			yield return null;
		}
	}

	void Start()
	{
		Spark.Instance.init(material_);
		StartCoroutine(loop());
	}
	
	void Update()
	{
		if (!ready_) {
			return;
		}
		Spark.Instance.render(0 /* front */, Camera.main, Time.time);
		var mesh = Spark.Instance.getMesh();
		GetComponent<MeshFilter>().sharedMesh = mesh;
		var material = Spark.Instance.getMaterial();
		GetComponent<MeshRenderer>().material = material;
	}
}

} // namespace UTJ {
