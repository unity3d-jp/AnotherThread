using UnityEngine;

namespace UTJ {

public struct MyTransform
{
	public Vector3 position_;
	public Quaternion rotation_;

	public void init()
	{
		position_ = CV.Vector3Zero;
		rotation_ = CV.QuaternionIdentity;
	}

	public Vector3 transformPosition(ref Vector3 pos)
	{
		return position_ + rotation_ * pos;
	}

	public Matrix4x4 getTRS()
	{
		return Matrix4x4.TRS(position_, rotation_, new Vector3(1f, 1f, 1f));
	}

	public Matrix4x4 getInverseR()
	{
		var mat_rot = Matrix4x4.TRS(CV.Vector3Zero,
									rotation_,
									CV.Vector3One);
		var mat = mat_rot.transpose;
		// mat.SetColumn(3, new Vector4(-position_.x, -position_.y, -position_.z, 1f));
		return mat;
	}
}

} // namespace UTJ {