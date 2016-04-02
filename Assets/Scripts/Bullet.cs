using UnityEngine;
using System.Collections.Generic;

namespace UTJ {

public class Bullet : Task
{
	const int POOL_MAX = 64;
	private static Bullet[] pool_;
	private static int pool_index_;

	public static void createPool()
	{
		pool_ = new Bullet[POOL_MAX];
		for (var i = 0; i < POOL_MAX; ++i) {
			var task = new Bullet();
			task.alive_ = false;
			pool_[i] = task;
		}
		pool_index_ = 0;
	}

	public static void create(ref Vector3 position, ref Quaternion rotation, float speed, float update_time)
	{
		int cnt = 0;
		while (pool_[pool_index_].alive_) {
			++pool_index_;
			if (pool_index_ >= POOL_MAX)
				pool_index_ = 0;
			++cnt;
			if (cnt >= POOL_MAX) {
				Debug.LogError("EXCEED Bullet POOL!");
				break;
			}
		}
		var task = pool_[pool_index_];
		task.init(ref position, ref rotation, speed, update_time);
	}

	private int beam_id_;
	private float start_;

	public void init(ref Vector3 position, ref Quaternion rotation, float speed, float update_time)
	{
		base.init(ref position, ref rotation);
		var dir = rotation * Vector3.forward;
		var velocity = dir * speed;
		rigidbody_.setVelocity(velocity);
		collider_ = MyCollider.createBullet();
		MyCollider.initSphereBullet(collider_, ref position, 1f /* radius */);
		start_ = update_time;
		beam_id_ = Beam.Instance.spawn(0.75f /* width */, Beam.Type.Bullet);
		// beam_id_ = Beam2.Instance.spawn(ref rigidbody_.transform_.position_,
		// 								ref dir,
		// 								1.0f /* length */,
		// 								speed,
		// 								ref col,
		// 								0.75f /* width */);
	}

	public override void destroy()
	{
		Beam.Instance.destroy(beam_id_);
		beam_id_ = -1;
		MyCollider.destroyBullet(collider_);
		base.destroy();
	}

	public override void update(float dt, float update_time)
	{
		if (MyCollider.getHitOpponentForBullet(collider_) != MyCollider.Type.None) {
			Spark.Instance.spawn(ref rigidbody_.transform_.position_, Spark.Type.Bullet, update_time);
			destroy();
			return;
		}

		rigidbody_.update(dt);
		MyCollider.updateBullet(collider_, ref rigidbody_.transform_.position_);

		float sqr_dist = (rigidbody_.transform_.position_.x * rigidbody_.transform_.position_.x +
						  rigidbody_.transform_.position_.y * rigidbody_.transform_.position_.y);
		float radius_sqr = Tube.GetRadiusSqr(rigidbody_.transform_.position_.x, rigidbody_.transform_.position_.y);
		if (sqr_dist > radius_sqr) {
			Spark.Instance.spawn(ref rigidbody_.transform_.position_, Spark.Type.Bullet, update_time);
			destroy();
			return;
		}

		if (update_time - start_ > 1f) { // 寿命
			destroy();
			return;
		}
	}

	public override void renderUpdate(int front, MyCamera camera, ref DrawBuffer draw_buffer)
	{
		const float LENGTH = 2.5f;
		var tail = rigidbody_.transform_.position_ - rigidbody_.velocity_.normalized * LENGTH;
		Beam.Instance.renderUpdate(front,
								   beam_id_,
								   ref rigidbody_.transform_.position_,
								   ref tail);
	}

}

} // namespace UTJ {
