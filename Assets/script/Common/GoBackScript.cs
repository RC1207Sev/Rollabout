using UnityEngine;
using System.Collections;

// This script will bring the player to the previous scene on ESC or "back" button on smartphones.
// In case the user is in the entry point (scene 0), it will just exit game.

public class GoBackScript : MonoBehaviour {

	public string lastScene;

	// Use this for initialization
	void Start () {

		lastScene = PlayerPrefs.GetString("LastSceneName");
		PlayerPrefs.SetString("LastSceneName", Application.loadedLevelName);
	
	}
	
	// Update is called once per frame
	void Update () {

		if(Input.GetKeyDown("escape"))
		{
            
            if (Application.loadedLevel == 0)
                // exit game
                Application.Quit();
            else
                // load previous scene
			    Application.LoadLevel(lastScene);
		}
	
	}
}
