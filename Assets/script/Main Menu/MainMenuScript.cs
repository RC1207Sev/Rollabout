using UnityEngine;
using System.Collections;

public class MainMenuScript : MonoBehaviour {

	public string newGameSceneName;

	public GameObject MapListUI;

	// Use this for initialization
	void Start () {

        // TODO: handling user login
        PlayerPrefs.SetInt("user_id", 0);


    }
	
	// Update is called once per frame
	void Update () {
	
	}

	public void LoadScene(string sceneName)
	{
		PlayerPrefs.SetString("LastSceneName", Application.loadedLevelName);
		Application.LoadLevel (sceneName);

	}

	public void OpenMapListTab()
	{
		MapListUI.SetActive(true);
	}

}
