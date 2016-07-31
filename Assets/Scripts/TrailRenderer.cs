using UnityEngine;
using System.Collections;

namespace UTJ {

[RequireComponent(typeof(MeshFilter),typeof(MeshRenderer))]
public class TrailRenderer : MonoBehaviour {

	private MeshFilter mf_;
	private MeshRenderer mr_;

	void Start()
	{
		mf_ = GetComponent<MeshFilter>();
		mr_ = GetComponent<MeshRenderer>();
		mf_.sharedMesh = Trail.Instance.getMesh();
		mr_.sharedMaterial = Trail.Instance.getMaterial();
		mr_.SetPropertyBlock(Trail.Instance.getMaterialPropertyBlock());
	}
}

} // namespace UTJ {
