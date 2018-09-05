using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public LandscapeMeshable landscape;

    public float boundary;
    public float mouseSpeed = 4.0f;
    public float moveSpeed = 5.0f;

    // Use this for initialization
    void Start () {

        // Boundary is the half of size on each side
        // Size is (2 ^ detail level) in diamond square algorithm
        boundary = Mathf.Pow(2, landscape.detailLevel) / 2;

        // Camera default position
        transform.position = new Vector3(boundary, 50, boundary);
        transform.LookAt(Vector3.zero);

        // Lock the mouse
        Cursor.lockState = CursorLockMode.Locked;

    }
	
	// Update is called once per frame
	void Update () {

        InputMouse();

        InputKeyboard();

	}

    private void InputMouse() {

        // Yaw
        float yaw = mouseSpeed * Input.GetAxis("Mouse X");
        yaw += transform.eulerAngles.y;

        // Pitch
        float pitch = -mouseSpeed * Input.GetAxis("Mouse Y");
        pitch += transform.eulerAngles.x;
        // Restricted in -90 ~ 90 degrees
        if (pitch <= 180.0f) {
            pitch = Mathf.Min(pitch, 90.0f);
        } else {
            pitch = Mathf.Max(pitch, 270.0f);
        }

        // Roll
        float roll = transform.eulerAngles.z;

        transform.eulerAngles = new Vector3(pitch, yaw, roll);

    }

    private void InputKeyboard() {

        Vector3 moveVector = transform.localPosition;

        // Forward
        if (Input.GetKey(KeyCode.W)) {
            moveVector += transform.forward * moveSpeed * Time.deltaTime;
        }
        // Backward
        if (Input.GetKey(KeyCode.S)) {
            moveVector -= transform.forward * moveSpeed * Time.deltaTime;
        }
        // Left
        if (Input.GetKey(KeyCode.A)) {
            moveVector -= transform.right * moveSpeed * Time.deltaTime;
        }
        // Right
        if (Input.GetKey(KeyCode.D)) {
            moveVector += transform.right * moveSpeed * Time.deltaTime;
        }

        // Check the boundary
        moveVector.x = Mathf.Min(Mathf.Max(moveVector.x, -boundary), boundary);
        moveVector.z = Mathf.Min(Mathf.Max(moveVector.z, -boundary), boundary);

        transform.position = moveVector;

    }

}
