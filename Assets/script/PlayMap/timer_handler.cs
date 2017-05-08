using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;
using MyUtils;

public class timer_handler : MonoBehaviour {

	DateTime raceTimer;
	DateTime tempTimer;
	public TimeSpan CurrentTime;
	Text time_value;

	bool isRunning = false;

	public GameObject timeLabel; // to be filled in Unity
	public int minutes = 0;
	public int seconds = 0;

	// Use this for initialization
	void Start () {

		Transform totalTimeLabel;

		totalTimeLabel = SearchTools.FindInChildsTree("l_total_time", this.transform);
		totalTimeLabel.GetComponent<Text>().text = "Total Time: 0:00:000";
		time_value = timeLabel.GetComponent<Text>();
		raceTimer = new DateTime();
		//RestartRace();

	}

	public void RestartRace()
	{
		raceTimer = DateTime.Now;
		isRunning = true;
	}

	public void StopTimer()
	{
		isRunning = false;
	}
	
	// Update is called once per frame
	void FixedUpdate () {

		if (isRunning)
		{
			CurrentTime = DateTime.Now - raceTimer;
			time_value.text = CurrentTime.Minutes + ":" + CurrentTime.Seconds + ":" + CurrentTime.Milliseconds;
		}
	

	}
}
