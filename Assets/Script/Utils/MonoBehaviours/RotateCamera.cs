using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotateCamera : MonoBehaviour {

    public Transform secondPivot;

    Vector3 rotationMousePos;
    Vector3 rotationTransformRotation;
    Vector3 rotationCameraRotation;

    public float rotationSensibility = 0.25f;

    Vector3 moveMousePos;

    public float dragSpeed = 0.5f;

    public float zoomSensibility = 0.75f;

    // Update is called once per frame
    void Update()
    {
        //Rotation
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            transform.rotation = Quaternion.Euler(0, -45, 0);
            secondPivot.transform.rotation = Quaternion.Euler(30, 0, 0);
            transform.position = new Vector3(Level.singleton.arena.Width / 2, 0, Level.singleton.arena.Height / 2);
        }
        else
        { 
            if (Input.GetMouseButtonDown(1))
            {
                rotationMousePos = Input.mousePosition;
                rotationTransformRotation = transform.rotation.eulerAngles;
                rotationCameraRotation = secondPivot.rotation.eulerAngles;
            }
            else if (Input.GetMouseButton(1))
            {
                transform.rotation = Quaternion.Euler(rotationTransformRotation.x, rotationTransformRotation.y + (Input.mousePosition.x - rotationMousePos.x) * rotationSensibility, rotationTransformRotation.z);
                secondPivot.transform.rotation = Quaternion.Euler(Mathf.Clamp(rotationCameraRotation.x + (Input.mousePosition.y - rotationMousePos.y) * rotationSensibility * 0.5f, 25, 75), transform.rotation.eulerAngles.y, rotationCameraRotation.z);
            }
            else
            {
                //Déplacement
                if (Input.GetMouseButtonDown(2))
                {
                    moveMousePos = Input.mousePosition;
                }
                else if (Input.GetMouseButton(2))
                {
                    Vector3 pos = Camera.main.ScreenToViewportPoint(Input.mousePosition - moveMousePos);
                    transform.position += transform.right * pos.x * dragSpeed;
                    transform.position += transform.forward * pos.y * dragSpeed;

                    transform.position = new Vector3(Mathf.Clamp(transform.position.x, 0, Level.singleton.arena.Width)
                        , 3.5f
                        , Mathf.Clamp(transform.position.z, 0, Level.singleton.arena.Height));
                }
            }
    }

        if (Input.mouseScrollDelta.y != 0)
        {
            Camera.main.orthographicSize = Mathf.Clamp(Camera.main.orthographicSize + -Input.mouseScrollDelta.y * zoomSensibility, 2.5f, 15f);
        }
    }
}
