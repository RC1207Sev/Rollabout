using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class MapNameLabelScript : MonoBehaviour {

	public Text mapNameLabel;

	// Use this for initialization
	void Start () {
	
	}

	public void SetName(string MapName)
	{
		mapNameLabel.text = MapName;
	}

}
