using UnityEngine;
using System.Collections;

public class AccelerometerHandler {

    static AccelerometerHandler singleton;

    //declare matrix and calibration vector

    Matrix4x4 calibrationMatrix;

    Vector3 wantedDeadZone = Vector3.zero;

    public static AccelerometerHandler Instantiate()
    {
        if (singleton == null)
            singleton = new AccelerometerHandler();

        return singleton;
    }
 
 
    //Method for calibration 
    public void calibrateAccelerometer()
    {
        wantedDeadZone = Input.acceleration;
        Quaternion rotateQuaternion = Quaternion.FromToRotation(new Vector3(0f, 0f, -1f), wantedDeadZone);
        //create identity matrix ... rotate our matrix to match up with down vec
        Matrix4x4 matrix = Matrix4x4.TRS(Vector3.zero, rotateQuaternion, new Vector3(1f, 1f, 1f));
        //get the inverse of the matrix
        calibrationMatrix = matrix.inverse;

    }

    //Method to get the calibrated input 
    public Vector3 getAccelerometer(Vector3 accelerator)
    {
        Vector3 accel = this.calibrationMatrix.MultiplyVector(accelerator);
        return accel;
    }


}
