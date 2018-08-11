using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCameraController : MonoBehaviour {

	public float cameraSpeed = 1f;
	private float horizontalMovement = 0f;
	private float verticalMovement = 0f;

	// Use this for initialization
	void Start () {
		
	}

	// Update is called once per frame
	void Update () {
		horizontalMovement = Input.GetAxisRaw ("Horizontal");
		verticalMovement = Input.GetAxisRaw ("Vertical");
	}

	void FixedUpdate () {
		Vector3 move = new Vector3(horizontalMovement, verticalMovement, 0);
		this.transform.position += move * cameraSpeed * Time.fixedDeltaTime;
	}
		
}
