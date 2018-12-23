using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ThirdPersonCamera : MonoBehaviour {

    public bool lockCursor;
    public float cameraSensitivity = 1;

    public Transform target; //player character
    public float dstFromTarget = 2; //distance from player character
    public Vector2 yawMinMax = new Vector2(-45, 45);

    float yaw; //horizontal axis
    float pitch = 45f; //vertical axis

	void Start () {
		if (lockCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
	}
	
    //LateUpdate = appelé après Update
	void LateUpdate () {
        float sideCameraValue = Input.GetAxisRaw("Mouse X");
        float frontCameraValue = Input.GetAxisRaw("Mouse Y");
        //à passer sur joystick droit

        yaw += sideCameraValue * cameraSensitivity; 
        yaw = Mathf.Clamp(yaw, yawMinMax.x, yawMinMax.y);
        //force yaw à être entre yawmin et yawmax

        if (frontCameraValue > 0 && yaw > 0)
        {
            yaw = yaw - frontCameraValue;
        }
        if (frontCameraValue > 0 && yaw < 0)
        {
            yaw = yaw + frontCameraValue;
        }
        if (frontCameraValue < 0 && yaw > 0)
        {
            yaw = yaw + frontCameraValue;
        }
        if (frontCameraValue < 0 && yaw < 0)
        {
            yaw = yaw - frontCameraValue;
        }
        //remettre la caméra au milieu

        Vector3 targetRotation = new Vector3(pitch, yaw);
        transform.eulerAngles = targetRotation;
        //virage de caméra progressif

        transform.position = target.position - transform.forward * dstFromTarget;
        //target = player character
    }
}
