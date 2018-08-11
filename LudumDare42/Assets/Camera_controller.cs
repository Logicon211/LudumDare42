using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera_controller : MonoBehaviour {

    //public GameObject player;       //Public variable to store a reference to the player game object
    //private Vector3 offset;  
	// Use this for initialization

public Transform Player;
 private float Damping;
 private Transform PlayerTransform;
 private float Height;
private float Offset;
 
 private Vector3 Center;
 private float ViewDistance;



	void Start () {
		//offset = transform.position - player.transform.position;

		//This controls the camera movement speed
		Damping = 3.0f;
		Height = 0.0f;
		Offset = 0.0f;
 		ViewDistance = 0.0f;
	}
	
	// Update is called once per frame
	void Update () {
		 Vector3 mousePos = Input.mousePosition;
     	mousePos.z = ViewDistance;
    	Vector3 CursorPosition = Camera.main.ScreenToWorldPoint(mousePos);
        var PlayerPosition = Player.position;
     float playerX = PlayerPosition.x;
	 float playerY = PlayerPosition.y;
	 float mouseX = CursorPosition.x;
	 float mouseY = CursorPosition.y;

	float newX = playerX + mouseX /2;
	float newY = playerY + mouseY / 2;

	newX = Mathf.Clamp(newX, playerX-3, playerX+3);
	
	newY = Mathf.Clamp(newY, playerY-3, playerY+3);

     Center = new Vector3(newX, newY, -10);
     


     transform.position = Vector3.Lerp(transform.position, Center + new Vector3(0,Height,Offset), Time.deltaTime * Damping); 
	}

    void LateUpdate () 
    {
        // Set the position of the camera's transform to be the same as the player's, but offset by the calculated offset distance.
        //transform.position = player.transform.position + offset;
    }

}