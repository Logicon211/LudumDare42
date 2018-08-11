using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageController : MonoBehaviour, IDamageable<float> {

	public float garbageSpawnYOffset = 5f;

	public float health = 30f;

	public GameObject explosion;

	private SpriteRenderer casterRenderer;
	private SpriteRenderer shadowRenderer;

	private GameObject shadowObject;
	private bool isGarbageDropping;

	private float currentHealth;

	// Use this for initialization
	void Start () {
		//InitShadow ();
		//StartGarbageDrop ();

		currentHealth = health;
	}

	// Update is called once per frame
	void Update () {
		if (isGarbageDropping) {
			DropGarbage ();
		}
	}

	private void StartGarbageDrop () {
		Vector2 topOfCameraInViewportCoordinates = new Vector2 (1f, 1f);
		Vector2 topOfCameraInWorldCoordinates = Camera.main.ViewportToWorldPoint (topOfCameraInViewportCoordinates);
		float topOfCameraY = topOfCameraInWorldCoordinates.y;
		transform.position = new Vector2 (transform.position.x, topOfCameraY + garbageSpawnYOffset); 
		isGarbageDropping = true;
	}

	private void InitShadow () {
		shadowObject = new GameObject ();
		casterRenderer = GetComponent<SpriteRenderer> ();
		shadowRenderer = shadowObject.AddComponent<SpriteRenderer> ();
	}

	private void DropGarbage () {
		
	}

	public void Damage(float damageTaken) {
		// Damages enemy and handles death shit
		currentHealth -= damageTaken;
		if (currentHealth <= 0f) {
			Instantiate(explosion, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}


}
