using UnityEngine;

namespace UTJ {

public struct RigidbodyTransform
{
	public MyTransform transform_;
	public Vector3 velocity_;
	private Vector3 acceleration_;
	public float damper_;
	public Vector3 r_velocity_;
	public Vector3 r_acceleration_;
	public float r_damper_;

	public void init()
	{
		transform_.init();
		velocity_ = CV.Vector3Zero;
		acceleration_ = CV.Vector3Zero;
		damper_ = 0f;
		r_velocity_ = CV.Vector3Zero;
		r_acceleration_ = CV.Vector3Zero;
		r_damper_ = 0f;
	}

	public void setDamper(float damper)
	{
		damper_ = damper;
	}

	public void setRotateDamper(float damper)
	{
		r_damper_ = damper;
	}

	public void addForce(ref Vector3 v)
	{
		acceleration_.x += v.x;
		acceleration_.y += v.y;
		acceleration_.z += v.z;
	}

	public void addForceX(float v)
	{
		acceleration_.x += v;
	}
	public void addForceY(float v)
	{
		acceleration_.y += v;
	}
	public void addForceZ(float v)
	{
		acceleration_.z += v;
	}
	public void addForceXY(float x, float y)
	{
		acceleration_.x += x;
		acceleration_.y += y;
	}

	public void setAcceleration(ref Vector3 a)
	{
		acceleration_ = a;
	}

	public void setVelocity(ref Vector3 v)
	{
		velocity_ = v;
	}
	public void setVelocity(float x, float y, float z)
	{
		velocity_.x = x;
		velocity_.y = y;
		velocity_.z = z;
	}
	public void setVelocity(Vector3 velocity)
	{
		velocity_ = velocity;
	}

	public void addTorque(ref Vector3 torque)
	{
		r_acceleration_.x += torque.x;
		r_acceleration_.y += torque.y;
		r_acceleration_.z += torque.z;
	}

	public void addTorque(float x, float y, float z)
	{
		r_acceleration_.x += x;
		r_acceleration_.y += y;
		r_acceleration_.z += z;
	}

	public void addTorqueZ(float torque)
	{
		r_acceleration_.z += torque;
	}

	public void update(float dt)
	{
		// apply dampler
		acceleration_.x -= velocity_.x * damper_;
		acceleration_.y -= velocity_.y * damper_;
		acceleration_.z -= velocity_.z * damper_;
		// update velocity
		velocity_.x += acceleration_.x * dt;
		velocity_.y += acceleration_.y * dt;
		velocity_.z += acceleration_.z * dt;
		acceleration_.x = acceleration_.y = acceleration_.z = 0f; // clear acceleration
		// update position
		transform_.position_.x += velocity_.x * dt;
		transform_.position_.y += velocity_.y * dt;
		transform_.position_.z += velocity_.z * dt;

		/*
		 * for rotation
		 */
		// apply dampler
		r_acceleration_.x -= r_velocity_.x * r_damper_;
		r_acceleration_.y -= r_velocity_.y * r_damper_;
		r_acceleration_.z -= r_velocity_.z * r_damper_;
		// update velocity
		r_velocity_.x += r_acceleration_.x * dt;
		r_velocity_.y += r_acceleration_.y * dt;
		r_velocity_.z += r_acceleration_.z * dt;
		r_acceleration_.x = r_acceleration_.y = r_acceleration_.z = 0f; // clear acceleration
		// update rotation
		var nx = r_velocity_.x * dt;
		var ny = r_velocity_.y * dt;
		var nz = r_velocity_.z * dt;
		var len2 = nx*nx + ny*ny + nz*nz; // sin^2
		var w = Mathf.Sqrt(1f - len2); // (sin^2 + cos^2) = 1
		var q = new Quaternion(nx, ny, nz, w);
		transform_.rotation_ = q * transform_.rotation_;
	}

	public void cancelUpdateForTube(float dt)
	{
		// cancel
		transform_.position_.x -= velocity_.x * dt;
		transform_.position_.y -= velocity_.y * dt;
		transform_.position_.z -= velocity_.z * dt;
		// recalculate
		float len2 = (transform_.position_.x * transform_.position_.x +
					  transform_.position_.y * transform_.position_.y);
		float len = Mathf.Sqrt(len2);
		float rlen = 1f / len;
		float dx = -transform_.position_.y * rlen;
		float dy = transform_.position_.x * rlen;
		float norm = dx * velocity_.x + dy * velocity_.y;
		velocity_.x = dx * norm;
		velocity_.y = dy * norm;
		transform_.position_.x += velocity_.x * dt;
		transform_.position_.y += velocity_.y * dt;
		float PLAYER_WIDTH2 = 1f;
		float offset = rlen * (Tube.RADIUS - PLAYER_WIDTH2);
		transform_.position_.x *= offset;
		transform_.position_.y *= offset;
	}
}

} // namespace UTJ {