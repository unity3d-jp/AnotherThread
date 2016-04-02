using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UTJ {

public class Enemy : Task
{
	const int POOL_MAX = 1024;
	private static Enemy[] pool_;
	private static int pool_index_;

	public static void createPool()
	{
		pool_ = new Enemy[POOL_MAX];
		for (var i = 0; i < POOL_MAX; ++i) {
			var enemy = new Enemy();
			enemy.alive_ = false;
			enemy.type_ = Type.None;
			pool_[i] = enemy;
		}
		pool_index_ = 0;
	}
	

	public enum Type {
		None,
		Zako,
	}
	private enum Phase {
		Alive,
		Dying,
	}

	private Type type_;
	private IEnumerator enumerator_;
	private float update_time_;
	private Phase phase_;

	private Vector3 target_position_;
	private LockTarget lock_target_;

	public IEnumerator zako_act()
	{
		rigidbody_.setDamper(2f);
		rigidbody_.setRotateDamper(2f);
		target_position_.x = MyRandom.Range(-20f, 20);
		target_position_.y = MyRandom.Range(-20f, 20);
		rigidbody_.addForceZ(40f);
		yield return null;
		// for (var i = Utility.WaitForSeconds(10f, update_time_); i.MoveNext();) {
		for (var i = new Utility.WaitForSeconds(10f, update_time_); !i.end(update_time_);) {
			rigidbody_.addForceX(target_position_.x - rigidbody_.transform_.position_.x * 2f);
			rigidbody_.addForceY(target_position_.y - rigidbody_.transform_.position_.y * 2f);
			rigidbody_.addForceZ(10f);
			rigidbody_.addTorqueZ(-rigidbody_.velocity_.x * 1f);

			if (MyRandom.ProbabilityForSecond(1.5f, SystemManager.Instance.getDT())) {
				var pos = Player.Instance.rigidbody_.transform_.position_;
				pos.z += MyRandom.Range(-10f, 10f);
				EnemyBullet.create(ref rigidbody_.transform_.position_,
								   ref pos,
								   50f /* speed */,
								   update_time_);
			}

			if (phase_ == Phase.Dying) {
				// for (var j = Utility.WaitForSeconds(0.1f, update_time_); j.MoveNext();) {
				for (var j = new Utility.WaitForSeconds(0.1f, update_time_); j.end(update_time_);) {
					yield return null;
				}
				break;
			}

			yield return null;
		}
		destroy();
	}

	public static void create(ref Vector3 position, ref Quaternion rotation, Type type)
	{
		int cnt = 0;
		while (pool_[pool_index_].alive_) {
			++pool_index_;
			if (pool_index_ >= POOL_MAX)
				pool_index_ = 0;
			++cnt;
			if (cnt >= POOL_MAX) {
				Debug.LogError("EXCEED Enemy POOL!");
				break;
			}
		}
		var enemy = pool_[pool_index_];
		enemy.init(ref position, ref rotation, type);
	}

	public void init(ref Vector3 position, ref Quaternion rotation, Type type)
	{
		base.init(ref position, ref rotation);
		collider_ = MyCollider.createEnemy();
		MyCollider.initSphereEnemy(collider_, ref position, 1f /* radius */);
		var lock_pos = new Vector3(0, 0, 0);
		lock_target_ = LockTarget.create(this, ref lock_pos);

		type_ = type;
		phase_ = Phase.Alive;
		switch (type_) {
			case Type.None:
				Debug.Assert(false);
				break;

			case Type.Zako:
				/* ここで GC.allocate が発生してしまう */
				enumerator_ = zako_act(); // この瞬間は実行されない
				break;
		}
	}

	public override void destroy()
	{
		type_ = Type.None;
		enumerator_ = null;
		lock_target_.destroy();
		lock_target_ = null;
		MyCollider.destroyEnemy(collider_);
		base.destroy();
	}

	public override void update(float dt, float update_time)
	{
		update_time_ = update_time;
		if (enumerator_ != null) {
			enumerator_.MoveNext();
		}
		if (alive_) {
			if (MyCollider.getHitOpponentForEnemy(collider_) != MyCollider.Type.None) {
				rigidbody_.addTorque(MyRandom.Range(-100f, 100f),
									 MyRandom.Range(-100f, 100f),
									 MyRandom.Range(-100f, 100f));
				rigidbody_.addForceZ(-10000f);
				Explosion.Instance.spawn(ref rigidbody_.transform_.position_, update_time);
				Hahen.Instance.spawn(ref rigidbody_.transform_.position_, update_time);
				SystemManager.Instance.registSound(DrawBuffer.SE.Explosion);
				MyCollider.disableForEnemy(collider_);
				lock_target_.disable();
				phase_ = Phase.Dying;
			}
			if (lock_target_.isHitted()) {
				rigidbody_.addTorque(MyRandom.Range(-100f, 100f),
									 MyRandom.Range(-100f, 100f),
									 MyRandom.Range(-100f, 100f));
				rigidbody_.addForceZ(-10000f);
				lock_target_.clearLock();
				Explosion.Instance.spawn(ref lock_target_.updated_position_, update_time);
				Hahen.Instance.spawn(ref lock_target_.updated_position_, update_time);
				SystemManager.Instance.registSound(DrawBuffer.SE.Explosion);
				MyCollider.disableForEnemy(collider_);
				lock_target_.disable();
				phase_ = Phase.Dying;
			}

			rigidbody_.update(dt);
			MyCollider.updateEnemy(collider_, ref rigidbody_.transform_.position_);
			lock_target_.update();
		}
	}

	public override void renderUpdate(int front, MyCamera camera, ref DrawBuffer draw_buffer)
	{
		lock_target_.renderUpdate(front, camera);
		draw_buffer.regist(ref rigidbody_.transform_, DrawBuffer.Type.Zako);
	}
}

} // namespace UTJ {
