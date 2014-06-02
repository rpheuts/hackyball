using UnityEngine;
using System.Collections;

public class CloudBehavior : MonoBehaviour {

	private float speed;

	void Awake()
	{

	}

	// Use this for initialization
	void Start ()
	{
		speed = ((Random.value + 0.2f) / 120);

		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();
		renderer.transform.position = new Vector3 (
			Camera.main.ScreenToWorldPoint(new Vector3(Screen.width, 0)).x - Random.value * 2.5f,
			Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y - Random.value * 2.5f,
			-0.35f
		);
	}
	
	// Update is called once per frame
	void Update ()
	{
		SpriteRenderer renderer = gameObject.GetComponent<SpriteRenderer>();


		if (renderer.transform.position.x < Camera.main.ScreenToWorldPoint(new Vector3(-renderer.sprite.texture.width, 0)).x)
		{
			renderer.transform.position = new Vector3 (
				Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + (renderer.sprite.texture.width / 2), 0)).x,
				Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y - Random.value * 2.5f,
				-0.35f
			);
		}
		else if (renderer.transform.position.x > Camera.main.ScreenToWorldPoint(new Vector3(Screen.width + renderer.sprite.texture.width, 0)).x)
		{
			renderer.transform.position = new Vector3 (
				Camera.main.ScreenToWorldPoint(new Vector3(-(renderer.sprite.texture.width / 2), 0)).x,
				Camera.main.ScreenToWorldPoint(new Vector3(0, Screen.height)).y - Random.value * 2.5f,
				-0.35f
				);
		}
		else
		{
			if (SackBehavior.WindSpeed >= 0)
				renderer.transform.position = new Vector3 (renderer.transform.position.x - (speed + (SackBehavior.WindSpeed / 5)), renderer.transform.position.y, -0.35f);
			else
				renderer.transform.position = new Vector3 (renderer.transform.position.x + (speed + (SackBehavior.WindSpeed / 5)), renderer.transform.position.y, -0.35f);	
		}
	}
}
