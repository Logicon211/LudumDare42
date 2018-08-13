using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageController : MonoBehaviour, IDamageable<float> {

	private float spawnYOffset = 5f;
	private float dropSpeed = 15f;

	public Material shadowMaterial;
	public bool dropFromSky = true;

	public float health = 30f;

	public GameObject explosion;

	public float powerupSpawnChance = 10f;

	private GameObject powerUpControllerGameObject;
	private PowerUpController powerUpController;

	private Collider2D collider;

	private SpriteRenderer casterRenderer;
	private SpriteRenderer shadowRenderer;

	private GameObject shadowObject;
	private bool isDropping;

	private float currentHealth;
	private bool isDead = false;

	public float landingDamage = 10f;

	// Use this for initialization
	void Start () {
		casterRenderer = GetComponent<SpriteRenderer> ();
		currentHealth = health;
		powerUpControllerGameObject = GameObject.FindWithTag ("PowerUpController");
		powerUpController = powerUpControllerGameObject.GetComponent<PowerUpController> ();
		if (dropFromSky) {
			collider = GetComponent<Collider2D> ();
			InitShadow ();
			StartGarbageDrop ();
		}
	}

	// Update is called once per frame
	void Update () {
		//Get Redder as you take more damage:
		if (currentHealth < health) {
			if(currentHealth < 0f) {
				currentHealth = 0f;
			}
			float healthPercentage = currentHealth/health;
			casterRenderer.color = new Color(1f, healthPercentage, healthPercentage);
		}
	}

	void FixedUpdate () {
		if (isDropping) {
			DropGarbage ();
		}
	}

	private void StartGarbageDrop () {
		//Disable collider until garbage drops into place
		collider.enabled = false;

		//Find top of camera to ensure garbage never spawns in view of player
		Vector2 topOfCameraInViewportCoordinates = new Vector2 (1f, 1f);
		Vector2 topOfCameraInWorldCoordinates = Camera.main.ViewportToWorldPoint (topOfCameraInViewportCoordinates);
		float topOfCameraY = topOfCameraInWorldCoordinates.y;

		//Spawn garbage above cameras max y value
		transform.position = new Vector2 (transform.position.x, topOfCameraY + spawnYOffset);
		casterRenderer.sortingLayerName = "Falling Garbage";
		isDropping = true;
	}

	private void InitShadow () {
		shadowObject = new GameObject ();
		shadowObject.transform.position = new Vector3(transform.position.x, transform.position.y, -1);
		shadowObject.transform.localScale = transform.localScale;
		shadowObject.transform.rotation = transform.rotation;
		shadowRenderer = shadowObject.AddComponent<SpriteRenderer> ();
		shadowRenderer.sprite = casterRenderer.sprite;

		shadowRenderer.material = shadowMaterial;
		shadowRenderer.sortingLayerName = casterRenderer.sortingLayerName;
		shadowRenderer.sortingOrder = casterRenderer.sortingOrder + 10;
	}

	private void DropGarbage () {
		float yPos = Mathf.Max (shadowObject.transform.position.y, transform.position.y - (dropSpeed * Time.fixedDeltaTime));
		transform.position = new Vector3 (transform.position.x, yPos, -2);
		if (yPos <= shadowObject.transform.position.y) {
			Destroy (shadowObject);
			collider.enabled = true;
			casterRenderer.sortingLayerName = "Garbage";
			isDropping = false;
			CheckCollisionWithPlayer ();
		}
	}

	public void Damage(float damageTaken) {
		// Damages enemy and handles death
		currentHealth -= damageTaken;
		if (currentHealth <= 0f && !isDead) {
			isDead = true;

			Instantiate(explosion, transform.position, Quaternion.identity);
			Random.seed = System.DateTime.Now.Millisecond;
			
			float randomRoll = Random.Range(1f, 100f);
			if(randomRoll <= powerupSpawnChance) {
				try{
				powerUpController.SpawnRandomPowerUpAtLocation (transform.position.x, transform.position.y);
				}
				catch(System.NullReferenceException e){
					//Catching exception that happens if you try to spawn a powerup on a dead object
				}
			}
			Destroy(gameObject);
		}

	}

	public void CheckCollisionWithPlayer() {
		Collider2D[] contacts = new Collider2D[5];
		ContactFilter2D filter = new ContactFilter2D ();
		collider.GetComponent<Collider2D> ().OverlapCollider (filter.NoFilter(), contacts);
		foreach (Collider2D collision in contacts) {
			if (collision != null) {
				PlayerController playerController = collision.gameObject.GetComponent<PlayerController> ();
				if (playerController != null) {
					playerController.Damage (landingDamage);
					return;
				}
			}
		}
	}

}
