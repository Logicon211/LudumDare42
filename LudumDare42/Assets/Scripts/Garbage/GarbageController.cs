using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageController : MonoBehaviour, IDamageable<float> {

	private float spawnYOffset = 5f;
	private float dropSpeed = 15f;

	public Material shadowMaterial;

	public float health = 30f;

	public GameObject explosion;

	private GameObject powerUpControllerGameObject;
	private PowerUpController powerUpController;

	private Collider2D collider;

	private SpriteRenderer casterRenderer;
	private SpriteRenderer shadowRenderer;

	private GameObject shadowObject;
	private bool isDropping;

	private float currentHealth;

	// Use this for initialization
	void Start () {
		currentHealth = health;
		powerUpControllerGameObject = GameObject.FindWithTag ("PowerUpController");
		powerUpController = powerUpControllerGameObject.GetComponent<PowerUpController> ();
		collider = GetComponent<Collider2D> ();
		InitShadow ();
		StartGarbageDrop ();
	}

	// Update is called once per frame
	void Update () {
		
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
		casterRenderer = GetComponent<SpriteRenderer> ();
		shadowRenderer = shadowObject.AddComponent<SpriteRenderer> ();
		shadowRenderer.sprite = casterRenderer.sprite;

		shadowRenderer.material = shadowMaterial;
//		shadowRenderer.material.color = Color.black;
//		shadowRenderer.material.color.a = 0.5f;
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
		}
	}

	public void Damage(float damageTaken) {
		// Damages enemy and handles death
		currentHealth -= damageTaken;
		if (currentHealth <= 0f) {
			Instantiate(explosion, transform.position, Quaternion.identity);
			powerUpController.SpawnRandomPowerUpAtLocation (transform.position.x, transform.position.y);
			Destroy(gameObject);
		}
	}


}
