using UnityEngine;
using System.Collections;

public class ScoreboardHandler : MonoBehaviour {

	public GameObject SingleScorePrefab;

	GameObject temp;

	// Use this for initialization
	void Start () {
	
		ClearScoreBoard ();

	}

	public void AddScore(int pos, string name, int milliseconds)
	{
		temp = Instantiate (SingleScorePrefab);
		temp.transform.SetParent (this.transform);

		temp.SendMessage ("SetPosition", pos.ToString());
		temp.SendMessage ("SetUserName", name);
		if (milliseconds > 0)
			temp.SendMessage ("SetTime", milliseconds);

	}

	public void ClearScoreBoard()
	{
		// clear screen from previous researches
		foreach (Transform t in this.transform)
		{
			Destroy (t.gameObject);
		}
	}

}
