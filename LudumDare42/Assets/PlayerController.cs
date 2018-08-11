using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {

Vector3 mouse_position;
Transform PlayerTransform; //Assign to the object you want to rotate
Vector3 object_pos;
float angle;

	// Use this for initialization
	void Start () {
		PlayerTransform = this.gameObject.transform;

	}
	
	// Update is called once per frame
	void Update () {
 
     mouse_position = Input.mousePosition;
    mouse_position.z = 5.23f; //The distance between the camera and object
     object_pos = Camera.main.WorldToScreenPoint(PlayerTransform.position);
     mouse_position.x = mouse_position.x - object_pos.x;
     mouse_position.y = mouse_position.y - object_pos.y;
     angle = Mathf.Atan2(mouse_position.y, mouse_position.x) * Mathf.Rad2Deg;
     transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));

	}
}
