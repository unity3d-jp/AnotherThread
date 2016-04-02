using UnityEngine;

namespace UTJ {

public class LockTarget
{
	const int POOL_MAX = 128;
	private static LockTarget[] pool_;
	private static int pool_index_;
	private static int lock_max_ = 16;
	private static int fired_num_ = 0;
	private static int lock_num_ = 0;

	public static int getCurrentLockNum() { return lock_num_; }
	public static int getCurrentFiredNum() { return fired_num_; }

	public static void createPool()
	{
		pool_ = new LockTarget[POOL_MAX];
		for (var i = 0; i < POOL_MAX; ++i) {
			var obj = new LockTarget();
			obj.alive_ = false;
			pool_[i] = obj;
		}
		pool_index_ = 0;
	}

	public static LockTarget create(Task owner_task, ref Vector3 position)
	{
		int cnt = 0;
		while (pool_[pool_index_].alive_) {
			++pool_index_;
			if (pool_index_ >= POOL_MAX)
				pool_index_ = 0;
			++cnt;
			if (cnt >= POOL_MAX) {
				Debug.LogError("EXCEED LockTarget POOL!");
				break;
			}
		}
		var obj = pool_[pool_index_];
		obj.init(owner_task, ref position);
		return obj;
	}

	public static bool checkAll(Task player)
	{
		if (lock_num_ >= lock_max_) {
			return false;
		}

		bool locked = false;
		// var mat = player.rigidbody_.transform_.getTRS();
		// var inv_mat = mat.inverse;
		var inv_mat = player.rigidbody_.transform_.getInverseR();
		for (var i = 0; i < pool_.Length; ++i) {
			var lock_target = pool_[i];
			if (lock_target.alive_ &&
				!lock_target.disabled_ &&
				!lock_target.fired_ &&
				!lock_target.locked_ &&
				!lock_target.hitted_) {
				if (lock_target.isEntering(ref inv_mat,
										   ref player.rigidbody_.transform_.position_,
										   1f /* ratio */,
										   1f /* dist_min */,
										   500f /* dist_max */)) {
					lock_target.setLock();
					locked = true;
					++lock_num_;
					if (lock_num_ >= lock_max_) {
						break;
					}
				}
			}
		}
		return locked;
	}

	public static bool fireMissiles(Task player)
	{
		bool fired = false;
		if (lock_num_ > 0) {
			for (var i = 0; i < pool_.Length; ++i) {
				var lock_target = pool_[i];
				if (lock_target.alive_ && !lock_target.disabled_) {
					if (lock_target.locked_ && !lock_target.fired_) {
						Missile.create(ref player.rigidbody_.transform_.position_,
									   ref player.rigidbody_.transform_.rotation_,
									   lock_target);
						lock_target.fired_ = true;
						++fired_num_;
						fired = true;
					}
				}
			}
		}
		return fired;
	}

	public bool alive_;
	public bool disabled_;
	private Task owner_task_;
	private Vector3 position_;
	public Vector3 updated_position_;
	public bool locked_;
	public bool fired_;
	public bool hitted_;
	private int sight_id_;
	
	private void init(Task owner_task, ref Vector3 position)
	{
		alive_ = true;
		disabled_ = false;
		owner_task_ = owner_task;
		position_ = position;
		locked_ = false;
		fired_ = false;
		hitted_ = false;
		sight_id_ = -1;
	}
	
	public void destroy()
	{
		alive_ = false;
		if (fired_) {
			--fired_num_;
			fired_ = false;
		}
		if (locked_) {
			--lock_num_;
			locked_ = false;
		}
	}

	public void disable()
	{
		disabled_ = true;
		if (fired_) {
			--fired_num_;
			fired_ = false;
		}
		if (locked_) {
			--lock_num_;
			locked_ = false;
		}
	}

	public void update()
	{
		updated_position_ = owner_task_.rigidbody_.transform_.transformPosition(ref position_);
		if (!owner_task_.alive_) {
			destroy();
		}
	}

	public bool isHitted()
	{
		return hitted_;
	}

	public void hit()
	{
		hitted_ = true;
	}

	public void setLock()
	{
		locked_ = true;
		sight_id_ = Sight.Instance.spawn();
	}

	public void clearLock()
	{
		if (locked_) {
			--lock_num_;
			locked_ = false;
		}
		if (fired_) {
			--fired_num_;
			fired_ = false;
		}
		hitted_ = false;
		sight_id_ = -1;
	}

	public bool isEntering(ref Matrix4x4 inv_mat,
						   ref Vector3 pos,
						   float ratio,
						   float dist_min,
						   float dist_max)
	{
		var point = updated_position_ - pos;
		point = inv_mat.MultiplyVector(point);
		if (dist_min < point.z && point.z < dist_max) {
			var dist_xy2 = point.x*point.x + point.y*point.y;
			var dist_xy = Mathf.Sqrt(dist_xy2);
			var grad = dist_xy / point.z;
			if (grad < ratio) {
				return true;
			}
		}
		return false;
	}

	public void renderUpdate(int front, MyCamera camera)
	{ 
		if (!disabled_ && (locked_ || fired_)) {
			var v = camera.getScreenPoint(ref updated_position_);
			if (v.z > 0f) {
				if (sight_id_ >= 0) {
					if (Sight.Instance.isShown(sight_id_)) {
						Sight.Instance.renderUpdate(sight_id_, v.x, v.y);
					} else {
						var rect = new Rect(v.x, v.y, 32, 32);
						// var col = fired_ ? new Color(1f, 0.5f, 0f) : new Color(0.1f, 1f, 0.2f);
						var type = fired_ ? MySprite.Type.LockFired : MySprite.Type.Locked;
						MySprite.Instance.put(front, ref rect, MySprite.Kind.Target, type);
					}
				}
			}
		}
	}

}

} // namespace UTJ {
