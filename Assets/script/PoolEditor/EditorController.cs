using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System;

public class EditorController : MonoBehaviour {

	public float speed;

    public float touchSensibility;

    public float altitude;

	public float zoomSpeed;

	public GameObject GridGO;

	public Text labelLayerLevel;

	public int xmin,xmax,zmin,zmax,maxy,miny;

	int currentRotation;

	Vector3 movement, oldPosition;

	float moveHorizontal, moveVertical, zoom;
	
	private Quaternion neededRotation;

    // touch ray positions
    private Vector2 worldStartPoint;

    private int nbTouches;
    private Vector2 lastDelta;


    // Use this for initialization
    void Start () {
		currentRotation = 0;
		neededRotation = this.transform.rotation;

        lastDelta = Vector2.zero;
    }
	
	// Update is called once per frame
	void Update () {

        // only work with one touch
        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            nbTouches = Input.touchCount;

            if (nbTouches == 1)
            {
                for (int i = 0; i < nbTouches; i++)
                {
                    Touch touch = Input.GetTouch(i);

                    TouchPhase phase = touch.phase;

                    switch (phase)
                    {
                        case TouchPhase.Began:
                            print("New touch detected at position " + touch.position + " , index " + touch.fingerId);
                            break;
                        case TouchPhase.Moved:
                            lastDelta = touch.deltaPosition;
                            moveHorizontal = -lastDelta.x * touchSensibility;
                            moveVertical = -lastDelta.y * touchSensibility;
                            print("Touch index " + touch.fingerId + " has moved by " + touch.deltaPosition);
                            break;
                        case TouchPhase.Stationary:
                            print("Touch index " + touch.fingerId + " is stationary at position " + touch.position);
                            break;
                        case TouchPhase.Ended:
                            print("Touch index " + touch.fingerId + " ended at position " + touch.position);
                            break;
                        case TouchPhase.Canceled:
                            print("Touch index " + touch.fingerId + " cancelled");
                            break;
                    }
                }
            }

        }else
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }

		zoom = Input.GetAxis("Mouse ScrollWheel");
		
		movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);

        movement = Vector3.Lerp(movement, Vector3.zero, 0.5f);

		
		if (((altitude - (zoom * zoomSpeed))) > miny && (altitude - (zoom * zoomSpeed)) > (GridGO.transform.position.y - 4)) 
		{
			altitude -=  zoom * zoomSpeed;
		}

		oldPosition = this.transform.position;

		this.transform.Translate (movement * speed);
		this.transform.position = new Vector3(this.transform.position.x, altitude, this.transform.position.z);

		// check camera position to keep the player between bounds
		if (!(((this.transform.position.x > xmin) && (this.transform.position.x < xmax)) &&
		    ((this.transform.position.z > zmin) && (this.transform.position.z < zmax))))
		{
			this.transform.position = oldPosition;
		}

		this.transform.rotation = Quaternion.Slerp(this.transform.rotation, neededRotation, Time.deltaTime * 2f);
		
	}

	public void Rotate(int angle)
	{
		currentRotation += angle;
		neededRotation = new Quaternion();
		neededRotation.eulerAngles = new Vector3(this.transform.rotation.eulerAngles.x, currentRotation, this.transform.rotation.eulerAngles.z);
	}

	
	public void ChangeAltitudeRelative(int delta)
	{

		float tempAlt = GridGO.transform.position.y + delta;

		if ((tempAlt > miny) && (tempAlt < maxy)) 
		{
			GridGO.transform.Translate(0,delta,0);
			altitude = tempAlt;
			labelLayerLevel.text = (Int32.Parse(labelLayerLevel.text) + delta) + "";
		}

	}

    // convert screen point to world point
    private Vector2 getWorldPoint(Vector2 screenPoint)
    {
        RaycastHit hit;
        Physics.Raycast(Camera.main.ScreenPointToRay(screenPoint), out hit);
        return hit.point;
    }

}
