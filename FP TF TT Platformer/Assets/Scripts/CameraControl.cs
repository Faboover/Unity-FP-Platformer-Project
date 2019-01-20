using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour
{

    public enum RotationAxes { MouseXAndY = 0, MouseX = 1, MouseY = 2 }
    public RotationAxes axes = RotationAxes.MouseXAndY;

    public float sensitivityY = 5F;

    public float minimumY = -60F;
    public float maximumY = 60F;

    float rotationX = 0F;

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        // Only Rotates head up and down
        if (axes == RotationAxes.MouseXAndY)
        {

            rotationX = transform.localEulerAngles.x + -(Input.GetAxis("Mouse Y")) * sensitivityY;
            //rotationX = Mathf.Clamp (rotationX, minimumY, maximumY);
            if (rotationX <= 60.0f || rotationX >= 300.0f)
            {
                transform.localEulerAngles = new Vector3(rotationX, 90, 0);
            }

        }
        //		else if (axes == RotationAxes.MouseX)
        //		{
        //			transform.Rotate(Input.GetAxis("Mouse X") * sensitivityX, 0, 0);
        //		}
        //		else
        //		{
        //			rotationX += Input.GetAxis("Mouse Y") * sensitivityY;
        //			rotationX = Mathf.Clamp (rotationX, minimumY, maximumY);
        //			
        //			transform.localEulerAngles = new Vector3(-rotationX, transform.localEulerAngles.y, 0);
        //		}

    }
}
