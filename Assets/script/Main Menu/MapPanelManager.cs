using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class MapPanelManager : MonoBehaviour {

	public Text mapName;
	public Text mapTime;
    public Image mapImageField;

    public int map_id;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void SetName(string name)
	{
		mapName.text = name;
	}

	public void SetTime(int milliseconds)
	{

		TimeSpan temp = new TimeSpan(0,0,0,0,milliseconds);
		mapTime.text = "Best Time: " + temp.Minutes + ":" + zeroFilling(2, temp.Seconds) + ":" + zeroFilling(3, temp.Milliseconds);

	}

	public void SetId(int id)
	{
		map_id = id;
	}

	public void LoadSceneOnClick()
	{
		// pass the map_id value to the next scene

		PlayerPrefs.SetInt("map_id", map_id);
		Application.LoadLevel("CustomLevel");
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

    public void SetImage(Sprite pImg)
    {
        mapImageField.sprite = pImg;
    }

}
