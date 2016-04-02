using UnityEngine;
using System.Collections;
using System.Collections.Generic;

namespace UTJ {

public class GameManager
{
	// singleton
	static GameManager instance_;
	public static GameManager Instance { get { return instance_ ?? (instance_ = new GameManager()); } }

	private IEnumerator enumerator_;
	private float update_time_;

	public void init()
	{
		enumerator_ = act();	// この瞬間は実行されない
	}

	public void update(float dt, float update_time)
	{
		update_time_ = update_time;
		enumerator_.MoveNext();
	}

	private IEnumerator act()
	{
		for (;;) {
			yield return null;
			var pos = new Vector3(MyRandom.Range(-15f, 15f), MyRandom.Range(-15f, 15f), -10f);
			Enemy.create(ref pos, ref CV.QuaternionIdentity, Enemy.Type.Zako);
			// for (var i = Utility.WaitForSeconds(0.20f, update_time_); i.MoveNext();) { yield return null; }
			for (var i = new Utility.WaitForSeconds(0.2f, update_time_); !i.end(update_time_);) {
				yield return null;
			}
		}
	}
}

} // namespace UTJ {
