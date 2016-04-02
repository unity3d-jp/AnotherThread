namespace UTJ {

public struct InputBuffer
{
	public const int INPUT_MAX = 8;
	public int[] buttons_;
}

public class InputManager
{
	// singleton
	static InputManager instance_;
	public static InputManager Instance { get { return instance_ ?? (instance_ = new InputManager()); } }

	public enum Button {
		Horizontal,
		Vertical,
		Fire,
		Pause,
		Roll,
	}

	public InputBuffer[] input_buffer_;
	private int front_;
	public void init()
	{
		input_buffer_ = new InputBuffer[2];
		for (int i = 0; i < 2; ++i) {
			input_buffer_[i].buttons_ = new int[8] { 0, 0, 0, 0, 0, 0, 0, 0, };
		}
		front_ = 0;
	}
	public void flip()
	{
		front_ = 1 - front_;
	}

	public int[] referButtons()
	{
		return input_buffer_[front_].buttons_;
	}
	public int getButton(Button button)
	{
		return input_buffer_[front_].buttons_[(int)button];
	}
}

} // namespace UTJ {
