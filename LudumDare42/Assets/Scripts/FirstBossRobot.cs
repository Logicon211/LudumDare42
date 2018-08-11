using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FirstBossRobot : MonoBehaviour {

	public float speed = 1f;
	public float attackCooldown = 1f;
	public float attackRange = 5f;

	public float ballRadius = 5;
	public float ballSpinSpeed = 20f;
	public float ballExtendSpeed = 1f;

	public GameObject spikeBall;
	public Material chainMaterial;

	private float attacking = 0f;


	GameObject player;

	Rigidbody2D RB;
	Animator animator;

	// Use this for initialization

	GameObject leftArm, rightArm, leftSpikeBall, rightSpikeBall;

	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		RB = GetComponent<Rigidbody2D>();
		// groundCheck1 = transform.Find ("groundCheck1");
		leftArm = transform.Find("LeftArm").gameObject;
		rightArm = transform.Find("RightArm").gameObject;
		leftSpikeBall = Instantiate(spikeBall, leftArm.transform.position, Quaternion.identity); //transform.Find("SpikeBallLeft").gameObject;
		rightSpikeBall =Instantiate(spikeBall, leftArm.transform.position, Quaternion.identity); // transform.Find("SpikeBallRight").gameObject;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (player != null) {
			// float angle = Mathf.Atan2(player.transform.position.y, player.transform.position.x) * Mathf.Rad2Deg;
        	// transform.rotation = Quaternion.AngleAxis(angle, Vector3.forward);

			Vector2 direction = Vector3.Normalize(player.transform.position - transform.position);

			if (direction != Vector2.zero) {
				transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
			}

			RB.velocity = direction * speed;

			// if(Vector2.Distance(transform.position, player.transform.position) <= attackRange) {
				
			// 	if(attacking <= 0f) {
			// 		attacking = attackCooldown;
			// 	} else {
			// 		attacking -= Time.deltaTime;
			// 		if (attacking <= 0f) {
			// 			attacking = 0f;
			// 		}
			// 	}
				
			// }

			leftSpikeBall.transform.RotateAround (leftArm.transform.position, Vector3.forward, ballSpinSpeed * Time.deltaTime);
			var desiredPosition = (leftSpikeBall.transform.position - leftArm.transform.position).normalized * ballRadius + leftArm.transform.position;
			leftSpikeBall.transform.position = Vector3.MoveTowards(leftSpikeBall.transform.position, desiredPosition, Time.deltaTime * ballExtendSpeed);

			rightSpikeBall.transform.RotateAround (rightArm.transform.position, Vector3.forward, ballSpinSpeed * Time.deltaTime);
			desiredPosition = (rightSpikeBall.transform.position - rightArm.transform.position).normalized * ballRadius + rightArm.transform.position;
			rightSpikeBall.transform.position = Vector3.MoveTowards(rightSpikeBall.transform.position, desiredPosition, Time.deltaTime * ballExtendSpeed);

			//Draw lines
			LineRenderer leftBallLR = leftSpikeBall.GetComponent<LineRenderer>();
			LineRenderer rightBallLR = rightSpikeBall.GetComponent<LineRenderer>();

			//Chain texture stuff that didn't work
			//leftBallLR.textureMode =  LineTextureMode.Tile;
       		//leftBallLR.material.SetTextureScale("_MainTex", new Vector2(tileAmount, 1.0f));
			// leftBallLR.material = chainMaterial;

			leftBallLR.enabled = true;
			leftBallLR.positionCount = 2;
			leftBallLR.material.color = Color.black;
			leftBallLR.startWidth = 1f;
			leftBallLR.endWidth = 0.5f;
			leftBallLR.SetPosition (0, new Vector3(leftSpikeBall.transform.position.x, leftSpikeBall.transform.position.y, 0f));
			leftBallLR.SetPosition (1, new Vector3(leftArm.transform.position.x, leftArm.transform.position.y, 1f));

			rightBallLR.enabled = true;
			rightBallLR.positionCount = 2;
			rightBallLR.material.color = Color.black;
			rightBallLR.startWidth = 1f;
			rightBallLR.endWidth = 0.5f;
			rightBallLR.SetPosition (0, new Vector3(rightSpikeBall.transform.position.x, rightSpikeBall.transform.position.y, 0f));
			rightBallLR.SetPosition (1, new Vector3(rightArm.transform.position.x, rightArm.transform.position.y, 1f));
		}
	}
}
