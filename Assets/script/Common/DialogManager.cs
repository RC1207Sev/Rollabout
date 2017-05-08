using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Events;

public class DialogManager : MonoBehaviour {

	public Text title;
	public Text description;
	public InputField inputText;
	public Button[] dialogButtons;
	public GameObject modalPanelObject;
	public GameObject[] objectsToDisable;

	private static DialogManager dialManager;


	// Use this for initialization
	void Start () {
	
	}

	public static DialogManager Instance(){

		// Singleton

		if (!dialManager)
		{
			dialManager = FindObjectOfType (typeof (DialogManager)) as DialogManager;
			if (!dialManager)
				Debug.LogError("There are no DialogManager script on this scene");
		}

		return dialManager;

	}

	void Awake(){

		transform.SetAsLastSibling();

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void Choice(string passedTitle, string passedDescription,string[] activeButtons, UnityAction[] passedEvent, bool isInputRequired = false)
	{
		string singleButtonName;

		modalPanelObject.SetActive(true);

		for (int i=0;i<activeButtons.Length;i++)
		{
			singleButtonName = activeButtons[i];

			foreach(Button singleButton in dialogButtons)
			{
				if (singleButton.name == singleButtonName)
				{
					singleButton.onClick.RemoveAllListeners();
					singleButton.onClick.AddListener (passedEvent[i]);
					singleButton.onClick.AddListener (ClosePanel);
					singleButton.gameObject.SetActive (true);
				}
			}
		}

		this.title.text = passedTitle;
		this.description.text = passedDescription;

		if (isInputRequired)
			inputText.gameObject.SetActive (true);
		else
			inputText.gameObject.SetActive (false);

		foreach(GameObject GO in objectsToDisable)
		{
			GO.SetActive (false);
		}

	}

	void ClosePanel()
	{
		modalPanelObject.SetActive(false);

		// reactivate all objects disabled during Modal
		foreach(GameObject GO in objectsToDisable)
		{
			GO.SetActive (true);
		}

		// Clear all buttons
		foreach(Button singleButton in dialogButtons)
		{
			singleButton.gameObject.SetActive (false);
		}

	}

}
