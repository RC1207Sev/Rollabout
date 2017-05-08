using UnityEngine;
using System.Collections;
using System.IO;
using System;
using System.Collections.Generic;
using UnityEngine.Events;

public class FileHandler : MonoBehaviour {

	private struct piece
	{
		public Vector3 position;
		public Quaternion rotation;
		public string pieceType;
		public string pieceParams;

	}

	List<piece> allPieces;

	public GameObject ball;

	public CustomLevel_Script mainScript;
	
	public GameObject UI;

	public GameObject Maincam;

    public GameObject screenshotCam;

    public ScoreboardHandler HighScoreContainer;

	public GameObject[] piecesGO;

	public string AddMapPage = "http://rollabout.azurewebsites.net/addmap.php";

	public string LoadMapPage = "http://rollabout.azurewebsites.net/loadmap.php";

	public string AddScorePage = "http://rollabout.azurewebsites.net/addhighscore.php";

	public string GetScoresOfMap = "http://rollabout.azurewebsites.net/loadhighscore.php";

    public string getUserInfoPhp = "http://rollabout.azurewebsites.net/getuserinfo.php";

    public string saveScreenshotPhp = "http://rollabout.azurewebsites.net/addscreenshot.php";

    GameObject spawnPoint, winningPoint, failTrigger, winningTrigger, instantiatedBall;

	private string MapName;

	public int mapID = 0;

	private DialogManager dm;

	// Use this for initialization
	void Start () {

		allPieces = new List<piece>();
		MapName = "";

        piecesGO = Resources.LoadAll<GameObject>("Prefabs/Structures");

    }

	public void addPiece(Vector3 position, Quaternion rotation, string pieceType, string pieceParams)
	{
		piece tmp = new piece();

		tmp.position = position;
		tmp.rotation = rotation;
		tmp.pieceType = pieceType;
		tmp.pieceParams = pieceParams;

		allPieces.Add (tmp);

	}

	public void RemovePiece(GameObject passedPiece)
	{

		piece tmp = new piece();

		tmp.position = passedPiece.transform.position;

		foreach(piece p in allPieces)
		{
			if (Vector3.Equals(p.position, tmp.position))
			{
				allPieces.Remove(p);
				return;
			}
			    
		}

	}


	// Use this for initialization
	public void SaveMap (string fileName = "") {

		String singleLine;
		List<String> AllLines = new List<String>();

		UnityAction OkSetName, cancelSetName;

		if (MapName == "")
		{
			OkSetName = new UnityAction(SetMapName);
			cancelSetName = new UnityAction(CancelButtonAction);

			dm = DialogManager.Instance ();
			dm.Choice ("Save Map","Insert your map name: \n (max 40 alphanumerical characters)",new string[] {"b_ok","b_cancel"}, new UnityAction[] {OkSetName, cancelSetName}, true);

		}else
		{
			foreach(piece p in allPieces)
			{
				singleLine = p.position.x + ";";
				singleLine += p.position.y + ";";
				singleLine += p.position.z + ";";
				singleLine += p.rotation.x + ";";
				singleLine += p.rotation.y + ";";
				singleLine += p.rotation.z + ";";
				singleLine += p.rotation.w + ";";
				singleLine += p.pieceType + ";";
				singleLine += p.pieceParams;
				AllLines.Add (singleLine);
			}

			StartCoroutine (SendToWeb(AllLines, MapName));
			//MapName = "";
		}

	}


	// Gets Map Name from a Modal Dialog
	public void SetMapName()
	{
		dm = DialogManager.Instance ();
		MapName = dm.inputText.text;
		UI.BroadcastMessage("SetName", MapName, SendMessageOptions.DontRequireReceiver);
		SaveMap (MapName);
	}

	public void CancelButtonAction(){
		// does nothing
	}

	public IEnumerator LoadWWWMap (int map_id, int user_id) {

        WWW w;
        WWWForm form = new WWWForm();

		string[] fileLevel;
		string[] tempResult;
        string[] ballParams;

        int tempx = 0, tempy = 0, tempz = 0;
        float temprx = 0, tempry = 0, temprz = 0, temprw = 1;

        bool header = true;
		
		piece tempPiece;
		
		GameObject tempGO;

		mapID = map_id;

		// Load map from server

		form.AddField("map_id", map_id.ToString());
		
		// Upload to a php script
		w = new WWW(LoadMapPage, form);
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			Debug.Log(w.error);
		}
		else {
			Debug.Log("Finished Downloading Map pieces");
		}

		Debug.Log(w.text);


		allPieces = new List<piece> ();

		fileLevel = w.text.Split('|');
		
		foreach(string line in fileLevel)
		{
			
			tempResult = line.Split(';');
			
			tempPiece = new piece();
			
			for (int i=0;i<tempResult.Length;i++)
			{
				switch (i)
				{
				case 0:
					tempx =  Int32.Parse(tempResult[i]);
					break;
				case 1:
					tempy = Int32.Parse(tempResult[i]);
					break;
				case 2:
					tempz = Int32.Parse(tempResult[i]);
					break;
				case 3:
					temprx = float.Parse(tempResult[i]);
					break;
				case 4:
					tempry = float.Parse(tempResult[i]);
					break;
				case 5:
					temprz = float.Parse(tempResult[i]);
					break;
				case 6:
					temprw = float.Parse(tempResult[i]);
					break;
				case 7:
					tempPiece.pieceType = tempResult[i];
					break;
				case 8:
					tempPiece.pieceParams = tempResult[i];
					break;
				default:
					Debug.Log("WARNING: wrong cell number in csv single line: " + i);
					break;
				}
			}
			
			tempPiece.position = new Vector3(tempx, tempy, tempz); 

			tempPiece.rotation = new Quaternion(temprx, tempry, temprz, temprw);
			
			allPieces.Add (tempPiece);
			
		}
		
		Debug.Log ("File loaded, found " + allPieces.Count + " pieces");
		
		Debug.Log ("Loading Level pieces");
		
		foreach(piece p in allPieces)
		{
			Debug.Log("Creating piece type " + p.pieceType + " in position " + p.position.ToString());
			tempGO = GetGOfromList(p.pieceType);
			Instantiate(tempGO,p.position,p.rotation).name = tempGO.name;
		}
		
		Debug.Log ("All pieces loaded");
		
		Debug.Log ("Checking Spawn Point");
		
		//spawnPoint = GameObject.Find ("spawn_point_terrain");
        spawnPoint = GameObject.FindGameObjectWithTag("SpawnPoint");

        if (spawnPoint == null)
			Debug.LogError("ERROR: no Spawn Point Found");
		
		Debug.Log ("Checking Winning Point");
		
		winningTrigger = GameObject.Find ("winning_point");

        winningTrigger = GameObject.FindGameObjectWithTag("WinTrigger");

        if (winningTrigger == null)
			Debug.LogError("ERROR: no Winning Point Found");
		
		Debug.Log ("Creating Ball and assign to main camera");

        // Get user Ball's params

        form.AddField("user_id", user_id.ToString());

        // Upload to a php script
        w = new WWW(getUserInfoPhp, form);
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.Log(w.error);
        }
        else
        {
            Debug.Log("Finished Downloading Ball params");
        }

        Debug.Log(w.text);

        ballParams = w.text.Split(';');

        instantiatedBall = (GameObject)Resources.Load("Prefabs/Spheres/" + ballParams[0], typeof(GameObject));
        instantiatedBall = (GameObject)Instantiate(instantiatedBall, spawnPoint.transform.position, Quaternion.identity);
		Maincam.GetComponent<FollowObjectCam> ().objToFollow = instantiatedBall;
		
		failTrigger = GameObject.Find ("FallingTrigger");
		
		failTrigger.GetComponent<FallingScript> ().spawnPoint = spawnPoint.transform;
		
		Debug.Log ("Assigning data to main script");
		
		mainScript.failTrigger = failTrigger;
		
		mainScript.fallingCheck = failTrigger.GetComponent<FallingScript> ();
		
		mainScript.spawnPoint = spawnPoint.transform;
		
		mainScript.theBall = instantiatedBall.GetComponent<Rigidbody> ();
		
		mainScript.successTrigger = winningTrigger;
		
		mainScript.winningCheck = mainScript.successTrigger.GetComponent<WinningScript> ();
		
		mainScript.isInitialized = true;
		
		Debug.Log ("Setting up UI");
		
		UI.BroadcastMessage ("SetPlayer", instantiatedBall,SendMessageOptions.RequireReceiver);
		
		instantiatedBall.SetActive (false);
		
		Debug.Log ("Map loaded, game can start");

	}

	// Use this for initialization
	public void LoadMap (string fileName) {

		string[] fileLevel;
		string[] tempResult;

		int tempx = 0, tempy = 0, tempz = 0;

		bool header = true;

		piece tempPiece;

		GameObject tempGO;

		string result = Path.GetTempPath();

		Debug.Log(result + "map.csv");

		if (fileName == null)
			fileLevel = File.ReadAllLines (result + "map.csv");
		else
			fileLevel = File.ReadAllLines (fileName);

		allPieces = new List<piece> ();


		foreach(string line in fileLevel)
		{
			// skipping the header row
			if (header)
			{
				header = false;
				continue;
			}

			tempResult = line.Split(';');

			tempPiece = new piece();

			for (int i=0;i<tempResult.Length;i++)
			{
				switch (i)
				{
				case 0:
					tempx =  Int32.Parse(tempResult[i]);
					break;
				case 1:
					tempy = Int32.Parse(tempResult[i]);
					break;
				case 2:
					tempz = Int32.Parse(tempResult[i]);
					break;
				case 3:
					tempPiece.pieceType = tempResult[i];
					break;
				case 4:
					tempPiece.pieceParams = tempResult[i];
					break;
				default:
					Debug.Log("WARNING: wrong cell number in csv single line: " + i);
					break;
				}
			}

			tempPiece.position = new Vector3(tempx, tempy, tempz); 

			allPieces.Add (tempPiece);

		}

		Debug.Log ("File loaded, found " + allPieces.Count + " pieces");

		Debug.Log ("Loading Level pieces");

		foreach(piece p in allPieces)
		{
			Debug.Log("Creating piece type " + p.pieceType + " in position " + p.position.ToString());
			tempGO = GetGOfromList(p.pieceType);
			Instantiate(tempGO,p.position,Quaternion.identity).name = tempGO.name;
		}

		Debug.Log ("All pieces loaded");

		Debug.Log ("Checking Spawn Point");

		spawnPoint = GameObject.Find ("spawn_point_terrain");

		if (spawnPoint == null)
			Debug.LogError("ERROR: no Spawn Point Found");

		Debug.Log ("Checking Winning Point");
		
		winningTrigger = GameObject.Find ("winning_point");
		
		if (winningTrigger == null)
			Debug.LogError("ERROR: no Winning Point Found");

		Debug.Log ("Creating Ball and assign to main camera");

		instantiatedBall = (GameObject)	Instantiate (ball, spawnPoint.transform.Find ("SpawnPoint").position, Quaternion.identity);
		this.GetComponent<FollowObjectCam> ().objToFollow = instantiatedBall;
		
		failTrigger = GameObject.Find ("FallingTrigger");

		failTrigger.GetComponent<FallingScript> ().spawnPoint = spawnPoint.transform;

		Debug.Log ("Assigning data to main script");

		mainScript.failTrigger = failTrigger;

		mainScript.fallingCheck = failTrigger.GetComponent<FallingScript> ();

		mainScript.spawnPoint = spawnPoint.transform.Find ("SpawnPoint");

		mainScript.theBall = instantiatedBall.GetComponent<Rigidbody> ();

		mainScript.successTrigger = winningTrigger.transform.Find ("WinningPoint").gameObject;

		mainScript.winningCheck = mainScript.successTrigger.GetComponent<WinningScript> ();

		mainScript.isInitialized = true;

		Debug.Log ("Setting up UI");

		UI.BroadcastMessage ("SetPlayer", instantiatedBall,SendMessageOptions.RequireReceiver);

		instantiatedBall.SetActive (false);

		Debug.Log ("Map loaded, game can start");

	}

	GameObject GetGOfromList(string name)
	{
		foreach(GameObject singleGO in piecesGO)
		{
			if (singleGO.name == name)
				return singleGO;
		}

		return null;

	}

	// Check agains all map rules
	private bool IsMapLegal (out string  errorMsg)
	{
		bool foundSpawn = false;
		bool foundWinning = false;

		errorMsg = "";

		foreach(piece p in allPieces)
		{
            // check if there's atleast one spawn point
            //if (p.pieceType == "spawn_point_terrain")
            //    foundSpawn = true;
            if (GameObject.FindGameObjectWithTag("SpawnPoint") != null)
                foundSpawn = true;
            // check if there's atleast one winning point
            //        if (p.pieceType == "WinTrigger")
            //foundWinning = true;
            if (GameObject.FindGameObjectWithTag("WinTrigger") != null)
                foundWinning = true;
        }

		if (!foundSpawn)
		{
			errorMsg = "ERROR_NO_SPAWN";
			return false;
		}
			
		if (!foundWinning)
		{
			errorMsg = "ERROR_NO_WINNING";
			return false;
		}

		return true;
	}
	
	IEnumerator SendToWeb(List<String> AllLines, string mapName) {

		string err = "";
		UnityAction OkSetName;

		yield return new WaitForFixedUpdate ();

		// Run integrity checks on map
		if (!IsMapLegal (out err))
		{
			Debug.Log("Map has not passed verification test: " + err);
			OkSetName = new UnityAction(CancelButtonAction);
			
			dm = DialogManager.Instance ();
			dm.Choice ("Save Map","Map has not passed verification test: " + err,new string[] {"b_ok"}, new UnityAction[] {OkSetName});
            yield break;
		}

		// Create a Web Form
		WWWForm form = new WWWForm();

		form.AddField("MapName", mapName);
		if (mapID > 0)
			form.AddField("MapID", mapID);
		form.AddField("UserID", "0");
		form.AddField("PiecesCount", AllLines.Count);

		for (int i=0;i<AllLines.Count;i++)
		{
			form.AddField("Piece_" + i, AllLines[i]);
			Debug.Log("Piece_" + i + " added with value " + AllLines[i]);
		}
	
		//form.AddBinaryData("fileUpload", bytes, "screenShot.png", "image/png");
		
		// Upload to a cgi script
		WWW w = new WWW(AddMapPage, form);
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			print(w.error);
			err = "There was an ERROR saving your map: " + w.error;
		}
		else {
			print("Finished Uploading " + AllLines.Count + " pieces");
			Debug.Log (w.text);
			err = "Map saved succesfully";

			// get map id from returned text

			string[] returnedVars = w.text.Split('|');
			string[] singleVar;

			foreach (string varString in returnedVars)
			{
				if(varString.IndexOf("RADATA_") != -1)
				{
					singleVar = varString.Split('_');

					switch (singleVar[1])
					{
					case "MapID":
						mapID = Int32.Parse(singleVar[2]);
						break;
					default:
						Debug.Log("Wrong DATA retrieved: " + singleVar[1]);
						break;
					}
				}
			}

            // upload the picture if the map has been saved correctly
            if (mapID != 0)
                StartCoroutine(UploadPNG());

        }

		OkSetName = new UnityAction(CancelButtonAction);
		
		dm = DialogManager.Instance ();
		dm.Choice ("Save Map", err, new string[] {"b_ok"}, new UnityAction[] {OkSetName});

	}

    IEnumerator UploadPNG() {

        UI.SetActive(false);
        screenshotCam.SetActive(true);
        Maincam.SetActive(false);
        // We should only read the screen after all rendering is complete
        yield return new WaitForEndOfFrame();

        UI.SetActive(true);
        screenshotCam.SetActive(false);
        Maincam.SetActive(true);

        // Create a texture the size of the screen, RGB24 format
        int width = Screen.width;
        int height = Screen.height;
        var tex = new Texture2D(width, height, TextureFormat.RGB24, false);


        // Read screen contents into the texture
        tex.ReadPixels(new Rect(0, 0, width, height), 0, 0);
        tex.Apply();

        // Encode texture into PNG
        byte[] bytes = tex.EncodeToJPG(30);
        Destroy(tex);

        // Create a Web Form
        WWWForm form = new WWWForm();
        form.AddField("mapID", mapID);
        form.AddBinaryData("fileUpload", bytes, mapID + ".jpg", "image/jpg");

        // Upload to a cgi script
        WWW w = new WWW(saveScreenshotPhp, form);
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            Debug.Log(w.error);
        }
        else
        {
            Debug.Log("Finished Uploading Screenshot");
            Debug.Log(w.text);
        }


    }

	public void SaveHighScore(int milliseconds)
	{
		// fill informations for SendScore

		StartCoroutine (SendScoreToWeb(mapID , 0, milliseconds));
	}

	public void LoadHighScores()
	{
		StartCoroutine (ShowHighScores(PlayerPrefs.GetInt("map_id")));
	}

	IEnumerator ShowHighScores(int map_id)
	{
		string[] allScores, scoreLine;

		WWWForm form = new WWWForm();

		form.AddField("map_id", map_id.ToString());
		
		// Upload to a php script
		WWW w = new WWW(GetScoresOfMap, form);
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			Debug.Log(w.error);
		}
		else {
			Debug.Log("Finished Downloading HighScores");
		}
		
		Debug.Log(w.text);

		HighScoreContainer.ClearScoreBoard ();

		allScores = w.text.Split ('|');

		if (w.text != "")
		{
			for (int i=0;i<allScores.Length;i++)
			{
				scoreLine = allScores[i].Split (';');
				HighScoreContainer.AddScore(i, scoreLine[0], Int32.Parse (scoreLine[1]));
			}
		}

		// if less than 10 scores, filling the score board with empties
		if (allScores.Length < 10)
		{
			for (int i=allScores.Length ;i<=10;i++)
			{
				HighScoreContainer.AddScore(i, "Nobody", 0);
			}
		}

	}

	IEnumerator SendScoreToWeb(int map_id, int user_id, int milliseconds)
	{
				// Create a Web Form
				WWWForm form = new WWWForm ();
		
				form.AddField ("MapID", map_id);
				form.AddField ("UserID", user_id);
				form.AddField ("LapTime", milliseconds);

				// Upload to a cgi script
				WWW w = new WWW (AddScorePage, form);
				yield return w;
				if (!string.IsNullOrEmpty (w.error)) {
						print (w.error);
				} else {
						print ("Score uploaded");
						Debug.Log (w.text);
				}

	}


    // Update is called once per frame
    void Update () {
	
	}
}
