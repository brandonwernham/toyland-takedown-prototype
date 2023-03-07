using UnityEngine;
using System.Collections;

public class Rotate : MonoBehaviour {

	public float speed;
	float timeToDestroy = 10f;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.up * Time.deltaTime * speed * 10);

		timeToDestroy -= Time.deltaTime;
		if (timeToDestroy <= 0) {
			Destroy(gameObject);
		}
	}
}
