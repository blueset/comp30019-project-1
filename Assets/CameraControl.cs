using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraControl : MonoBehaviour {

    public float boundary;
    public float mouseSensitvity = 5.0f;
    public float moveSpeed = 5.0f;
    public float rollSpeed = 100.0f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {

        InputMouse();
        InputKeyboard();

	}

    private void InputKeyboard() {

        Vector3 moveVector = new Vector3(0, 0, 0);

        if (Input.GetKey(KeyCode.W)) {  // W: forward
            moveVector += this.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.S)) {  // S: backward
            moveVector -= this.transform.forward * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.A)) {  // A: left
            moveVector -= transform.right * moveSpeed * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.D)) {  // D: right
            moveVector += transform.right * moveSpeed * Time.deltaTime;
        }

        this.transform.localPosition += moveVector;

    }

    private void InputMouse() {

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitvity;
        float mouseY = Input.GetAxis("Mouse Y") * -mouseSensitvity;

        this.transform.Rotate(Vector3.up, mouseX);
        this.transform.Rotate(Vector3.right, mouseY);

    }

}
