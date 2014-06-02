using UnityEngine;
using System.Collections;

public class KickLineBehavior : MonoBehaviour {

	// Use this for initialization
	void Start ()
	{
		Vector3 vector = renderer.transform.position;
		vector.y = Camera.main.ScreenToWorldPoint (new Vector3 (0, Screen.height / 5)).y;
		renderer.transform.position = vector;
	}

	// Update is called once per frame
	void Update () {
	
	}
}
