using UnityEngine;
using System.Collections;
using System;

public class SackBehavior : MonoBehaviour
{
	public Vector2 speed = new Vector2(0, 0);

	public static int GroundTouches = 0;
	public static int Kicks = 0;
	public static int WindSpeed = 0;

	public static bool Active = true;
	public static bool OutOfScreen = false;
	
	private Vector2 windSpeed;

	private int _lastSecond = -1;
	private bool _kicked = false;

	// Use this for initialization
	void Start ()
	{
		
	}

	void Awake()
	{
		GroundTouches = 0;
		Kicks = 0;
		WindSpeed = 0;
		
		Active = true;
		OutOfScreen = false;

		_lastSecond = -1;
		_kicked = false;
		windSpeed = new Vector2 (0, 0);
	}

	// Update is called once per frame
	void Update ()
	{
		HandleInput ();

		UpdateWind ();

		float x = Camera.main.WorldToScreenPoint (transform.position).x;
		if (x < 0 || x > Screen.width)
			OutOfScreen = true;
	}

	void HandleInput()
	{
		if (Active && !_kicked && Camera.main.WorldToScreenPoint(transform.position).y < (Screen.height / 5))
		{
			for (var i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch (i);
				if (touch.phase == TouchPhase.Began)
				{
					float impact = ((Screen.height / 2f) / (Camera.main.WorldToScreenPoint(transform.position).y));
					rigidbody2D.velocity = new Vector2 (-(touch.position.x - (Screen.width / 2)) / (Screen.width / 4), -speed.y + 6 + impact);
					_kicked = true;
					Kicks++;
					break;
				}
			}
			
			if (!_kicked && Input.GetMouseButtonDown (0))
			{
				float impact = (((float)Screen.height / 2f) / (Camera.main.WorldToScreenPoint(transform.position).y));
				rigidbody2D.velocity = new Vector2 (-(Input.mousePosition.x - (Screen.width / 2)) / (Screen.width / 4), -speed.y + 6 + impact);
				Kicks++;
				_kicked = true;
			}
		}
		else
			_kicked = false;
	}

	void UpdateWind()
	{
		int sec = DateTime.Now.Second;
		if (sec % 15 == 0)
		{
			if (_lastSecond != sec)
			{
				windSpeed.x = 0.5f - UnityEngine.Random.value;
				WindSpeed = (int)(windSpeed.x * 10f);
				
				_lastSecond = sec;
			}
		}
		else
			_lastSecond = -1;

		if (Active)
		{
			Vector2 newVelocity = rigidbody2D.velocity;
			newVelocity.x -= windSpeed.x / 30;
			
			rigidbody2D.velocity = newVelocity;
		}
	}


	void OnCollisionEnter2D(Collision2D collision)
	{
		if(collision.gameObject.name == "Ground")
		{ 
			GroundTouches += 1;
		}
	}

	void FixedUpdate()
	{
		
	}
}
