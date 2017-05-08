using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PlayerController : MonoBehaviour {
	
	public float speed;
	
	private Rigidbody rb;

	private Transform mainCamera;

	private Quaternion tempRotation, oldRotation;

    public float startingHorizontalRot;
    public float startingVerticalRot;

    private float moveHorizontal;
    private float moveVertical;

    public AccelerometerHandler smartAcc;
    private Vector3 smartAccVect;

    //debug info
    public Text x_axis, y_axis, movement_text, x_axis_orig, y_axis_orig;
    public Text x_axis_start, y_axis_start;

    private Vector3 movement;

    void Start ()
	{
        // turn off screen saver (darkner)
        Screen.sleepTimeout = 0;
        rb = this.GetComponent<Rigidbody>();
		mainCamera = GameObject.Find ("Main Camera").transform;

        smartAcc = AccelerometerHandler.Instantiate();
        smartAcc.calibrateAccelerometer();
    }

    void Awake()
    {
        //    startingHorizontalRot = Input.acceleration.x;
        //    startingVerticalRot = Input.acceleration.y;

        //x_axis_start.text = startingHorizontalRot.ToString();
        //y_axis_start.text = startingVerticalRot.ToString();
    }
	
	void FixedUpdate ()
	{

        if (SystemInfo.deviceType == DeviceType.Handheld)
        {
            smartAccVect = smartAcc.getAccelerometer(Input.acceleration);

            //moveHorizontal = Mathf.Max(Mathf.Min(Input.acceleration.x - startingHorizontalRot, 0.3f), -0.3f);
            //moveVertical = Mathf.Max(Mathf.Min(Input.acceleration.y - startingVerticalRot, 0.3f), -0.3f);
            moveHorizontal = Mathf.Clamp(smartAccVect.x, -0.3f, 0.3f);
            moveVertical = Mathf.Clamp(smartAccVect.y, -0.3f, 0.3f);
        }
        else
        {
            moveHorizontal = Input.GetAxis("Horizontal");
            moveVertical = Input.GetAxis("Vertical");
        }

        if (x_axis_orig != null)
            x_axis_orig.text = Input.acceleration.x.ToString();


        if (y_axis_orig != null)
            y_axis_orig.text = Input.acceleration.y.ToString();

        if (x_axis != null)
            x_axis.text = moveHorizontal.ToString();


        if (y_axis != null)
            y_axis.text = moveVertical.ToString();

        movement = new Vector3(moveHorizontal, 0.0f, moveVertical);

        if (movement.magnitude > 0.3f)
        {
            movement = movement.normalized;
            movement = movement / 10 * 3;
        }

        movement = mainCamera.TransformDirection(movement);

        movement.y = 0;

        rb.AddForce(movement * speed);

        if (movement_text != null)
            movement_text.text = movement.ToString();
    }
}