using UnityEngine;

public class PanelAnimationScript : MonoBehaviour {

	public GameObject player; // must be filled in Unity
	Animator panelAnimations;

	static int normalStateHash = Animator.StringToHash("Base.entry");		// NOTE: the layer name MUST be "Base" and not the standard "Base Layer"
	static int DisabledStateHash = Animator.StringToHash("Base.Disabled");	// NOTE: the layer name MUST be "Base" and not the standard "Base Layer"
	
	// Use this for initialization
	void Start () {

		panelAnimations = this.GetComponent<Animator>();
	
	}

	public void UIToggle()
	{


		if (panelAnimations.GetCurrentAnimatorStateInfo(0).nameHash == DisabledStateHash)
		{
			panelAnimations.SetTrigger("Toggle");
			Debug.Log (this.name + " enabled");
            if (player != null)
			    player.SetActive(false);
		}
		else
		{
			panelAnimations.SetTrigger("Toggle");
			Debug.Log (this.name + " disabled");
            if (player != null)
                player.SetActive(true);
		}
	}

	public void SetPlayer(GameObject p)
	{
		player = p;
	}

}
