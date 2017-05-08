using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ready_button_script : MonoBehaviour {

	public GameObject player;  // must be filled in Unity
	public GameObject entireUI;	// must be filled in Unity

	Animator button_anim;

	static int normalStateHash = Animator.StringToHash("Base.Normal");
	static int DisabledStateHash = Animator.StringToHash("Base.Disabled");

	// Use this for initialization
	void Start () {
	
		button_anim = this.GetComponent<Animator>();
		player.SetActive(false);
	}
	
	// Update is called once per frame
	void Update () {

		/*
		if ((button_anim.GetAnimatorTransitionInfo(0).nameHash == normalStateHash) && player.activeSelf){
				player.SetActive(false);
		}
		else
		if ((button_anim.GetAnimatorTransitionInfo(0).nameHash == DisabledStateHash) && !player.activeSelf){
				player.SetActive(true);
		}
		 */


	
	}

	public void ToggleVisibility()
	{

	}

}

