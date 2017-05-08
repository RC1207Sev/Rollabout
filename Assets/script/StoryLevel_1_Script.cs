using UnityEngine;
using System.Collections;

public class StoryLevel_1_Script : MonoBehaviour {

	public GameObject failTrigger;
	public GameObject successTrigger;
	public GameObject gameUI;

	public Transform spawnPoint;

	FallingScript fallingCheck;
	WinningScript winningCheck;

	Rigidbody theBall;


	// Use this for initialization
	void Start () {

		if (failTrigger == null)
		{
			failTrigger = GameObject.Find ("FallingTrigger");
			successTrigger = GameObject.Find ("WinningTrigger");
			gameUI = GameObject.Find ("GameUI");
		}

		fallingCheck = failTrigger.GetComponent<FallingScript>();
		winningCheck = successTrigger.GetComponent<WinningScript>();
		theBall = GameObject.Find ("Ball").GetComponent<Rigidbody>();
		theBall.gameObject.SetActive(false);
	
	}

	public void ready_button_clicked()
	{
		theBall.position = spawnPoint.position;
		gameUI.BroadcastMessage ("RestartRace", SendMessageOptions.DontRequireReceiver);
		gameUI.BroadcastMessage ("UIToggle", SendMessageOptions.DontRequireReceiver);
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if(fallingCheck.hasFailed)
		{
			Debug.Log("Falling detected");
			theBall.isKinematic = true;
			theBall.transform.position = spawnPoint.position;
			fallingCheck.hasFailed = false;
			gameUI.BroadcastMessage ("RelativeLives",-1, SendMessageOptions.DontRequireReceiver);
			theBall.isKinematic = false;
		}
		if(winningCheck.hasWin)
		{
			gameUI.BroadcastMessage ("StopTimer", SendMessageOptions.DontRequireReceiver);
			Debug.Log ("You Win!");
			gameUI.BroadcastMessage ("UIToggle", SendMessageOptions.DontRequireReceiver);
			theBall.isKinematic = false;
			winningCheck.hasWin = false;
			theBall.transform.position = spawnPoint.position;
		}
			
			
	}
}
