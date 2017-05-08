using UnityEngine;
using System.Collections;
using System;
using UnityEngine.UI;

public class MapsLoader : MonoBehaviour {

	public string LoadMapList = "http://rollabout.azurewebsites.net/getmaplist.php";

    public string LoadMapImagesRoot = "http://rollabout.azurewebsites.net/images/maps/";

    public GameObject mapsContainer;

	public GameObject mapPanelPrefab;

	// Use this for initialization
	void Start () {
	
	}

	void Awake() {

		StartCoroutine(ListLoader());
	}

	IEnumerator ListLoader()
	{
		WWWForm form = new WWWForm();
		
		string[] AllLevels;
		string[] singleField;

		GameObject temp;

		// clear screen from previous researches
		foreach (Transform t in mapsContainer.transform)
		{
			Destroy (t.gameObject);
		}
		
		// Download from a php script
		WWW w = new WWW(LoadMapList);
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			Debug.Log(w.error);
		}
		else {
			Debug.Log("Finished Loading Maps");
		}
		
		Debug.Log(w.text);

		AllLevels = w.text.Split ('|');

		foreach(string level in AllLevels)
		{
			singleField = level.Split(';');

			temp = Instantiate(mapPanelPrefab);
			temp.SendMessage ("SetName", singleField[1]);
			temp.SendMessage ("SetId", Int32.Parse(singleField[0]));

			if (singleField[8] != "")
				temp.BroadcastMessage ("SetTime", Int32.Parse(singleField[9]));

			temp.transform.SetParent(mapsContainer.transform);
            StartCoroutine(GetMapImage(Int32.Parse(singleField[0]), temp));

			yield return new WaitForEndOfFrame();

		}

	}

    IEnumerator GetMapImage(int mapID, GameObject pMapPanel)
    {

        // Start a download of the given URL
        WWW www = new WWW(LoadMapImagesRoot + mapID + ".jpg");

        // Wait for download to complete
        yield return www;

        // assign texture
        pMapPanel.SendMessage("SetImage", Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0.5f, 0.5f)));

    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
