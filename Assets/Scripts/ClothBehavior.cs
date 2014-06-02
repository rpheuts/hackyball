using UnityEngine;
using System.Collections;

public class ClothBehavior : MonoBehaviour {

	InteractiveCloth cloth;

	// Use this for initialization
	void Start () {
		cloth = GetComponent<InteractiveCloth> ();
	}
	
	// Update is called once per frame
	void Update ()
	{
		//HandleInput ();

		cloth.AddForceAtPosition (new Vector3 (2, 0, 0),
                                              new Vector3 (0.1f, 0.1f, 0),
                                              0.1f);

		Debug.Log (cloth);



	}

	void HandleInput()
	{
			for (var i = 0; i < Input.touchCount; i++)
			{
				Touch touch = Input.GetTouch (i);
				if (touch.phase == TouchPhase.Began)
				{
					((InteractiveCloth)this.GetComponent<InteractiveCloth>()).AddForceAtPosition(new Vector3(0, 0, 20),
						new Vector3(touch.position.x, touch.position.y, 0),
						0.1f);
				}
			}
			
			if (Input.GetMouseButtonDown (0))
			{
			Debug.Log (SackBehavior.Kicks);
				((InteractiveCloth)GetComponent<InteractiveCloth>()).AddForceAtPosition(new Vector3(0, 0, -5),
					new Vector3(2, 2, 0),
					1.0f);

			Debug.Log (GetComponent<InteractiveCloth>());
			}

		if (Input.GetMouseButtonDown(0)) {
			Debug.Log("Mouse down");
			Ray ray = Camera.main.ScreenPointToRay(new Vector3(Input.mousePosition.x, Input.mousePosition.y, 0));
			RaycastHit hit;
			if (Physics.Raycast(ray, out hit)) {
				Debug.Log("hit some object");
				if (hit.collider == gameObject.collider) {
					Debug.Log("Hit this object: " + hit.point);
					InteractiveCloth cloth = GetComponent<InteractiveCloth>();
					Debug.Log("Cloth = " + cloth);
					// Gets here, and cloth variable is a valid InteractiveCloth (there is only one in the scene)
					// hit.point looks to be exactly where the ray should intercept the cloth
					cloth.AddForceAtPosition(new Vector3(0,0,20), hit.point, 5);
					Debug.Log("Done applying force.");
				}
			}
		}
	}
}
