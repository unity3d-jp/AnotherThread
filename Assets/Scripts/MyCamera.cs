using UnityEngine;

namespace UTJ {

public class MyCamera : Task
{
	const float SCREEN_WIDTH2 = 480f;
	const float SCREEN_HEIGHT2 = 272;
	private Player player_;
	private Vector3 look_target_;
	private Matrix4x4 screen_matrix_;

	public static MyCamera create()
	{
		var camera = new MyCamera();
		camera.init();
		return camera;
	}

	public override void init()
	{
		base.init();
		rigidbody_.init();
		rigidbody_.transform_.position_ = new Vector3(0, 0, -8);
		look_target_ = CV.Vector3Zero;
	}

	public void setPlayer(Player player)
	{
		player_ = player;
	}

	public override void update(float dt, float update_time)
	{
		float roll = player_.getRoll();
		var x = player_.rigidbody_.transform_.position_.x;
		var y = player_.rigidbody_.transform_.position_.y;
		var z = player_.rigidbody_.transform_.position_.z;
		var sn = Mathf.Sin(-roll * Mathf.Deg2Rad);
		var cs = Mathf.Cos(-roll * Mathf.Deg2Rad);
		var pos = new Vector3(cs * x - sn * y,
							  sn * x + cs * y,
							  z);
		var diff = pos -  rigidbody_.transform_.position_;
		diff.x = diff.x*0.5f;
		diff.y = diff.y*0.5f;
		look_target_ = Vector3.Lerp(look_target_, diff, 0.2f);
		rigidbody_.transform_.rotation_ = Quaternion.Euler(0f, 0f, roll) * Quaternion.LookRotation(look_target_);

		// rigidbody_.transform_.position_ = player_.rigidbody_.transform_.position_;
		// rigidbody_.transform_.rotation_ = player_.rigidbody_.transform_.rotation_;

		var view_matrix = Matrix4x4.TRS(rigidbody_.transform_.position_,
										rigidbody_.transform_.rotation_,
										CV.Vector3One);
		var projection_matrix = SystemManager.Instance.ProjectionMatrix;
		screen_matrix_ = projection_matrix * view_matrix.inverse;
	}

	public override void renderUpdate(int front, MyCamera camera, ref DrawBuffer draw_buffer)
	{
		draw_buffer.registCamera(ref rigidbody_.transform_);
	}

	public Vector3 getScreenPoint(ref Vector3 world_position)
	{
		var v = screen_matrix_.MultiplyPoint(world_position);
		return new Vector3(v.x * (-SCREEN_WIDTH2), v.y * (-SCREEN_HEIGHT2), v.z);
	}
}

} // namespace UTJ {
