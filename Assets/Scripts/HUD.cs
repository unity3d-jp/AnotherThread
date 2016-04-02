using System.Collections;
using UnityEngine;

namespace UTJ {

public class HUD {

	// singleton
	static HUD instance_;
	public static HUD Instance { get { return instance_ ?? (instance_ = new HUD()); } }

	// private Color color_ = new Color(1f, 1f, 1f, 0.5f); 
	private Rect location_rect_ = new Rect(0f, 240f, 492f, 87f);
	private Rect weapon_rect_ = new Rect(-400f, -120f, 90f, 113f);
	private Rect maxlockon_rect_ = new Rect(-160f, -190f, 223f, 26f);
	private Rect[] gauge_rect_L_list_;
	private Rect[] gauge_rect_M_list_;
	private Rect[] gauge_rect_S_list_;
	private Rect unitychan_rect_ = new Rect(360f, -158f, 192f, 192f);

	private float current_dt_;
	private float update_time_;

	private IEnumerator unitychan_enumerator_;
	private float unitychan_width_ratio_;
	private float unitychan_height_ratio_;
	private MySprite.Kind unitychan_kind_;

	private Vector2 damage_mark_target_;
	private Vector2 damage_mark_position_;
	private int damage_mark_phase_;
	private float damage_mark_time_;
	private bool damage_mark_on_;

	public void init()
	{
		gauge_rect_L_list_ = new Rect[4] {
			new Rect(-418f, -220, 53f, 66f),
			new Rect(-378f, -220, 53f, 66f),
			new Rect(-338f, -220, 53f, 66f),
			new Rect(-298f, -220, 53f, 66f),
		};
		gauge_rect_M_list_ = new Rect[4] {
			new Rect(-260f, -229, 40f, 48f),
			new Rect(-230f, -229, 40f, 48f),
			new Rect(-200f, -229, 40f, 48f),
			new Rect(-170f, -229, 40f, 48f),
		};
		gauge_rect_S_list_ = new Rect[8] {
			new Rect(-144f, -232, 27f, 43f),
			new Rect(-126f, -232, 27f, 43f),
			new Rect(-108f, -232, 27f, 43f),
			new Rect( -90f, -232, 27f, 43f),
			new Rect( -72f, -232, 27f, 43f),
			new Rect( -54f, -232, 27f, 43f),
			new Rect( -36f, -232, 27f, 43f),
			new Rect( -18f, -232, 27f, 43f),
		};

		unitychan_enumerator_ = unitychan_act();
	}

	private IEnumerator unitychan_act()
	{
		float wait_sec;
		for (;;) {
			wait_sec = MyRandom.Range(5f, 15f);
			for (var i = new Utility.WaitForSeconds(wait_sec, update_time_); !i.end(update_time_);) {
				yield return null;
			}
			unitychan_height_ratio_ = 4f/128f;
			for (unitychan_width_ratio_ = 0f;
				 unitychan_width_ratio_ < 1f;
				 unitychan_width_ratio_ += 8f * current_dt_) {
				yield return null;
			}
			unitychan_width_ratio_ = 1f;
			
			for (;
				 unitychan_height_ratio_ < 1f;
				 unitychan_height_ratio_ += 8f * current_dt_) {
				yield return null;
			}
			unitychan_height_ratio_ = 1f;
			
			DrawBuffer.SE voice = DrawBuffer.SE.VoiceIkuyo;
			switch (MyRandom.Range(0, 5)) {
				case 0:
					voice = DrawBuffer.SE.VoiceIkuyo;
					unitychan_kind_ = MySprite.Kind.UnityChanLaugh;
					break;
				case 1:
					voice = DrawBuffer.SE.VoiceUwaa;
					unitychan_kind_ = MySprite.Kind.UnityChanOuch;
					break;
				case 2:
					voice = DrawBuffer.SE.VoiceSorosoro;
					unitychan_kind_ = MySprite.Kind.UnityChanLaugh;
					break;
				case 3:
					voice = DrawBuffer.SE.VoiceOtoto;
					unitychan_kind_ = MySprite.Kind.UnityChanGrin;
					break;
				case 4:
					voice = DrawBuffer.SE.VoiceYoshi;
					unitychan_kind_ = MySprite.Kind.UnityChanGrin;
					break;
			}
			SystemManager.Instance.registSound(voice);
			wait_sec = MyRandom.Range(2f, 3f);
			for (var i = new Utility.WaitForSeconds(wait_sec, update_time_); !i.end(update_time_);) {
				yield return null;
			}
			
			for (;
				 unitychan_height_ratio_ > 4f/128f;
				 unitychan_height_ratio_ -= 8f * current_dt_) {
				yield return null;
			}
			unitychan_height_ratio_ = 4f/128f;
			for (;
				 unitychan_width_ratio_ > 0f;
				 unitychan_width_ratio_ -= 8f * current_dt_) {
				yield return null;
			}
			unitychan_width_ratio_ = 0f;
		}
	}

	private void damage_mark_update(float dt, float update_time)
	{
		switch (damage_mark_phase_) {
			case 0:
				// do nothing
				break;
			case 1:
				var diff_x = damage_mark_target_.x - damage_mark_position_.x;
				if (Mathf.Abs(diff_x) < 4f) {
					++damage_mark_phase_;
				} else {
					if (diff_x > 0f) {
						damage_mark_position_.x += 200f * dt;
					} else {
						damage_mark_position_.x -= 200f * dt;
					}
				}
				break;
			case 2:
				var diff_y = damage_mark_target_.y - damage_mark_position_.y;
				if (Mathf.Abs(diff_y) < 4f) {
					++damage_mark_phase_;
					damage_mark_time_ = update_time;
				} else {
					if (diff_y > 0f) {
						damage_mark_position_.y += 200f * dt;
					} else {
						damage_mark_position_.y -= 200f * dt;
					}
				}
				break;
			case 3:
				var elapsed = update_time - damage_mark_time_;
				if (((int)(elapsed * 16f)) % 2 == 0) {
					damage_mark_on_ = true;
				} else {
					damage_mark_on_ = false;
				}
				if (elapsed > 1f) {
					damage_mark_on_ = false;
					damage_mark_phase_ = 0;
				}
				break;
		}
	}
	
	public void setDamagePoint(ref Vector3 position)
	{
		damage_mark_target_.x = position.x * 10f;
		damage_mark_target_.y = position.z * 10f;
		damage_mark_phase_ = 1;
		damage_mark_on_ = true;
	}

	public void update(float dt, float update_time)
	{
		current_dt_ = dt;
		update_time_ = update_time;
		unitychan_enumerator_.MoveNext();
		damage_mark_update(dt, update_time);
	}

	public void renderUpdate(int front)
	{
		MySprite.Instance.put(front, ref location_rect_, MySprite.Kind.Location, MySprite.Type.Half);
		MySprite.Instance.put(front, ref maxlockon_rect_, MySprite.Kind.MaxLockon, MySprite.Type.Half);

		// lockon gauge
		int lock_num = LockTarget.getCurrentLockNum();
		int fired_num = LockTarget.getCurrentFiredNum();
		int charge_num = lock_num - fired_num;
		int idx = 0;
		for (var i = 0; i < gauge_rect_L_list_.Length; ++i) {
			MySprite.Kind kind = (idx < charge_num ? MySprite.Kind.GaugeRL : MySprite.Kind.GaugeBL);
			MySprite.Instance.put(front, ref gauge_rect_L_list_[i], kind, MySprite.Type.Half);
			++idx;
		}
		for (var i = 0; i < gauge_rect_M_list_.Length; ++i) {
			MySprite.Kind kind = (idx < charge_num ? MySprite.Kind.GaugeRM : MySprite.Kind.GaugeBM);
			MySprite.Instance.put(front, ref gauge_rect_M_list_[i], kind, MySprite.Type.Half);
			++idx;
		}
		for (var i = 0; i < gauge_rect_S_list_.Length; ++i) {
			MySprite.Kind kind = (idx < charge_num ? MySprite.Kind.GaugeRS : MySprite.Kind.GaugeBS);
			MySprite.Instance.put(front, ref gauge_rect_S_list_[i], kind, MySprite.Type.Half);
			++idx;
		}

		// unitychan
		if (unitychan_width_ratio_ > 0f) {
			var rect = new Rect(unitychan_rect_.x,
								unitychan_rect_.y,
								unitychan_rect_.width * unitychan_width_ratio_,
								unitychan_rect_.height * unitychan_height_ratio_);
			MySprite.Instance.put(front, ref rect, unitychan_kind_, MySprite.Type.Full);
		}

		// weapon & damage
		MySprite.Instance.put(front, ref weapon_rect_, MySprite.Kind.Weapon, MySprite.Type.Half);
		if (damage_mark_on_) {
			var rect = weapon_rect_;
			rect.x += damage_mark_position_.x;
			rect.y += damage_mark_position_.y;
			rect.width = 32f;
			rect.height = 32f;
			MySprite.Instance.put(front, ref rect, MySprite.Kind.DamageMark, MySprite.Type.GuardMark);
		}
	}
}

} // namespace UTJ {
