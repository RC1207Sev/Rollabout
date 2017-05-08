using UnityEngine;
using System.Collections;

public class WinningScript : MonoBehaviour {

	public bool hasWin;

	// Use this for initialization
	void Start () {
		hasWin = false;
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	
	void OnCollisionEnter(Collision collision) {

		hasWin = true;
		Debug.Log("Entered Winning collision");

	}

	void OnCollisionExit(Collision collision) {
		
		hasWin = false;
		Debug.Log("Exiting Winning collision");
		
	}



}
