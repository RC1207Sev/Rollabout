using UnityEngine;
using System.Collections;

public class ProfileScript : MonoBehaviour {

	public Transform ballPosition;

	public GameObject lastMenu;

    public string userSetSphere = "http://rollabout.azurewebsites.net/setusersphere.php";

    void Start() {

	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public void ChangeMaterial(GameObject passedPrefab)
	{
        GameObject tempGO;

        //rend.material = passedMat;
        foreach(Transform child in ballPosition)
        {
            Destroy(child.gameObject);
        }

        tempGO = (GameObject) Instantiate(passedPrefab, ballPosition.position, ballPosition.rotation);
        tempGO.GetComponent<Rigidbody>().useGravity = false;
        tempGO.transform.localScale = new Vector3(2, 2, 2);
        tempGO.AddComponent<RotatorScript>().speedy = -0.1f;
        tempGO.name = passedPrefab.name;
        tempGO.transform.SetParent(ballPosition);

	}

	public void MenuToggle(GameObject menu)
	{

		if ((lastMenu != null))
        {
            if (lastMenu.name == menu.name)
                return;
            lastMenu.GetComponent<Animator>().SetTrigger("Toggle");
        } 
            
		menu.GetComponent<Animator>().SetTrigger("Toggle");
		lastMenu = menu;
	}

    public void SetAsCurrentSphere()
    {

        StartCoroutine(SetUserSphereWWW(PlayerPrefs.GetInt("user_id"), ballPosition.GetChild(0).gameObject));

    }

    IEnumerator SetUserSphereWWW(int userid, GameObject sphere)
    {
        string err;
        // Create a Web Form
        WWWForm form = new WWWForm();

        form.AddField("user_id", userid);
        form.AddField("ball_pref", sphere.name);

        // Upload to a cgi script
        WWW w = new WWW(userSetSphere, form);
        yield return w;
        if (!string.IsNullOrEmpty(w.error))
        {
            print(w.error);
            err = "There was an ERROR saving your sphere: " + w.error;
        }
        else
        {
            print("User Sphere set successfully");
            Debug.Log(w.text);
            err = "Map saved succesfully";
        }

        // TODO: show modal dialog
        Debug.Log(err);

    }

}
