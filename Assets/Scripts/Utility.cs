using UnityEngine;
using System.Collections;

namespace UTJ {

public class Utility
{
	public struct WaitForSeconds
	{
		private float period_;
		private float start_;
		public WaitForSeconds(float period, float update_time)
		{
			period_ = period;
			start_ = update_time;
		}
		public bool end(float update_time)
		{
			return update_time - start_ > period_;
		}
	}
}

} // namespace UTJ {
