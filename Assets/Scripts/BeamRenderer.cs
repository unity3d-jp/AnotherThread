using UnityEngine;
using System.Collections;

namespace UTJ {

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class BeamRenderer : MonoBehaviour {

	private MeshFilter mf_;
	private MeshRenderer mr_;

	void Start()
	{
		mf_ = GetComponent<MeshFilter>();
		mr_ = GetComponent<MeshRenderer>();
		mf_.sharedMesh = Beam.Instance.getMesh();
		mr_.sharedMaterial = Beam.Instance.getMaterial();
		mr_.SetPropertyBlock(Beam.Instance.getMaterialPropertyBlock());
	}
}

} // namespace UTJ {
