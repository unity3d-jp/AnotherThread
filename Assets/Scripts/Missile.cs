using UnityEngine;

namespace UTJ {

public class Missile : Task
{
	const int POOL_MAX = 64;
	private static Missile[] pool_;
	private static int pool_index_;

	public static void createPool()
	{
		pool_ = new Missile[POOL_MAX];
		for (var i = 0; i < POOL_MAX; ++i) {
			var obj = new Missile();
			obj.alive_ = false;
			pool_[i] = obj;
		}
		pool_index_ = 0;
	}

	public static Missile create(ref Vector3 position, ref Quaternion rotation, LockTarget lock_target)
	{
		int cnt = 0;
		while (pool_[pool_index_].alive_) {
			++pool_index_;
			if (pool_index_ >= POOL_MAX)
				pool_index_ = 0;
			++cnt;
			if (cnt >= POOL_MAX) {
				Debug.LogError("EXCEED Missile POOL!");
				break;
			}
		}
		var obj = pool_[pool_index_];
		obj.init(ref position, ref rotation, lock_target);
		return obj;
	}

	
	private LockTarget lock_target_;
	private int trail_id_ = -1;
	private Vector3 trail_locator_;
	private float arrival_time_;
	private float destroy_start_;

	private void init(ref Vector3 position, ref Quaternion rotation, LockTarget lock_target)
	{
		// var rotation = Quaternion.identity;
		base.init(ref position, ref rotation);
		rigidbody_.init();
		rigidbody_.transform_.position_ = position;
		float theta;
		if (MyRandom.Probability(0.5f)) {
			theta = (MyRandom.Range(-1f, 1f) * Mathf.PI*0.125f) * Mathf.Rad2Deg;
		} else {
			theta = (MyRandom.Range(-1f, 1f) * Mathf.PI*0.125f + Mathf.PI) * Mathf.Rad2Deg;
		}
		var rot = Quaternion.Euler(0f, 0f, theta) * rotation;
		var v = rot * new Vector3(20f, 0f, 0f);
		rigidbody_.setVelocity(ref v);
		lock_target_ = lock_target;
		trail_locator_ = new Vector3(0, 0, -1);
		// var col = new Color(0.1f, 1f, 0.5f);
		var pos = rigidbody_.transform_.transformPosition(ref trail_locator_);
		trail_id_ = Trail.Instance.spawn(ref pos,
										 0.2f /* width */,
										 Trail.Type.Missile);
		arrival_time_ = MyRandom.Range(1.0f, 1.5f);
		destroy_start_ = 0f;
	}

	public override void destroy()
	{
		if (trail_id_ >= 0) {
			Trail.Instance.destroy(trail_id_);
			trail_id_ = -1;
		}
		base.destroy();
	}

	public override void update(float dt, float update_time)
	{
		if (alive_) {
			float flow_z = -20f * dt;
			if (destroy_start_ <= 0f) {
				if (!lock_target_.alive_) {
					destroy_start_ = update_time;
					rigidbody_.setVelocity(0f, 0f, flow_z);
				} else {
					if (arrival_time_ < 0.9f) {
						var diff = lock_target_.updated_position_ - rigidbody_.transform_.position_;
						var accel = (diff/arrival_time_ - rigidbody_.velocity_)*2f/arrival_time_;
						rigidbody_.setAcceleration(ref accel);
					}
					// if (arrival_time_ > 0.5f && MyRandom.Probability(0.5f)) {
					// 	var theta = MyRandom.Range(0, Mathf.PI*2f);
					// 	rigidbody_.addForceX(Mathf.Sin(theta)*100f);
					// 	rigidbody_.addForceY(Mathf.Cos(theta)*100f);
					// }
					arrival_time_ -= dt;
					if (arrival_time_ < 0f) {
						lock_target_.hit();
						destroy_start_ = update_time;
					}
				}
			} else {
				rigidbody_.setVelocity(0f, 0f, 0f);
				if (update_time - destroy_start_ > 0.25f) {
					destroy();
					return;
				}
			}
			rigidbody_.update(dt);

			if (trail_id_ >= 0) {
				var pos = rigidbody_.transform_.transformPosition(ref trail_locator_);
				Trail.Instance.update(trail_id_, ref pos, flow_z, update_time);
			}
		}
	}

	public override void renderUpdate(int front, MyCamera camera, ref DrawBuffer draw_buffer)
	{
		if (trail_id_ >= 0) {
			Trail.Instance.renderUpdate(front, trail_id_);
		}
	}

}

} // namespace UTJ {
