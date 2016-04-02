using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UTJ {

public class RigidbodyTest : MonoBehaviour
{
	private RigidbodyTransform rigidbody_;

	void Start()
	{
		rigidbody_.init();
		rigidbody_.setRotateDamper(1f);
	}

	void Update()
	{
		var power_x = Input.GetAxis("Horizontal")*Mathf.PI * 1f;
		var power_y = Input.GetAxis("Vertical")*Mathf.PI * 1f;
		float dt = 1.0f/60.0f;
		var up = new Vector3(0f, -power_x, 0f);
		rigidbody_.addTorque(ref up);
		var left = new Vector3(power_y, 0f, 0f);
		rigidbody_.addTorque(ref left);
		rigidbody_.update(dt);
		// Debug.Log(rigidbody_.transform_.rotation_.w);
		transform.rotation = rigidbody_.transform_.rotation_;
	}
}


} // namespace UTJ {
