using UnityEngine;
using System.Collections;
using UnityEngine.Events;

public class FeedbackButtonScript : MonoBehaviour {

	DialogManager dm;

	UnityAction OkSetName;

	UnityAction cancelSetName;

	public string addFeedbackURL = "http://rollabout.azurewebsites.net/addfeedback.php";

	FileHandler mapRelatedData;

	// Use this for initialization
	void Start () {
		
		// search for Filehandler and map id
		mapRelatedData = GameObject.FindObjectOfType<FileHandler>();
	}

	public void OpenFeedbackPanel()
	{
		OkSetName = new UnityAction(SetMapName);
		cancelSetName = new UnityAction(CancelButtonAction);
		
		dm = DialogManager.Instance ();
		dm.Choice ("Feedback","Your feedback is EXTREMELY important for us, spit it out!",new string[] {"b_ok","b_cancel"}, new UnityAction[] {OkSetName, cancelSetName}, true);

	}

	// Gets Map Name from a Modal Dialog
	public void SetMapName()
	{
		string feedbackString;
		int mapID, userID;


		dm = DialogManager.Instance ();
		feedbackString = dm.inputText.text;

		if (mapRelatedData)
			mapID = mapRelatedData.mapID;
		else
			mapID = 0;

		// get user from player preferences (saved from mainmenu scene)
		userID = PlayerPrefs.GetInt("UserID");

		StartCoroutine(SaveFeedback(feedbackString, userID, mapID, Application.loadedLevelName));


	}
	
	public void CancelButtonAction(){
		// does nothing
	}

	IEnumerator SaveFeedback(string feedbackStr, int user_id, int map_id, string scene)
	{
		string err;

		// Create a Web Form
		WWWForm form = new WWWForm();
		
		form.AddField("Feedback", feedbackStr);

		form.AddField("MapID", map_id);
		form.AddField("UserID", user_id);
		form.AddField("Scene", scene);	
		
		//form.AddBinaryData("fileUpload", bytes, "screenShot.png", "image/png");
		
		// Upload to a cgi script
		WWW w = new WWW(addFeedbackURL, form);
		yield return w;
		if (!string.IsNullOrEmpty(w.error)) {
			print(w.error);
			err = "There was an ERROR saving your map: " + w.error;
		}
		else {
			print("Finished Uploading fedback");
			Debug.Log (w.text);
			err = "Finished Uploading fedback";
		}
		
		OkSetName = new UnityAction(CancelButtonAction);
		
		dm = DialogManager.Instance ();
		dm.Choice ("Feedback", err, new string[] {"b_ok"}, new UnityAction[] {OkSetName});
	}

}
