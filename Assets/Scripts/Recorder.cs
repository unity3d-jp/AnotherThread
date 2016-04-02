// e.g. ffmpeg -r 60 -i "./work/conv/20151221_%04d.png" -vcodec mpeg4 -b 6000k out.mov

using UnityEngine;
using System.Collections;

public class Recorder : MonoBehaviour
{
	public KeyCode[] screenCaptureKeys;
	public KeyCode[] keyModifiers;

	private int minimumWidth = 480;
    private int minimumHeight = 272;
	private string directory = "movie_work/";
    private string baseFilename;
    private int framerate = 60;
    public bool isRecording = false;
    public int endFrameno = 60;

    private int frameno = -1;

	void Reset ()
	{
		screenCaptureKeys = new KeyCode[]{ KeyCode.R };
		keyModifiers = new KeyCode[] { KeyCode.LeftShift, KeyCode.RightShift };
    
        baseFilename = System.DateTime.Now.ToString("yyyyMMdd");
    }

    void Start()
    {
		Reset();
        Time.captureFramerate = framerate;
    }

	void Update ()
	{
        checkRecodingKey();

        if (isRecording == true)
        {
            TakeScreenShot();
        }
	}

    bool checkRecodingKey()
    {
        bool isModifierPressed = false;
        bool ret = false;
        if (keyModifiers.Length > 0)
        {
            foreach (KeyCode keyCode in keyModifiers)
            {
                if (Input.GetKey(keyCode))
                {
                    isModifierPressed = true;
                    break;
                }
            }
        }

        if (isModifierPressed)
        {
            foreach (KeyCode keyCode in screenCaptureKeys)
            {
                if (Input.GetKeyDown(keyCode))
                {
                    isRecording = !isRecording;
                }
            }
        }
        return ret;
    }

	public void TakeScreenShot ()
	{
		float rw = (float)minimumWidth / Screen.width;
		float rh = (float)minimumHeight / Screen.height;
		int scale = (int)Mathf.Ceil(Mathf.Max(rw, rh));

        ++frameno;
        string path = string.Format("{0}{1}_{2:D4}.png", directory, baseFilename, frameno);

		Application.CaptureScreenshot(path, scale);
		Debug.Log(string.Format("screen shot : path = {0}, scale = {1} (screen = {2}, {3})",
			path, scale, Screen.width, Screen.height), this);

        if (endFrameno > 0 && frameno >= endFrameno)
        {
            isRecording = false;
        }
	}
}