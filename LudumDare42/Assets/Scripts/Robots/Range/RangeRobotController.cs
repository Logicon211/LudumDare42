using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRobotController : MonoBehaviour, IDamageable<float> {

	[Range(0, .3f)] [SerializeField] private float movementSmoothing = .05f;
	[SerializeField] private float health = 10f;
	[SerializeField] private float shootCooldown = 5f;
	public GameObject projectile;
	public GameObject garbageController;
	public Animator animator;

	public GameObject explosion;
	public GameObject poofEffect;
	
	private AudioSource audio;
	private Rigidbody2D enemyBody;
	private Vector3 velocity = Vector3.zero;
	private float currentCooldown;
	private bool hasShot = true;
	private GarbageSpawnController garbageScript;
	private GameManager gameManager;

	private float currentHealth;
	SpriteRenderer spriteRenderer;

	private bool isDead = false;

	private Transform shootPosition;

	void Awake() {
		enemyBody = GetComponent<Rigidbody2D>();
		audio = GetComponent<AudioSource>();
		currentCooldown = 2f;
	}

	// Use this for initialization
	void Start () {
		garbageScript = garbageController.GetComponent<GarbageSpawnController>();
		spriteRenderer = GetComponent<SpriteRenderer>();
		gameManager = GameObject.FindWithTag("GameController").GetComponent<GameManager>();
		shootPosition = transform.Find("ShootPosition");
		currentHealth = health;

		Instantiate(poofEffect, transform.position, Quaternion.identity);
	}
	
	// Update is called once per frame
	void Update () {
		//Handling shooting cooldown
		if (hasShot) {
			currentCooldown -= Time.deltaTime;
			if (currentCooldown <= 0f) {
				currentCooldown = shootCooldown;
				hasShot = false;
			}
		}

		//Get Redder as you take more damage:
		if (currentHealth < health) {
			if(currentHealth < 0f) {
				currentHealth = 0f;
			}
			float healthPercentage = currentHealth/health;
			Debug.Log("SPRITE RENDERER");
			Debug.Log(spriteRenderer);
			spriteRenderer.color = new Color(1f, healthPercentage, healthPercentage);
		}
	}

	// Moves the enemy towards the x and y given
	public void Move(float tarX, float tarY) {
		
		//Create a new velocity then smooth is out when we apply it
		Vector3 targetVelocity = new Vector2(tarX * 10f, tarY * 10f);
		enemyBody.velocity = Vector3.SmoothDamp(enemyBody.velocity, targetVelocity, ref velocity, movementSmoothing);		
	}

	// Rotates the enemy to face the player
	public void Rotate(Vector3 direction) {
		if (direction != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation(Vector3.forward, direction);
		}
	}

	// Shoots bullet at target x and y
	public void Shoot(float tarX, float tarY) {
		if (!hasShot) {
			animator.SetTrigger("shootTrigger");
			audio.Play(0);
			GameObject bullet = Instantiate(projectile, shootPosition.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(tarX * 10f, tarY * 10f);
			hasShot = true;
		}
	}

	// Damages enemy and handles death shit
	public void Damage(float damageTaken) {
		currentHealth -= damageTaken;
		if (currentHealth <= 0f && !isDead) {
			isDead = true;
			Instantiate(explosion, transform.position, Quaternion.identity);
			garbageScript.SpawnAtLocation(GarbageSpawnController.CIRCLE_GARBAGE_INDEX, transform.position.x, transform.position.y, false);
			gameManager.DecreaseEnemyCount();
			Destroy(gameObject);
		}
	}
}
