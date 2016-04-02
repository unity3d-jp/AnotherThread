using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UTJ {

public class Player : Task
{
	private static Player player_;
	public static Player Instance { get { return player_; } }

	public const float PLAYER_WIDTH2 = 0.5f;
	private RigidbodyTransform target_cursor_;

	private Vector3 l_bullet_locator_ = new Vector3(-0.5f, 0.25f, 3f);
	private Vector3 r_bullet_locator_ = new Vector3( 0.5f, 0.25f, 3f);
	private float fire_time_;
	private bool prev_fire_button_ = false;

	private float pitch_acceleration_;
	private float pitch_velocity_;
	private float pitch_;
	private float roll_;
	private float roll_target_;

	private int l_trail_;
	private int r_trail_;
	private int t_trail_;
	private Vector3 l_trail_locator_ = new Vector3(-0.4f, 0f, -2f);
	private Vector3 r_trail_locator_ = new Vector3( 0.4f, 0f, -2f);

	// auto play
	private IEnumerator enumerator_;
	private float update_time_;
	private int[] autoplay_buttons_;
	private bool hit_wall_;

	public static Player create()
	{
		var player = new Player();
		Debug.Assert(player_ == null);
		player_ = player;
		player.init();
		return player;
	}

	public override void init()
	{
		base.init();
		rigidbody_.init();
		rigidbody_.setDamper(8f);
		collider_ = MyCollider.createPlayer();
		MyCollider.initSpherePlayer(collider_, ref rigidbody_.transform_.position_,
									0.5f /* radius */);
		target_cursor_.init();
		target_cursor_.transform_.position_.z = 32f;
		target_cursor_.setDamper(8f);
		fire_time_ = 0f;
		pitch_acceleration_ = 0f;
		pitch_velocity_ = 0f;
		pitch_ = 0f;
		roll_ = 0f;
		roll_target_ = 0f;

		float width = 0.3f;
		// var color = new Color(0.1f, 0.6f, 1.0f);
		var lpos = rigidbody_.transform_.transformPosition(ref l_trail_locator_);
		l_trail_ = Trail.Instance.spawn(ref lpos, width, Trail.Type.Player);
		var rpos = rigidbody_.transform_.transformPosition(ref r_trail_locator_);
		r_trail_ = Trail.Instance.spawn(ref rpos, width, Trail.Type.Player);

		// auto play
		enumerator_ = auto_play(); // この瞬間は実行されない
		autoplay_buttons_ = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0, };
		hit_wall_ = false;
	}

	public IEnumerator auto_play()
	{
		for (;;) {
			float time = MyRandom.Range(0.5f, 1f);
			float dir = (float)((int)MyRandom.Range(0f, 8f)) * Mathf.PI * 0.25f;
			for (var i = new Utility.WaitForSeconds(time, update_time_); !i.end(update_time_);) {
				autoplay_buttons_[(int)InputManager.Button.Horizontal] = (int)(Mathf.Cos(dir)*128f);
				autoplay_buttons_[(int)InputManager.Button.Vertical] = (int)(Mathf.Sin(dir)*128f);
				if (MyRandom.ProbabilityForSecond(1f, SystemManager.Instance.getDT())) {
					autoplay_buttons_[(int)InputManager.Button.Fire] = 0;
				} else {
					autoplay_buttons_[(int)InputManager.Button.Fire] = 1;
				}
				if (MyRandom.ProbabilityForSecond(0.6f, SystemManager.Instance.getDT())) {
					autoplay_buttons_[(int)InputManager.Button.Roll] = MyRandom.Range(-1, 2);
				}
				yield return null;
				if (hit_wall_) {
					break;
				}
			}
		}
	}

	public int getButton(InputManager.Button button)
	{
		if (Options.Instance.AutoPlay) {
			return autoplay_buttons_[(int)button];
		} else {
			return InputManager.Instance.getButton(button);
		}
	}

	public override void destroy()
	{
		Trail.Instance.destroy(l_trail_);
		l_trail_ = -1;
		Trail.Instance.destroy(r_trail_);
		r_trail_ = -1;
		MyCollider.destroyPlayer(collider_);
		collider_ = -1;
		base.destroy();
	}

	public float getRoll()
	{
		return roll_;
	}

	public override void update(float dt, float update_time)
	{
		update_time_ = update_time;

		if (MyCollider.getHitOpponentForPlayer(collider_) != MyCollider.Type.None) {
			var pos = new Vector3(MyRandom.Range(-2f, 2f),
								  MyRandom.Range(-2f, 2f),
								  MyRandom.Range(-2f, 2f));
			HUD.Instance.setDamagePoint(ref pos);
		}

		if (Options.Instance.AutoPlay) {
			enumerator_.MoveNext();
		}

		int hori = getButton(InputManager.Button.Horizontal);
		int vert = getButton(InputManager.Button.Vertical);
		bool fire_button = getButton(InputManager.Button.Fire) > 0;
		int lr = getButton(InputManager.Button.Roll);

		roll_target_ += (float)(lr) * 150f * dt;
		roll_ = Mathf.Lerp(roll_, roll_target_, 0.1f);
		const float MOVE_FORCE = 1f;
		const float CURSOR_FORCE = 6f;

		float sn = Mathf.Sin(roll_*Mathf.Deg2Rad);
		float cs = Mathf.Cos(roll_*Mathf.Deg2Rad);
		float vx = hori * cs - vert * sn;
		float vy = hori * sn + vert * cs;
		rigidbody_.addForceXY(vx*MOVE_FORCE, vy*MOVE_FORCE);
		target_cursor_.addForceXY(vx*CURSOR_FORCE, vy*CURSOR_FORCE);

		if (hori < 0) {
			pitch_acceleration_ += 2000f;
		} else if (hori > 0) {
			pitch_acceleration_ += -2000f;
		}

		// cursor update
	    {
			// 外に向けさせない
			float x = rigidbody_.transform_.position_.x + target_cursor_.transform_.position_.x;
			float y = rigidbody_.transform_.position_.y + target_cursor_.transform_.position_.y;
			float len2 = x*x + y*y;
			float R = 64f;
			if (len2 > Tube.RADIUS_SQR) {
				R = 256f;
			}
			target_cursor_.addForceX(-target_cursor_.transform_.position_.x * R);
			target_cursor_.addForceY(-target_cursor_.transform_.position_.y * R);
			target_cursor_.update(dt);
		}

		// pitch
		pitch_acceleration_ += -pitch_ * 8f; // spring
		pitch_acceleration_ += -pitch_velocity_ * 4f; // friction
		pitch_velocity_ += pitch_acceleration_ * dt;
		pitch_ += pitch_velocity_ * dt;
		pitch_acceleration_ = 0f;

		// rotate
		rigidbody_.transform_.rotation_ = Quaternion.LookRotation(target_cursor_.transform_.position_) * Quaternion.Euler(0, 0, pitch_);

		// update
		rigidbody_.update(dt);
		float radius = Tube.RADIUS - PLAYER_WIDTH2;
		if (rigidbody_.transform_.position_.sqrMagnitude >= radius * radius) {
			hit_wall_ = true;
			rigidbody_.cancelUpdateForTube(dt);
			if (MyRandom.ProbabilityForSecond(60f, SystemManager.Instance.getDT())) {
				float x = rigidbody_.transform_.position_.x;
				float y = rigidbody_.transform_.position_.y;
				float z = rigidbody_.transform_.position_.z;
				float len = Mathf.Sqrt(x*x+y*y);
				float rlen = 1f/len;
				float r = rlen * Tube.RADIUS;
				var v = new Vector3(x*r, y*r, z);
				Spark.Instance.spawn(ref v, Spark.Type.Orange, update_time);
			}
		} else {
			hit_wall_ = false;
		}

		// collider
		MyCollider.updatePlayer(collider_, ref rigidbody_.transform_.position_);

		// fire bullets
		if (fire_button && update_time - fire_time_ > 0.06667f) {
			var lpos = rigidbody_.transform_.transformPosition(ref l_bullet_locator_);
			Bullet.create(ref lpos, ref rigidbody_.transform_.rotation_, 120f /* speed */, update_time);
			var rpos = rigidbody_.transform_.transformPosition(ref r_bullet_locator_);
			Bullet.create(ref rpos, ref rigidbody_.transform_.rotation_, 120f /* speed */, update_time);
			SystemManager.Instance.registSound(DrawBuffer.SE.Bullet);
			fire_time_ = update_time;
		}

		// fire missiles
		if (fire_button && !prev_fire_button_) {			// tmp
			bool fired = LockTarget.fireMissiles(this);
			if (fired) {
				SystemManager.Instance.registSound(DrawBuffer.SE.Missile);
			}
		}
		prev_fire_button_ = fire_button;

		// trail
	    {
			float flow_z = -30f * dt;
			var lpos = rigidbody_.transform_.transformPosition(ref l_trail_locator_);
			Trail.Instance.update(l_trail_, ref lpos, flow_z, update_time);
			var rpos = rigidbody_.transform_.transformPosition(ref r_trail_locator_);
			Trail.Instance.update(r_trail_, ref rpos, flow_z, update_time);
		}
	}

	public override void renderUpdate(int front, MyCamera camera, ref DrawBuffer draw_buffer)
	{
		var v = camera.getScreenPoint(ref target_cursor_.transform_.position_);
		if (v.z > 0) {
			var rect = new Rect(v.x, v.y, 96f, 96f);
			MySprite.Instance.put(front, ref rect, MySprite.Kind.Cursor, MySprite.Type.Full);
		}

		// trail
		if (l_trail_ >= 0) {
			Trail.Instance.renderUpdate(front, l_trail_);
		}
		if (r_trail_ >= 0) {
			Trail.Instance.renderUpdate(front, r_trail_);
		}
					   
		draw_buffer.registPlayer(ref rigidbody_.transform_);
	}
}

} // namespace UTJ {
