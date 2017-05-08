using UnityEngine;
using System.Collections;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class HorizontalSlider : MonoBehaviour {

	public bool isActive;
	public Transform SlidingPart;
	// this obstacle slide for a multiplier of his size
	public int slidingUnits;  // must be filled in Unity
	public float slidingSpeed;	// must be filled in Unity
	public string axis;	// must be filled in Unity
	public AnimationCurve slidingMovement;

	Vector3 orignalPosition;

	// Use this for initialization
	void Start () {
	
		orignalPosition = new Vector3(0,0,0);

	}
	
	// Update is called once per frame
	void Update () {

        if (SlidingPart == null)
            Destroy(this);
	
		if (isActive)
		{
			switch (axis)
			{
			case "x":
                    if (SlidingPart != null)
                        SlidingPart.transform.localPosition = new Vector3(orignalPosition.x + (slidingUnits * slidingMovement.Evaluate(Time.time * slidingSpeed)),SlidingPart.localPosition.y , SlidingPart.transform.localPosition.z);
				break;
			case "y":
                    if (SlidingPart != null)
                        SlidingPart.transform.localPosition = new Vector3(SlidingPart.transform.localPosition.x,orignalPosition.y + (slidingUnits * slidingMovement.Evaluate(Time.time * slidingSpeed)), SlidingPart.transform.localPosition.z);
				break;
			case "z":
                    if (SlidingPart != null)
                        SlidingPart.transform.localPosition = new Vector3(SlidingPart.transform.localPosition.x,SlidingPart.transform.localPosition.y,orignalPosition.z + (slidingUnits * slidingMovement.Evaluate(Time.time * slidingSpeed)));
				break;
			default:
				break;
			}
		}

	}

	public void Wakeup()
	{
		isActive = true;
	}

}
