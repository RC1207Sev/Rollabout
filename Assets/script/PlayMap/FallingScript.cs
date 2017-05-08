using UnityEngine;
using System.Collections;

public class FallingScript : MonoBehaviour {

	public Transform spawnPoint;

	public bool hasFailed;

	// Use this for initialization
	void Start () {
		hasFailed = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnTriggerEnter(Collider other) {
	
		hasFailed = true;

	}

	public void OnCollisionEnter(Collision collision) {

		Debug.Log("Entered Falling collision");
		hasFailed = true;
		//collision.rigidbody.velocity = Vector3.zero;
		//collision.transform.position = spawnPoint.position;

	}

}
