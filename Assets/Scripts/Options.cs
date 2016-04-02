namespace UTJ {

public class Options
{
	private static Options instance_;
	public static Options Instance { get { return instance_ ?? (instance_ = new Options()); } }

	public bool AutoPlay { get; set; }
	public bool PerformanceMeter { get; set; }
	public void init()
	{
		AutoPlay = true;
		PerformanceMeter = true;
	}
}

} // namespace UTJ {
