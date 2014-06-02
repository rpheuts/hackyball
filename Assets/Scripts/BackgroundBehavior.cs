using UnityEngine;
using System.Collections;
using System;



public class BackgroundBehavior : MonoBehaviour
{
	private static float DAYNIGHT_CYCLE_TIME = 30;
	private static float DAYNIGHT_CYCLE_FREQ = 5;

	private Sprite _currentBackground;

	private Color _currentTopColor;
	private Color _currentBottomColor;

	private Color _dayBottomColor = new Color(0.486f, 0.796f, 0.98f);
	private Color _dayTopColor = new Color(0.0f, 0.518f, 0.91f);

	private Color _nightBottomColor = new Color(0.067f, 0.039f, 0.275f);
	private Color _nightTopColor = new Color(0.0f, 0.0f, 0.1f);

	private Color _dayNightDiffTop;
	private Color _dayNightDiffBottom;
	private int _dayNightSeconds = -1;
	private Boolean _day = true;

	void Awake()
	{
		_currentTopColor = _dayTopColor;
		_currentBottomColor = _dayBottomColor;
		
		_dayNightDiffTop = (_dayTopColor - _nightTopColor) / DAYNIGHT_CYCLE_TIME;
		_dayNightDiffBottom = (_dayBottomColor - _nightBottomColor) / DAYNIGHT_CYCLE_TIME;
		
		UpdateGradient (_dayTopColor, _dayBottomColor);
	}

	// Use this for initialization
	void Start ()
	{
		_currentTopColor = _dayTopColor;
		_currentBottomColor = _dayBottomColor;

		_dayNightDiffTop = (_dayTopColor - _nightTopColor) / DAYNIGHT_CYCLE_TIME;
		_dayNightDiffBottom = (_dayBottomColor - _nightBottomColor) / DAYNIGHT_CYCLE_TIME;

		UpdateGradient (_dayTopColor, _dayBottomColor);
	}

	// Update is called once per frame
	void Update ()
	{
		int seconds = DateTime.Now.Second % (int)DAYNIGHT_CYCLE_FREQ;
		
		if (seconds == 0)
		{
			if (_dayNightSeconds != seconds)
			{
				if (_day)
				{
					_currentTopColor -= _dayNightDiffTop;
					_currentBottomColor -= _dayNightDiffBottom;
				}
				else
				{
					_currentTopColor += _dayNightDiffTop;
					_currentBottomColor += _dayNightDiffBottom;
				}
				
				UpdateGradient(_currentTopColor, _currentBottomColor);
				
				_dayNightSeconds = seconds;
				
				if (_currentTopColor.b < _nightTopColor.b)
					_day = false;
				if (_currentTopColor.b > _dayTopColor.b)
					_day = true;
			}
		}
		else
			_dayNightSeconds = -1;
	}

	private void UpdateGradient(Color top, Color bottom)
	{
		Vector3 screenPoint = Camera.main.ScreenToWorldPoint (new Vector3 (Screen.width * 2, Screen.height * 2));
		
		Texture2D tex = new Texture2D((int)screenPoint.x, (int)screenPoint.y);
		Color gradient = top;
		Color diff = (top - bottom) / tex.height; 

		for (int j=tex.height; j>=0; j--)
		{
			for (int i=tex.width; i>=0; i--)
			{
				Color pixel = new Color(gradient.r, gradient.g, gradient.b, 1.0f);
				tex.SetPixel(i, j, pixel);
			}

			gradient -= diff;
		}

		tex.Apply ();
		
		Sprite sprite = Sprite.Create(tex, new Rect(0, 0, screenPoint.x, screenPoint.y),new Vector2(0, 0), 1.0f);

		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.sprite = sprite;
		renderer.transform.position = new Vector3 ((int)-screenPoint.x / 2, (int)-screenPoint.y / 2);
	}
}
