using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRobotMovement : MonoBehaviour {

	public RangeRobotController controller;
	public float maxDistance = 20f;
	public float shootDistance = 40f;
	public float speed = 10f;
	public float shootSpeed = 20f;

	private GameObject player;
	private Transform playerTransform;
	private bool move = false;
	private bool shoot = false;
	private Vector3 moveDir = Vector3.zero;
	private Vector3 shootDir = Vector3.zero;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		float dist = Vector3.Distance(player.transform.position, transform.position);
		if (dist > maxDistance) {
			move = true;
		}
		if (dist < shootDistance) {
			shoot = true;
		}

	}

	void FixedUpdate() {
		if (move) {
			moveDir = (player.transform.position - transform.position).normalized;	
		}
		if (shoot) {
			shootDir = (player.transform.position - transform.position).normalized;	
		}
		controller.Move(moveDir.x * speed * Time.fixedDeltaTime, moveDir.y * speed * Time.fixedDeltaTime);
		controller.Shoot(shootDir.x * shootSpeed * Time.fixedDeltaTime, shootDir.y * shootSpeed * Time.fixedDeltaTime);
		moveDir = Vector3.zero;
		shootDir = Vector3.zero;
		move = false;
		shoot = false;
	}
}
