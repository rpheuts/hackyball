using UnityEngine;
using System.Collections;
using System;

public class HeartBehavior : MonoBehaviour {

	private int _lastSecond = -1;

	public ParticleSystem HitEffect;

	void Awake()
	{
		_lastSecond = -1;	
	}

	// Use this for initialization
	void Start ()
	{ 
		renderer.transform.position = GetNewLocation ();

		Debug.Log (renderer.transform.position.x + " " + renderer.transform.position.y);
	}
	
	// Update is called once per frame
	void Update ()
	{
		int sec = DateTime.Now.Second % 15;

		if (sec == 0)
		{
			if (sec != _lastSecond)
			{
				renderer.transform.position = GetNewLocation ();
				_lastSecond = sec;
			}
		}
		else
			_lastSecond = -1;
	}

	void OnTriggerEnter2D(Collider2D collider)
	{
		if(collider.gameObject.name == "Ball" && SackBehavior.Active)
		{ 
			if (SackBehavior.GroundTouches > 0)
				SackBehavior.GroundTouches--;

			instantiate(HitEffect, renderer.transform.position);

			renderer.transform.position = GetNewLocation ();;
		}
	}

	private ParticleSystem instantiate(ParticleSystem prefab, Vector3 position)
	{
		ParticleSystem newParticleSystem = Instantiate(
			prefab,
			position,
			Quaternion.identity
			) as ParticleSystem;
		
		// Make sure it will be destroyed
		Destroy(
			newParticleSystem.gameObject,
			newParticleSystem.startLifetime
			);
		
		return newParticleSystem;
	}

	Vector3 GetNewLocation()
	{
		float x = new Vector3((UnityEngine.Random.value * 10000) % (Screen.width - 25), 0f).x;
		float y = new Vector3(0f, ((UnityEngine.Random.value * 10000) + 100) % (Screen.height - 25)).y;

		x = (x < 25) ? 25 : x;
		y = (y < 25) ? 25 : y;

		Vector3 retVal = Camera.main.ScreenToWorldPoint(new Vector3(x, y, 0));
		retVal.z = -0.35f;

		return retVal;
	}
}
