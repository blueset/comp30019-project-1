using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public GameObject landscape;

    public float boundary;
    public float mouseSpeed = 2.0f;
    public float moveSpeed = 5.0f;

    private float yaw = 0.0f;
    private float pitch = 0.0f;

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        InputMouse();

        InputKeyboard();

        CheckBoundary();

	}

    private void InputMouse() {

        // Yaw
        yaw += mouseSpeed * Input.GetAxis("Mouse X");
        // Pitch
        pitch -= mouseSpeed * Input.GetAxis("Mouse Y");

        transform.eulerAngles = new Vector3(pitch, yaw, 0.0f);
    }

    private void InputKeyboard() {

        Vector3 moveVector = new Vector3(0, 0, 0);

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

        transform.localPosition += moveVector;

    }

    private void CheckBoundary() {

        // Boundary is the half of size on each side
        boundary = landscape.GetComponent<Renderer>().bounds.size.x / 2;

        float x = transform.position.x;
        float y = transform.position.y;
        float z = transform.position.z;

        x = Mathf.Min(Mathf.Max(x, -boundary), boundary);
        z = Mathf.Min(Mathf.Max(z, -boundary), boundary);

        transform.position = new Vector3(x, y, z);
    }

}
