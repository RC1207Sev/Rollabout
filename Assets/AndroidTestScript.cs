using UnityEngine;
using System.Collections;

public class AndroidTestScript : MonoBehaviour {

    public PlayerController pc;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

    public void SetInitialsAcc()
    {
        pc.startingHorizontalRot = Input.acceleration.x;
        pc.startingVerticalRot = Input.acceleration.y;

        pc.smartAcc.calibrateAccelerometer();

        pc.x_axis_start.text = pc.startingHorizontalRot.ToString();
        pc.y_axis_start.text = pc.startingVerticalRot.ToString();
    }
}
