using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class lifes_handler : MonoBehaviour {

	public GameObject livesLabel; // must be filled in Unity
	public int lifes;
	Text lifes_value;
	int currentlives;

	// Use this for initialization
	void Start () {
	
		lifes_value = livesLabel.GetComponent<Text>();
		lifes_value.text = lifes + "";
		currentlives = lifes;

	}

	// Update is called once per frame
	void Update () {
	
	}

	public void RelativeLifes(int delta)
	{
		currentlives += delta;
		lifes_value.text = currentlives + "";
		if (currentlives == 0)
			EndGame();
	}

	public void AbsoluteLifes(int delta)
	{
		currentlives = delta;
		lifes_value.text = currentlives + "";
	}

	public void EndGame()
	{
		currentlives = lifes;
		this.BroadcastMessage("StopTimer");
		this.BroadcastMessage("UIToggle");
	}

}
