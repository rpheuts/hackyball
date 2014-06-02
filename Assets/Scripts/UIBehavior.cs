using UnityEngine;
using System.Collections;
using UnityEngine.SocialPlatforms;

public class UIBehavior : MonoBehaviour {

	public Font UIFont;

	private const int _lives = 5;
	private bool _scoreReported = false;

	private static bool _mainMenu;
	private static bool _restart;
	private Texture2D _menuBackground;
	private static ADBannerView banner = null;

	// Use this for initialization
	void Start ()
	{
		Application.targetFrameRate = 60;
		Social.localUser.Authenticate (ProcessAuthentication);
		
		if (!_restart)
			Time.timeScale = 0;
		_restart = false;
		
		_menuBackground = new Texture2D (Screen.width, Screen.height);
		Color pixel = new Color(0f, 0f, 0f, 0.5f);
		for (int j=_menuBackground.height; j>=0; j--)
			for (int i=_menuBackground.width; i>=0; i--)
				_menuBackground.SetPixel(i, j, pixel);
		_menuBackground.Apply ();
	}

	void Awake()
	{
		_scoreReported = false;
	}

	void OnGUI ()
	{
		Texture2D tex = (Texture2D)Resources.Load("heart");

		for (int i=SackBehavior.GroundTouches; i<_lives; i++)
		{
			GUI.DrawTexture(new Rect (((_lives - 1) * 25) - (i * 25), 2, 25, 25), tex);
		}

		GUI.skin.font = UIFont;
		GUIStyle style = new GUIStyle ();
		style.fontSize = 30;
		style.normal.textColor = new Color (0.18f, 1f, 0f);

		GUI.Label(new Rect((Screen.width) - 40, 4, 20, 10), SackBehavior.Kicks.ToString(), style);

		if (SackBehavior.GroundTouches >= _lives || SackBehavior.OutOfScreen)
		{
			SackBehavior.Active = false;

			if (!_scoreReported)
			{
				Social.ReportScore(SackBehavior.Kicks, "HackyBallTotal", ProcessScoreReport);
				_scoreReported = true;
			}

			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), _menuBackground, GUIStyle.none);
			float buttonY = (Screen.height / 2) - ((Screen.width / 6) / 2);

			Texture2D gameOverButton = (Texture2D)Resources.Load("GameOverButton");
			GUI.Button(new Rect((Screen.width / 2) - ((Screen.width / 1.5f) / 2),
			                        buttonY - ((Screen.width / 6) * 0.5f),
			                        (Screen.width / 1.5f),
			                    	(Screen.width / 6)), gameOverButton, GUIStyle.none);

			style.fontSize = 50;
			
			GUI.Label(new Rect((Screen.width) - (Screen.width / 2) - 120, buttonY + (Screen.width / 6) * 0.75f, 20, 10), "Score: " + SackBehavior.Kicks.ToString(), style);

			Texture2D mainButton = (Texture2D)Resources.Load("PlayButton");
			if (GUI.Button(new Rect((Screen.width / 2) - ((Screen.width / 3) / 2),
			                        buttonY + ((Screen.width / 6) * 1.5f),
			                        (Screen.width / 3),
			                        (Screen.width / 6)), mainButton, GUIStyle.none))
			{
				_restart = true;
				Time.timeScale = 1;
				Application.LoadLevel(0);
			}
			
			Texture2D scoreButton = (Texture2D)Resources.Load("ScoreButton");
			if (GUI.Button(new Rect((Screen.width / 2) - ((Screen.width / 3) / 2),
			                        buttonY + ((Screen.width / 6) * 2.5f) + 5,
			                        (Screen.width / 3),
			                        (Screen.width / 6)), scoreButton, GUIStyle.none))
			{
				Social.ShowLeaderboardUI();
			}

			if (banner == null)
			{
				banner = new ADBannerView(ADBannerView.Type.Banner, ADBannerView.Layout.BottomCenter);
			}
			else
				banner.visible = true;
		}
		else if (Time.timeScale == 0)
		{
			GUI.Box(new Rect(0, 0, Screen.width, Screen.height), _menuBackground, GUIStyle.none);
			float buttonY = (Screen.height / 2) - ((Screen.width / 6) / 2);

			Texture2D mainButton = (Texture2D)Resources.Load("PlayButton");
			if (GUI.Button(new Rect((Screen.width / 2) - ((Screen.width / 3) / 2),
			                        buttonY,
			                        (Screen.width / 3),
			                        (Screen.width / 6)), mainButton, GUIStyle.none))
			{
				_restart = true;
				Time.timeScale = 1;
				Application.LoadLevel(0);
			}

			Texture2D scoreButton = (Texture2D)Resources.Load("ScoreButton");
			if (GUI.Button(new Rect((Screen.width / 2) - ((Screen.width / 3) / 2),
			                        buttonY + (Screen.width / 6) + 5,
			                        (Screen.width / 3),
			                        (Screen.width / 6)), scoreButton, GUIStyle.none))
			{
				Social.ShowLeaderboardUI();
			}

			if (banner == null)
			{
				banner = new ADBannerView(ADBannerView.Type.Banner, ADBannerView.Layout.BottomCenter);
				ADBannerView.onBannerWasLoaded  += OnBannerLoaded;
			}
			else
				banner.visible = true;
		}
		else if (banner != null)
			banner.visible = false;
	}

	void ProcessScoreReport(bool success)
	{
		if (success) {
			Debug.Log ("Score reported");
		}
		else
			Debug.Log ("Failed to report score");
	}

	void ProcessAuthentication (bool success) {
		if (success) {
			Debug.Log ("Authenticated");
		}
		else
			Debug.Log ("Failed to authenticate");
	}

	void ProcessScores(bool success) {}

	void OnBannerLoaded()
	{
		banner.visible = true;
	}

}
