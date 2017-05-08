using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class SingleScoreScript : MonoBehaviour {

	public Text position;
	public Text username;
	public Text time;


	// Use this for initialization
	void Start () {
	
	}

	public void SetPosition(string positionNumber)
	{
		position.text = positionNumber;
	}

	public void SetUserName(string name)
	{
		username.text = name;
	}

	public void SetTime(int milliseconds)
	{
		TimeSpan temp = new TimeSpan(0,0,0,0,milliseconds);
		time.text = temp.Minutes + ":" + zeroFilling(2, temp.Seconds) + ":" + zeroFilling(3, temp.Milliseconds);
	}

	private string zeroFilling(int numberOfDigits, int value)
	{
		
		string temp = value + "";
		for (int i=temp.Length;i<numberOfDigits;i++)
		{
			temp = "0" + temp;
		}
		
		return temp;
		
	}

}
