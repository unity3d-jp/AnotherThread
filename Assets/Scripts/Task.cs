using UnityEngine;

namespace UTJ {

public abstract class Task
{
	public bool alive_;
	public RigidbodyTransform rigidbody_;
	public int collider_;
	public abstract void update(float dt, float update_time);
	public abstract void renderUpdate(int front, MyCamera camera, ref DrawBuffer draw_buffer);

	public virtual void init()
	{
		alive_ = true;
		TaskManager.Instance.add(this);
		rigidbody_.init();
	}
	public virtual void init(ref MyTransform transform)
	{
		init();
		rigidbody_.transform_ = transform;
	}
	public virtual void init(ref Vector3 position, ref Quaternion rotation)
	{
		init();
		rigidbody_.transform_.position_ = position;
		rigidbody_.transform_.rotation_ = rotation;
	}

	public virtual void destroy()
	{
		TaskManager.Instance.remove(this);
		alive_ = false;
		collider_ = -1;
	}
}

} // namespace UTJ {
