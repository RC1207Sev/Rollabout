using UnityEngine;
using System.Collections;

public class MultiStyleSelector : MonoBehaviour {

    public string style;

    public MultiStructPrefabHandler[] styleStructsHandlers;

	// Use this for initialization
	void Start () {

        styleStructsHandlers = this.GetComponents<MultiStructPrefabHandler>();

	}

    public void ChangeStyle(string pStyle)
    {
        style = pStyle;
    }

    public GameObject GetRandomPrefab()
    {
        foreach (MultiStructPrefabHandler singleStyle in styleStructsHandlers)
        {
            if (singleStyle.style == style)
                return singleStyle.GetRandomPrefab();
        }

        return null;

    }

}
