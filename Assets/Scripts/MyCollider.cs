using UnityEngine;
using System.Collections;

namespace UTJ {

public struct MyCollider
{
	private static MyCollider player_;
	const int POOL_BULLET_MAX = 128;
	private static MyCollider[] pool_bullet_;
	private static int pool_bullet_index_;
	const int POOL_ENEMY_MAX = 1024;
	private static MyCollider[] pool_enemy_;
	private static int pool_enemy_index_;
	const int POOL_ENEMY_BULLET_MAX = 2048;
	private static MyCollider[] pool_enemy_bullet_;
	private static int pool_enemy_bullet_index_;

	public static void createPool()
	{
		player_.alive_ = false;
		player_.id_ = 0;
		player_.type_ = Type.Player;

		pool_bullet_ = new MyCollider[POOL_BULLET_MAX];
		for (var i = 0; i < POOL_BULLET_MAX; ++i) {
			pool_bullet_[i].alive_ = false;
			pool_bullet_[i].id_ = i;
			pool_bullet_[i].type_ = Type.Bullet;
		}
		pool_bullet_index_ = 0;

		pool_enemy_ = new MyCollider[POOL_ENEMY_MAX];
		for (var i = 0; i < POOL_ENEMY_MAX; ++i) {
			pool_enemy_[i].alive_ = false;
			pool_enemy_[i].id_ = i;
			pool_enemy_[i].type_ = Type.Enemy;
		}
		pool_enemy_index_ = 0;

		pool_enemy_bullet_ = new MyCollider[POOL_ENEMY_BULLET_MAX];
		for (var i = 0; i < POOL_ENEMY_BULLET_MAX; ++i) {
			pool_enemy_bullet_[i].alive_ = false;
			pool_enemy_bullet_[i].id_ = i;
			pool_enemy_bullet_[i].type_ = Type.EnemyBullet;
		}
		pool_enemy_bullet_index_ = 0;
	}

	private static void create(ref MyCollider[] pool, ref int pool_index)
	{
		int cnt = 0;
		while (pool[pool_index].alive_) {
			++pool_index;
			if (pool_index >= pool.Length)
				pool_index = 0;
			++cnt;
			if (cnt >= pool.Length) {
				Debug.LogError("EXCEED Collider POOL!");
				Debug.Assert(false);
				break;
			}
		}
		pool[pool_index].alive_ = true;
		pool[pool_index].disabled_ = false;
		pool[pool_index].opponent_type_ = Type.None;
	}

	public static int createPlayer()
	{
		player_.alive_ = true;
		player_.disabled_ = false;
		player_.opponent_type_ = Type.None;
		return 0;
	}
	
	public static int createBullet()
	{
		create(ref pool_bullet_, ref pool_bullet_index_);
		return pool_bullet_index_;
	}
	public static int createEnemy()
	{
		create(ref pool_enemy_, ref pool_enemy_index_);
		return pool_enemy_index_;
	}
	public static int createEnemyBullet()
	{
		create(ref pool_enemy_bullet_, ref pool_enemy_bullet_index_);
		return pool_enemy_bullet_index_;
	}

	public static void initSpherePlayer(int id, ref Vector3 pos, float radius)
	{
		player_.initSphere(ref pos, radius);
	}

	public static void initSphereBullet(int id, ref Vector3 pos, float radius)
	{
		Debug.Assert(pool_bullet_[id].alive_);
		pool_bullet_[id].initSphere(ref pos, radius);
	}

	public static void initSphereEnemy(int id, ref Vector3 pos, float radius)
	{
		Debug.Assert(pool_enemy_[id].alive_);
		pool_enemy_[id].initSphere(ref pos, radius);
	}

	public static void initSphereEnemyBullet(int id, ref Vector3 pos, float radius)
	{
		Debug.Assert(pool_enemy_bullet_[id].alive_);
		pool_enemy_bullet_[id].initSphere(ref pos, radius);
	}

	public static void updatePlayer(int id, ref Vector3 pos)
	{
		player_.update(ref pos);
	}

	public static void updateBullet(int id, ref Vector3 pos)
	{
		Debug.Assert(pool_bullet_[id].alive_);
		pool_bullet_[id].update(ref pos);
	}

	public static void updateEnemy(int id, ref Vector3 pos)
	{
		Debug.Assert(pool_enemy_[id].alive_);
		pool_enemy_[id].update(ref pos);
	}

	public static void updateEnemyBullet(int id, ref Vector3 pos)
	{
		Debug.Assert(pool_enemy_bullet_[id].alive_);
		pool_enemy_bullet_[id].update(ref pos);
	}

	public static void destroyPlayer(int id)
	{
		player_.alive_ = false;
		player_.opponent_type_ = Type.None;
	}

	public static void destroyBullet(int id)
	{
		Debug.Assert(pool_bullet_[id].alive_);
		pool_bullet_[id].alive_ = false;
		pool_bullet_[id].opponent_type_ = Type.None;
	}

	public static void destroyEnemy(int id)
	{
		Debug.Assert(pool_enemy_[id].alive_);
		pool_enemy_[id].alive_ = false;
		pool_enemy_[id].opponent_type_ = Type.None;
	}

	public static void destroyEnemyBullet(int id)
	{
		Debug.Assert(pool_enemy_bullet_[id].alive_);
		pool_enemy_bullet_[id].alive_ = false;
		pool_enemy_bullet_[id].opponent_type_ = Type.None;
	}

	public static void calculate()
	{
		// clear
		player_.opponent_type_ = Type.None;
		for (var i = 0; i < pool_bullet_.Length; ++i) {
			pool_bullet_[i].opponent_type_ = Type.None;
		}
		for (var i = 0; i < pool_enemy_.Length; ++i) {
			pool_enemy_[i].opponent_type_ = Type.None;
		}
		for (var i = 0; i < pool_enemy_bullet_.Length; ++i) {
			pool_enemy_bullet_[i].opponent_type_ = Type.None;
		}

		// player - enemy
		for (var i = 0; i < pool_enemy_.Length; ++i) {
			if (pool_enemy_[i].alive_ && !pool_enemy_[i].disabled_) {
				check_intersection(ref player_, ref pool_enemy_[i]);
			}
		}
		// player - enemy_bullet
		for (var i = 0; i < pool_enemy_bullet_.Length; ++i) {
			if (pool_enemy_bullet_[i].alive_) {
				check_intersection(ref player_, ref pool_enemy_bullet_[i]);
			}
		}
		// enemy - bullet
		for (var i = 0; i < pool_enemy_.Length; ++i) {
			if (pool_enemy_[i].alive_ && !pool_enemy_[i].disabled_) {
				for (var j = 0; j < pool_bullet_.Length; ++j) {
					if (pool_bullet_[j].alive_) {
						check_intersection(ref pool_enemy_[i], ref pool_bullet_[j]);
					}
				}
			}
		}
	}

	private static void check_intersection(ref MyCollider col0, ref MyCollider col1)
	{
		var diff = col1.center_ - col0.center_;
		var len2 = (diff.x * diff.x +
					diff.y * diff.y +
					diff.z * diff.z);
		var rad2 = col0.radius_+col1.radius_;
		rad2 = rad2 * rad2;
		if (len2 < rad2) {
			col0.opponent_type_ = col1.type_;
			col1.opponent_type_ = col0.type_;
		}
	}

	public static Type getHitOpponentForPlayer(int id)
	{
		Debug.Assert(player_.alive_);
		return player_.opponent_type_;
	}

	public static Type getHitOpponentForBullet(int id)
	{
		Debug.Assert(pool_bullet_[id].alive_);
		return pool_bullet_[id].opponent_type_;
	}

	public static Type getHitOpponentForEnemy(int id)
	{
		Debug.Assert(pool_enemy_[id].alive_);
		return pool_enemy_[id].opponent_type_;
	}

	public static Type getHitOpponentForEnemyBullet(int id)
	{
		Debug.Assert(pool_enemy_bullet_[id].alive_);
		return pool_enemy_bullet_[id].opponent_type_;
	}

	public static void disableForEnemy(int id)
	{
		pool_enemy_[id].disabled_ = true;
		pool_enemy_[id].opponent_type_ = Type.None;
	}


	public enum Type {
		None,
		Player,
		Bullet,
		Enemy,
		EnemyBullet,
	}

	public enum Shape {
		Sphere,
	}
	public bool alive_;
	public bool disabled_;
	public int id_;
	public Type type_;
	public Type opponent_type_;
	public Vector3 center_;
	public float radius_;
	public Shape shape_;

	public void initSphere(ref Vector3 pos, float radius)
	{
		center_ = pos;
		radius_ = radius;
		shape_ = Shape.Sphere;
	}

	public void update(ref Vector3 pos)
	{
		center_ = pos;
	}
}

} // namespace UTJ {
