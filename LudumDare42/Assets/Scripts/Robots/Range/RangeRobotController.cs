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
	
	private AudioSource audio;
	private Rigidbody2D enemyBody;
	private Vector3 velocity = Vector3.zero;
	private float currentCooldown;
	private bool hasShot = false;
	private GarbageSpawnController garbageScript;

	void Awake() {
		enemyBody = GetComponent<Rigidbody2D>();
		audio = GetComponent<AudioSource>();
		currentCooldown = shootCooldown;
	}

	// Use this for initialization
	void Start () {
		garbageScript = garbageController.GetComponent<GarbageSpawnController>();
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
			GameObject bullet = Instantiate(projectile, transform.position, Quaternion.identity) as GameObject;
            bullet.GetComponent<Rigidbody2D>().velocity = new Vector2(tarX * 10f, tarY * 10f);
			hasShot = true;
		}
	}

	// Damages enemy and handles death shit
	public void Damage(float damageTaken) {
		health -= damageTaken;
		if (health <= 0f) {
			garbageScript.SpawnAtLocation(1, transform.position.x, transform.position.y);
			Destroy(gameObject);
		}
	}
}
