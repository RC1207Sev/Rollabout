using UnityEngine;
using System.Collections;

public class grassMovingScript : MonoBehaviour {

	public float power = 0.1f;

	Transform rootObj;

	float randomDelay;

	// Use this for initialization
	void Start () {

		rootObj = this.transform.Find ("root");
		randomDelay = Random.Range (0,500);

	}
	
	// Update is called once per frame
	void Update () {

		foreach (Transform leaf in this.transform)
		{
			if(leaf.name != "root")
			{
				leaf.RotateAround (rootObj.position, leaf.up, power * Mathf.Sin (Time.time + randomDelay));
			}
		}
	
	}
}
