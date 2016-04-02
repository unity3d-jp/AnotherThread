using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UTJ {

public class QuaternionTest : MonoBehaviour
{
	public GameObject parent_object_;
	public Vector3 locator_;
	public Vector3 euler_;
	private Quaternion rotation_;
	private QuaternionTest parent_;
	private RigidbodyTransform rigidbody_;

	void Start()
	{
		if (parent_object_ != null) {
			parent_ = parent_object_.GetComponent<QuaternionTest>();
		}

		rotation_ = Quaternion.Euler(euler_);

		rigidbody_.init();
		if (parent_ == null) {
			rigidbody_.transform_.position_ = transform.position;
			rigidbody_.transform_.rotation_ = Quaternion.identity;
		} else {
			var pos = parent_.rigidbody_.transform_.transformPosition(ref locator_);
			rigidbody_.transform_.position_ = pos;
			// 初期化がうまくいかない。作業中
			// var rot = parent_.rigidbody_.transform_.rotation_ * rotation_;
			// rigidbody_.transform_.rotation_ = rot;
		}
		rigidbody_.setRotateDamper(16);
	}

	void Update()
	{
		if (parent_ == null) {
			float power = 32;
			rigidbody_.addTorque(0f, -(float)Input.GetAxis("Horizontal")*power, 0f);
			rigidbody_.addTorque((float)Input.GetAxis("Vertical")*power, 0f, 0f);
		} else {
			var q = (parent_.rigidbody_.transform_.rotation_ * rotation_ *
					 Quaternion.Inverse(rigidbody_.transform_.rotation_));
			var torque = new Vector3(q.x, q.y, q.z) * 128;
			rigidbody_.addTorque(ref torque);
			var pos = parent_.rigidbody_.transform_.transformPosition(ref locator_);
			rigidbody_.transform_.position_ = pos;
		}
		rigidbody_.update(1f/60f);
		transform.position = rigidbody_.transform_.position_;
		transform.rotation = rigidbody_.transform_.rotation_;
	}
}

} // namespace UTJ {
