using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeleeRobot : MonoBehaviour, IDamageable<float> {

	public float speed = 1f;
	public float attackCooldown = 1f;
	public float attackRange = 5f;
	public float health = 10f;

	public AudioClip punchSound;
	public GameObject hitEffect;
	public GameObject explosion;
	public GameObject garbageController;

	private float attacking = 0f;
	private float currentHealth;

	GameObject player;

	Rigidbody2D RB;
	Animator animator;
	AudioSource AS;
	SpriteRenderer spriteRenderer;
	GarbageSpawnController garbageScript;
	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
		RB = GetComponent<Rigidbody2D>();
		animator = GetComponent<Animator>();
		AS = GetComponent<AudioSource>();
		garbageScript = garbageController.GetComponent<GarbageSpawnController>();
		spriteRenderer = GetComponent<SpriteRenderer>();

		currentHealth = health;
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
					AS.PlayOneShot(punchSound);
				} else {

					attacking -= Time.deltaTime;
					if (attacking <= 0f) {
						attacking = 0f;
					}
				}
				
			}
		}

		//Get Redder as you take more damage:
		if (currentHealth < health) {
			if(currentHealth < 0f) {
				currentHealth = 0f;
			}
			float healthPercentage = currentHealth/health;
			spriteRenderer.color = new Color(1f, healthPercentage, healthPercentage);
		}
	}

	public void finishPunch() {
		//TODO: Check player distance and if they're still close, finish the punch, make the noise and do damage
		Instantiate(hitEffect, new Vector3(player.transform.position.x, player.transform.position.y, player.transform.position.z - 0.1f), Quaternion.identity);
	}

	// Damages enemy and handles death shit
	public void Damage(float damageTaken) {
		// Damages enemy and handles death shit
		currentHealth -= damageTaken;
		if (currentHealth <= 0f) {
			Instantiate(explosion, transform.position, Quaternion.identity);
			garbageScript.SpawnAtLocation(1, transform.position.x, transform.position.y);
			Destroy(gameObject);
		}
	}
}
