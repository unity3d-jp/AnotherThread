using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Threading;

namespace UTJ {

public class SystemManager : MonoBehaviour {

	// singleton
	static SystemManager instance_;
	public static SystemManager Instance { get { return instance_ ?? (instance_ = GameObject.Find("system_manager").GetComponent<SystemManager>()); } }

	private const int DefaultFps = 60;
	public System.Diagnostics.Stopwatch stopwatch_;
	private Thread update_thread_;
	private int rendering_front_;
	public int getRenderingFront() { return rendering_front_; }
	private DrawBuffer[] draw_buffer_;
	private System.Threading.ManualResetEvent manual_reset_event_;
	private int fps_;
	private float dt_;
	public float getDT() { return dt_; }
	public int getFPS()	{ return fps_; }
	public void setFPS(int fps)	{
		fps_ = fps;
		dt_ = 1.0f / (float)fps_;	
		if (fps_text_ != null) {
			fps_text_.text = "1/"+fps_.ToString();
		}
	}
	private UnityEngine.UI.Text fps_text_;

	private long update_frame_;
	private long update_sync_frame_;
	private long render_frame_;
	private long render_sync_frame_;
	private float update_time_;
	private int gc_start_count_;
	private long used_heap_size_;
	private long mono_heap_size_;
	private long mono_used_size_;

	private bool pause_;
	private bool prev_pause_button_;
	private Player player_;


	public GameObject player_prefab_;
	public GameObject zako_prefab_;
	public Material debris_material_;
	public Material spark_material_;
	public Material beam_material_;
	public Material trail_material_;
	public Material explosion_material_;
	public Material hahen_material_;

	private Camera camera_;
	public Matrix4x4 ProjectionMatrix { get; set; }

	private GameObject player_go_;
	private const int ZAKO_MAX = 64;
	private GameObject[] zako_pool_;
	private UVScroller[] uv_scroller_list_;
	private GameObject pausemenu_canvas_go_;
	
	// audio
	public const int AUDIO_CHANNEL_MAX = 4;

	private const int AUDIOSOURCE_BULLET_MAX = 4;
	private AudioSource[] audio_sources_bullet_;
	int audio_source_bullet_index_;
	public AudioClip se_bullet_;

	private const int AUDIOSOURCE_EXPLOSION_MAX = 4;
	private AudioSource[] audio_sources_explosion_;
	int audio_source_explosion_index_;
	public AudioClip se_explosion_;

	private const int AUDIOSOURCE_MISSILE_MAX = 2;
	private AudioSource[] audio_sources_missile_;
	int audio_source_missile_index_;
	public AudioClip se_missile_;

	private const int AUDIOSOURCE_LOCKON_MAX = 8;
	private AudioSource[] audio_sources_lockon_;
	int audio_source_lockon_index_;
	public AudioClip se_lockon_;

	private AudioSource audio_sources_voice_ikuyo_;
	public AudioClip se_voice_ikuyo_;
	private AudioSource audio_sources_voice_uwaa_;
	public AudioClip se_voice_uwaa_;
	private AudioSource audio_sources_voice_sorosoro_;
	public AudioClip se_voice_sorosoro_;
	private AudioSource audio_sources_voice_ototo_;
	public AudioClip se_voice_ototo_;
	private AudioSource audio_sources_voice_yoshi_;
	public AudioClip se_voice_yoshi_;

	private AudioSource audio_sources_bgm_;
	public AudioClip bgm01_;

	// debug
	private int display_fps_;
	public long update_tick_;
	public long render_update_tick_;
	public long render_tick_;
	public long render_start_tick_;
	public long render_tick2_;
	public long render_start_tick2_;
	public long render_tick3_;
	public long render_tick4_;

	private void init()
	{
	}

	void Awake()
	{
#if UNITY_PSP2
		UnityEngine.PSVita.Diagnostics.enableHUD = true;
#endif

		Application.targetFrameRate = 60;
		DontDestroyOnLoad(gameObject);

		stopwatch_ = new System.Diagnostics.Stopwatch();
		stopwatch_.Start();
		rendering_front_ = 0;

		Instance.init();
		Options.Instance.init();
		InputManager.Instance.init();
		GameManager.Instance.init();
		TaskManager.Instance.init();
		MyCollider.createPool();
		LockTarget.createPool();
		Missile.createPool();
		Bullet.createPool();
		Enemy.createPool();
		EnemyBullet.createPool();
		Debris.Instance.init(debris_material_);
		Spark.Instance.init(spark_material_);
		Beam.Instance.init(beam_material_);
		Trail.Instance.init(trail_material_);
		Explosion.Instance.init(explosion_material_);
		Hahen.Instance.init(hahen_material_);
		HUD.Instance.init();

		draw_buffer_ = new DrawBuffer[2];
		for (int i = 0; i < 2; ++i) {
			draw_buffer_[i].init();
		}
		manual_reset_event_ = new System.Threading.ManualResetEvent(false);
		setFPS(DefaultFps);
		update_frame_ = 0;
		update_sync_frame_ = 0;
		update_time_ = 0f;
		render_frame_ = 0;
		render_sync_frame_ = 0;
		pause_ = false;

		camera_ = GameObject.Find("MainCamera").GetComponent<Camera>(); // Camera.main;
		ProjectionMatrix = camera_.projectionMatrix;
		camera_.enabled = false;
		if (player_prefab_ != null) {
			player_go_ = Instantiate(player_prefab_) as GameObject;
		}

		if (zako_prefab_ != null) {
			zako_pool_ = new GameObject[ZAKO_MAX];
			for (var i = 0; i < ZAKO_MAX; ++i) {
				zako_pool_[i] = Instantiate(zako_prefab_) as GameObject;
				zako_pool_[i].SetActive(false);
			}
		}

		uv_scroller_list_ = new UVScroller[8];

		// pause menu
		pausemenu_canvas_go_ = GameObject.Find("Canvas");
		if (pausemenu_canvas_go_ != null) {
			var go = GameObject.Find("FPSValue");
			fps_text_ = go.GetComponent<UnityEngine.UI.Text>();
			fps_text_.text = "1/" + fps_.ToString();
			pausemenu_canvas_go_.SetActive(false);
		}

		// audio
		audio_sources_bullet_ = new AudioSource[AUDIOSOURCE_BULLET_MAX];
		for (var i = 0; i < AUDIOSOURCE_BULLET_MAX; ++i) {
			audio_sources_bullet_[i] = gameObject.AddComponent<AudioSource>();
			audio_sources_bullet_[i].clip = se_bullet_;
			audio_sources_bullet_[i].volume = 0.05f;
		}
		audio_source_bullet_index_ = 0;
		audio_sources_explosion_ = new AudioSource[AUDIOSOURCE_EXPLOSION_MAX];
		for (var i = 0; i < AUDIOSOURCE_EXPLOSION_MAX; ++i) {
			audio_sources_explosion_[i] = gameObject.AddComponent<AudioSource>();
			audio_sources_explosion_[i].clip = se_explosion_;
			audio_sources_explosion_[i].volume = 0.25f;
		}
		audio_source_explosion_index_ = 0;
		audio_sources_missile_ = new AudioSource[AUDIOSOURCE_MISSILE_MAX];
		for (var i = 0; i < AUDIOSOURCE_MISSILE_MAX; ++i) {
			audio_sources_missile_[i] = gameObject.AddComponent<AudioSource>();
			audio_sources_missile_[i].clip = se_missile_;
			audio_sources_missile_[i].volume = 0.25f;
		}
		audio_source_missile_index_ = 0;
		audio_sources_lockon_ = new AudioSource[AUDIOSOURCE_LOCKON_MAX];
		for (var i = 0; i < AUDIOSOURCE_LOCKON_MAX; ++i) {
			audio_sources_lockon_[i] = gameObject.AddComponent<AudioSource>();
			audio_sources_lockon_[i].clip = se_lockon_;
			audio_sources_lockon_[i].volume = 0.25f;
		}
		audio_source_lockon_index_ = 0;

		audio_sources_voice_ikuyo_ = gameObject.AddComponent<AudioSource>();
		audio_sources_voice_ikuyo_.clip = se_voice_ikuyo_;
		audio_sources_voice_ikuyo_.volume = 0.75f;
		audio_sources_voice_uwaa_ = gameObject.AddComponent<AudioSource>();
		audio_sources_voice_uwaa_.clip = se_voice_uwaa_;
		audio_sources_voice_uwaa_.volume = 0.75f;
		audio_sources_voice_sorosoro_ = gameObject.AddComponent<AudioSource>();
		audio_sources_voice_sorosoro_.clip = se_voice_sorosoro_;
		audio_sources_voice_sorosoro_.volume = 0.75f;
		audio_sources_voice_ototo_ = gameObject.AddComponent<AudioSource>();
		audio_sources_voice_ototo_.clip = se_voice_ototo_;
		audio_sources_voice_ototo_.volume = 0.75f;
		audio_sources_voice_yoshi_ = gameObject.AddComponent<AudioSource>();
		audio_sources_voice_yoshi_.clip = se_voice_yoshi_;
		audio_sources_voice_yoshi_.volume = 0.75f;

		audio_sources_bgm_ = gameObject.AddComponent<AudioSource>();
		audio_sources_bgm_.clip = bgm01_;
		audio_sources_bgm_.volume = 0.5f;
		audio_sources_bgm_.loop = true;

		gc_start_count_ = System.GC.CollectionCount(0 /* generation */);
	}

	void OnDestroy()
	{
		if (update_thread_ != null) {
			update_thread_.Abort();
		}
	}

	private void main_loop()
	{
		long begin_time = stopwatch_.ElapsedTicks;
		
		// flip
		int updating_front = 1 - rendering_front_;
		
		// begin
		Sight.Instance.begin();
		MySprite.Instance.begin();
		MyFont.Instance.begin();
		Beam.Instance.begin(updating_front);
		Trail.Instance.begin(updating_front);
		Spark.Instance.begin();
		Explosion.Instance.begin();
		Hahen.Instance.begin();

		// update
		bool locked = false;
		if (!pause_) {
			GameManager.Instance.update(dt_, update_time_);
			TaskManager.Instance.update(dt_, update_time_);
			// lockon
			locked = LockTarget.checkAll(player_);
			Sight.Instance.updateAll(dt_);
			// HUD
			HUD.Instance.update(dt_, update_time_);
			// collision
			MyCollider.calculate();

			if (InputManager.Instance.getButton(InputManager.Button.Pause) > 0) {
				pause_ = true;
			}
			++update_frame_;
			update_time_ += dt_;
		} else {
			if (InputManager.Instance.getButton(InputManager.Button.Pause) > 0) {
				pause_ = false;
			}
		}
		if (locked) {
			registSound(DrawBuffer.SE.Lockon);
		}
		update_tick_ = stopwatch_.ElapsedTicks - begin_time;
		begin_time = stopwatch_.ElapsedTicks;

		// renderUpdate
		draw_buffer_[updating_front].beginRender();
		TaskManager.Instance.renderUpdate(updating_front, ref draw_buffer_[updating_front]);
		
		// HUD
		HUD.Instance.renderUpdate(updating_front);

		render_update_tick_ = stopwatch_.ElapsedTicks - begin_time;
		// debug info
		if (Options.Instance.PerformanceMeter) {
		    {
				MyFont.Instance.putNumber(updating_front, (int)update_frame_, 8 /* keta */, 0.5f /* scale */,
										  -440f, 270f, MyFont.Type.Green);
			}
		    {
				int gc_count = System.GC.CollectionCount(0 /* generation */) - gc_start_count_;
				MyFont.Instance.putNumber(updating_front, gc_count, 8 /* keta */, 0.5f /* scale */,
										  -440f, 254f, MyFont.Type.Red);
			}
			if (used_heap_size_ != 0) {
				int bytes = (int)used_heap_size_;
				MyFont.Instance.putNumber(updating_front, bytes, 9 /* keta */, 0.5f /* scale */,
										  -340f, 254f, MyFont.Type.Red);
			}
			if (mono_heap_size_ != 0) {
				int bytes = (int)mono_heap_size_;
				MyFont.Instance.putNumber(updating_front, bytes, 9 /* keta */, 0.5f /* scale */,
										  -240f, 254f, MyFont.Type.Red);
			}
			if (mono_used_size_ != 0) {
				int bytes = (int)mono_used_size_;
				MyFont.Instance.putNumber(updating_front, bytes, 9 /* keta */, 0.5f /* scale */,
										  -140f, 254f, MyFont.Type.Red);
			}
		    {
				int task_count = TaskManager.Instance.getCount();
				MyFont.Instance.putNumber(updating_front, task_count, 8 /* keta */, 0.5f /* scale */,
										  -440f, 238f, MyFont.Type.Blue);
			}

			var height = 5f;
			var length = 440f;
			var badget = 16.666f;
			var update_y = 262f;
			var render_y = 254f;
		    {
				var rect0b = new Rect(-length*0.5f, update_y, length, height);
				MySprite.Instance.put(updating_front, ref rect0b, MySprite.Kind.Square, MySprite.Type.Black);

				var update_ratio = (float)update_tick_*1000f/((float)System.Diagnostics.Stopwatch.Frequency*badget);
				var rect1b = new Rect(length*(update_ratio*0.5f - 1f), update_y,
									  length*update_ratio, height);
				MySprite.Instance.put(updating_front, ref rect1b, MySprite.Kind.Square, MySprite.Type.Blue);

				var render_update_ratio = (float)render_update_tick_*1000f/((float)System.Diagnostics.Stopwatch.Frequency*badget);
				var rect2b = new Rect(length*(update_ratio - 1f + render_update_ratio*0.5f), update_y,
									  length*update_ratio, height);
				MySprite.Instance.put(updating_front, ref rect2b, MySprite.Kind.Square, MySprite.Type.Green);
			}
#if UNITY_PSP2 || UNITY_PS4
		    {
				var render_ratio = (float)render_tick_*1000f/((float)System.Diagnostics.Stopwatch.Frequency*badget);
				var rect0b = new Rect(-length*0.5f, render_y, length, height);
				MySprite.Instance.put(updating_front, ref rect0b, MySprite.Kind.Square, MySprite.Type.Black);
				var rect1b = new Rect(length*(render_ratio*0.5f - 1f), render_y, length*render_ratio, height);
				MySprite.Instance.put(updating_front, ref rect1b, MySprite.Kind.Square, MySprite.Type.Red);

				var render_ratio_begin = (float)render_tick3_*1000f/((float)System.Diagnostics.Stopwatch.Frequency*badget);
				var render_ratio_end = (float)render_tick4_*1000f/((float)System.Diagnostics.Stopwatch.Frequency*badget);
				var render_ratio_len = render_ratio_end - render_ratio_begin;
				var rect2b = new Rect(length*(render_ratio_len*0.5f - 1f + render_ratio_begin*0.5f), render_y,
				                      length*(render_ratio_len), height);
				MySprite.Instance.put(updating_front, ref rect2b, MySprite.Kind.Square, MySprite.Type.Magenta);
			}
#else
		    {
				var render_ratio = (float)render_tick2_*1000f/((float)System.Diagnostics.Stopwatch.Frequency*badget);
				var rect0b = new Rect(-length*0.5f, render_y, length, height);
				MySprite.Instance.put(updating_front, ref rect0b, MySprite.Kind.Square, MySprite.Type.Black);
				var rect1b = new Rect(length*(render_ratio*0.5f - 1f), render_y, length*render_ratio, height);
				MySprite.Instance.put(updating_front, ref rect1b, MySprite.Kind.Square, MySprite.Type.Red);
			}
#endif
		}

		// end
		Hahen.Instance.end(updating_front);
		Explosion.Instance.end(updating_front);
		Spark.Instance.end(updating_front);
		Trail.Instance.end();
		Beam.Instance.end();
		MyFont.Instance.end(updating_front);
		MySprite.Instance.end(updating_front);
		Sight.Instance.end(updating_front);
	}

	private void thread_entry()
	{
		var camera = MyCamera.create();
		player_ = Player.create();
		camera.setPlayer(player_);
		TaskManager.Instance.setCamera(camera);

		for (;;) {
			try {
				main_loop();
				while (update_sync_frame_ >= render_sync_frame_) {
					manual_reset_event_.WaitOne();
					manual_reset_event_.Reset();
				}
				++update_sync_frame_;

			} catch (System.Exception e) {
				Debug.Log(e);
			}
		}
	}

	void Start()
	{
		update_thread_ = new Thread(thread_entry);
		update_thread_.Start();
		audio_sources_bgm_.Play();
	}

	public float realtimeSinceStartup { get { return ((float)stopwatch_.ElapsedTicks) /  (float)System.Diagnostics.Stopwatch.Frequency; } }

	public Camera getCamera() { return camera_; }

	public void registUVScroller(UVScroller uv_scroller)
	{
		for (var i = 0; i < uv_scroller_list_.Length; ++i) {
			if (uv_scroller_list_[i] == null) {
				uv_scroller_list_[i] = uv_scroller;
				return;
			}
		}
		Debug.LogError("exceed UVScroller regist.");
		Debug.Assert(false);
	}

	public void registSound(DrawBuffer.SE se)
	{
		int updating_front = 1 - rendering_front_;
		draw_buffer_[updating_front].registSound(se);
	}

	/*
	 * 以下は MailThread
	 */

	// 入力更新
	private void input_update()
	{
		bool pause_pressed = false;
		if (!pause_) {
			bool pause_button = (Input.GetButtonDown("Cancel") ||
								 Input.GetMouseButton(0) ||
								 Input.touchCount > 0);
			if (!prev_pause_button_ && pause_button) {
				pause_pressed = true;
			}
			prev_pause_button_ = pause_button;
		}

		int[] buttons = InputManager.Instance.referButtons();
		buttons[(int)InputManager.Button.Horizontal] = (int)(Input.GetAxis("Horizontal") * 128f);
		buttons[(int)InputManager.Button.Vertical] = (int)(Input.GetAxis("Vertical") * 128f);
		buttons[(int)InputManager.Button.Fire] = (int)(Input.GetButton("Fire1") ? 1 : 0);
		buttons[(int)InputManager.Button.Pause] = pause_pressed ? 1 : 0;
		buttons[(int)InputManager.Button.Roll] = (int)(Input.GetAxis("rotate"));
		InputManager.Instance.flip();
	}

	// オブジェクト描画(SetActive)
	private void render(ref DrawBuffer draw_buffer)
	{
		if (player_prefab_ == null) { // テスト環境
			camera_.enabled = true;
			return;
		}

		// camera
		camera_.transform.position = draw_buffer.camera_transform_.position_;
		camera_.transform.rotation = draw_buffer.camera_transform_.rotation_;
		camera_.enabled = true;

		// player
		if (player_go_ != null) {
			player_go_.transform.position = draw_buffer.player_transform_.position_;
			player_go_.transform.rotation = draw_buffer.player_transform_.rotation_;
		}
		int zako_idx = 0;
		for (var i = 0; i < draw_buffer.object_num_; ++i) {
			switch (draw_buffer.object_buffer_[i].type_) {
				case DrawBuffer.Type.None:
					Debug.Assert(false);
					break;
				case DrawBuffer.Type.Zako:
					zako_pool_[zako_idx].SetActive(true);
					zako_pool_[zako_idx].transform.position = draw_buffer.object_buffer_[i].transform_.position_;
					zako_pool_[zako_idx].transform.rotation = draw_buffer.object_buffer_[i].transform_.rotation_;
					++zako_idx;
					break;
			}
		}
		for (var i = zako_idx; i < ZAKO_MAX; ++i) {
			zako_pool_[i].SetActive(false);
		}

		for (var i = 0; i < AUDIO_CHANNEL_MAX; ++i) {
			if (draw_buffer.se_[i] != DrawBuffer.SE.None) {
				switch (draw_buffer.se_[i]) {
					case DrawBuffer.SE.Bullet:
						audio_sources_bullet_[audio_source_bullet_index_].Play();
						++audio_source_bullet_index_;
						if (audio_source_bullet_index_ >= AUDIOSOURCE_BULLET_MAX) {
							audio_source_bullet_index_ = 0;
						}
						break;
					case DrawBuffer.SE.Explosion:
						audio_sources_explosion_[audio_source_explosion_index_].Play();
						++audio_source_explosion_index_;
						if (audio_source_explosion_index_ >= AUDIOSOURCE_EXPLOSION_MAX) {
							audio_source_explosion_index_ = 0;
						}
						break;
					case DrawBuffer.SE.Missile:
						audio_sources_missile_[audio_source_missile_index_].Play();
						++audio_source_missile_index_;
						if (audio_source_missile_index_ >= AUDIOSOURCE_MISSILE_MAX) {
							audio_source_missile_index_ = 0;
						}
						break;
					case DrawBuffer.SE.Lockon:
						audio_sources_lockon_[audio_source_lockon_index_].Play();
						++audio_source_lockon_index_;
						if (audio_source_lockon_index_ >= AUDIOSOURCE_LOCKON_MAX) {
							audio_source_lockon_index_ = 0;
						}
						break;

					case DrawBuffer.SE.VoiceIkuyo:
						audio_sources_voice_ikuyo_.Play();
						break;
					case DrawBuffer.SE.VoiceUwaa:
						audio_sources_voice_uwaa_.Play();
						break;
					case DrawBuffer.SE.VoiceSorosoro:
						audio_sources_voice_sorosoro_.Play();
						break;
					case DrawBuffer.SE.VoiceOtoto:
						audio_sources_voice_ototo_.Play();
						break;
					case DrawBuffer.SE.VoiceYoshi:
						audio_sources_voice_yoshi_.Play();
						break;
				}
				draw_buffer.se_[i] = DrawBuffer.SE.None;
			}
		}
	}

	private void unity_update()
	{
		beginPerformanceMeter2();

		if (player_prefab_ == null) { // テスト環境
			camera_.enabled = true;
			return;
		}

		// input phase
		input_update();

		//
		float render_time = update_time_ - dt_;
		render(ref draw_buffer_[rendering_front_]);
		Debris.Instance.render(rendering_front_, camera_, render_time);
		Spark.Instance.render(rendering_front_, camera_, render_time);
		Beam.Instance.render(rendering_front_);
		Trail.Instance.render(rendering_front_);
		if (explosion_material_ != null) {
			Explosion.Instance.render(rendering_front_, camera_, render_time);
		}
		Hahen.Instance.render(rendering_front_, render_time);
		for (var i = 0; i < uv_scroller_list_.Length; ++i) {
			if (uv_scroller_list_[i] == null) {
				break;
			}
			uv_scroller_list_[i].render(render_time);
		}

		if (pausemenu_canvas_go_ != null) {
			pausemenu_canvas_go_.SetActive(pause_);
		}

		// memory investigation
		used_heap_size_ = (long)UnityEngine.Profiling.Profiler.usedHeapSize;
		mono_heap_size_ = (long)UnityEngine.Profiling.Profiler.GetMonoHeapSize();
		mono_used_size_ = (long)UnityEngine.Profiling.Profiler.GetMonoUsedSize();
	}

	private void end_of_frame()
	{
		if (Time.deltaTime > 0) {
			++render_sync_frame_;
			if (!pause_) {
				++render_frame_;
			}
			// render_time_ = (render_frame_) * DT;
			rendering_front_ = 1 - rendering_front_; // flip
			manual_reset_event_.Set();
			stopwatch_.Start();
		} else {
			stopwatch_.Stop();
		}
	}

	public void beginPerformanceMeter()
	{
		render_start_tick_ = stopwatch_.ElapsedTicks;
	}
	public void endPerformanceMeter()
	{
		render_tick_ = stopwatch_.ElapsedTicks - render_start_tick_;
	}
	public void beginPerformanceMeter2()
	{
		render_start_tick2_ = stopwatch_.ElapsedTicks;
	}
	public void endPerformanceMeter2()
	{
		render_tick2_ = stopwatch_.ElapsedTicks - render_start_tick2_;
	}

	// The Update
	void Update()
	{
		render_tick3_ = stopwatch_.ElapsedTicks - render_start_tick_;
		unity_update();
		end_of_frame();
		render_tick4_ = stopwatch_.ElapsedTicks - render_start_tick_;
	}

	private void close_pause_menu()
	{
		System.GC.Collect();
		System.GC.WaitForPendingFinalizers();
		System.GC.Collect();
		pause_ = false;
	}

	public void onPauseMenuContinue()
	{
		close_pause_menu();
	}

	public void onPauseMenuAutoPlay()
	{
		Options.Instance.AutoPlay = !Options.Instance.AutoPlay;
		close_pause_menu();
	}

	public float onChangeFPS { get { return (float)getFPS()/12; } set { setFPS((int)value*12); } }

	public void onPauseMenuPMeter()
	{
		Options.Instance.PerformanceMeter = !Options.Instance.PerformanceMeter;
		close_pause_menu();
	}

}

} // namespace UTJ {
