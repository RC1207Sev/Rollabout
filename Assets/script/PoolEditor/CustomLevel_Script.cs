using UnityEngine;
using System.Collections;

public class CustomLevel_Script : MonoBehaviour {

	public FileHandler fileLoader;

	public GameObject failTrigger;
	public GameObject successTrigger;
	public GameObject gameUI;
	public timer_handler timerHandler;

	public Transform spawnPoint;

	public FallingScript fallingCheck;
	public WinningScript winningCheck;

	public Rigidbody theBall;

	public bool isInitialized = false;  // must be changed to true ONLY when failTrigger, successTrigger, gameUI, spawnPoint and theBall are initialized


	// Use this for initialization
	void Start () {

		timerHandler = gameUI.GetComponentInChildren<timer_handler> ();
		StartCoroutine (fileLoader.LoadWWWMap(PlayerPrefs.GetInt("map_id"), PlayerPrefs.GetInt("user_id")));
		fileLoader.LoadHighScores ();

	}

	public void ready_button_clicked()
	{
		Debug.Log ("Ready clicked");
		theBall.position = spawnPoint.position;
		theBall.isKinematic = true;
		theBall.isKinematic = false;
		gameUI.BroadcastMessage ("RestartRace", SendMessageOptions.DontRequireReceiver);
		gameUI.BroadcastMessage ("AbsoluteLifes", 3,  SendMessageOptions.DontRequireReceiver);
		gameUI.BroadcastMessage ("UIToggle", SendMessageOptions.DontRequireReceiver);

		// wake up all pieces
		foreach (GameObject singlePiece in GameObject.FindGameObjectsWithTag("MapPiece"))
		{
			singlePiece.SendMessage("Wakeup", SendMessageOptions.DontRequireReceiver);
		}

	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (isInitialized)
		{
			if(fallingCheck.hasFailed)
			{
				Debug.Log("Falling detected");
				theBall.isKinematic = true;
				theBall.transform.position = spawnPoint.position;
				fallingCheck.hasFailed = false;
				gameUI.BroadcastMessage ("RelativeLifes",-1, SendMessageOptions.DontRequireReceiver);
				theBall.isKinematic = false;
			}
			if(winningCheck.hasWin)
			{
				gameUI.BroadcastMessage ("StopTimer", SendMessageOptions.DontRequireReceiver);
				Debug.Log ("You Win!");
				gameUI.BroadcastMessage ("UIToggle", SendMessageOptions.DontRequireReceiver);
				gameUI.BroadcastMessage ("AbsoluteLifes", 3,  SendMessageOptions.DontRequireReceiver);
				theBall.isKinematic = false;
				winningCheck.hasWin = false;
				theBall.transform.position = spawnPoint.position;

				// save time in highscores
				fileLoader.SaveHighScore((int) timerHandler.CurrentTime.TotalMilliseconds);
				// Reload HighScores
				fileLoader.LoadHighScores ();


			}
		}
	
			
	}
}
