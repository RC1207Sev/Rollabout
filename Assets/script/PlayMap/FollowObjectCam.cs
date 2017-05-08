using UnityEngine;
using System.Collections;

public class FollowObjectCam : MonoBehaviour {

	public GameObject objToFollow;
	public int camX = 0;
	public int camY = 0;
	public int camZ = -5;
	public float rotationSpeed = 1;
	public float zoomSpeed = 1;
	public float rotationAmount;

	float zoom;

	Quaternion neededRotation;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {

		if (objToFollow != null)
		{
			//calculate the rotation needed 
			neededRotation = Quaternion.LookRotation(objToFollow.transform.position - this.transform.position);
			
			//use spherical interpolation over time 
			this.transform.rotation = Quaternion.Slerp(this.transform.rotation, neededRotation, Time.deltaTime * rotationSpeed);
			
			//this.transform.LookAt (objToFollow.transform.position);
			this.transform.position = objToFollow.transform.position - new Vector3(Mathf.Sin(rotationAmount) * 5, camY,Mathf.Cos (rotationAmount) * 5);

			zoom += Input.GetAxis("Mouse ScrollWheel");

			this.transform.Translate (0, 0, zoom * zoomSpeed);

			if(Input.GetKey(KeyCode.E))
			{
				rotationAmount += rotationSpeed / 100;
			}

			if(Input.GetKey(KeyCode.Q))
			{
				rotationAmount -= rotationSpeed / 100;
			}

		}

	
	}
}
