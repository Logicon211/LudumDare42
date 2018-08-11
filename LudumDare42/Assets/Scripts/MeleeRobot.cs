using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeRobot : MonoBehaviour {

	public float speed = 1f;
	public float attackCooldown = 1f;
	public float attackRange = 5f;

	private float attacking = 0f;


	GameObject player;

	Rigidbody2D RB;
	Animator animator;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		RB = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		// groundCheck1 = transform.Find ("groundCheck1");
		
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

			//Debug.Log(Vector2.Distance(transform.position, player.transform.position));
			if(Vector2.Distance(transform.position, player.transform.position) <= attackRange) {
				
				if(attacking <= 0f) {
					animator.SetTrigger("Attack");
					attacking = attackCooldown;
				} else {
					attacking -= Time.deltaTime;
					if (attacking <= 0f) {
						attacking = 0f;
					}
				}
				
			}
		}

		if(Input.GetMouseButtonDown(0)) {
			animator.SetTrigger("Attack");
		}
	}
}
