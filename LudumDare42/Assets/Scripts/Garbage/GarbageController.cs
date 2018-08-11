using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageController : MonoBehaviour, IDamageable<float> {

	public float spawnYOffset = 5f;
	public float dropSpeed = 1f;

	public Material shadowMaterial;

	public float health = 30f;

	public GameObject explosion;

	private SpriteRenderer casterRenderer;
	private SpriteRenderer shadowRenderer;

	private GameObject shadowObject;
	private bool isDropping;

	private float currentHealth;

	// Use this for initialization
	void Start () {
		//InitShadow ();
		//StartGarbageDrop ();

		currentHealth = health;
	}

	// Update is called once per frame
	void Update () {
	}

	void FixedUpdate () {
//		if (isDropping) {
//			DropGarbage ();
//		}
	}

	private void StartGarbageDrop () {
		Vector2 topOfCameraInViewportCoordinates = new Vector2 (1f, 1f);
		Vector2 topOfCameraInWorldCoordinates = Camera.main.ViewportToWorldPoint (topOfCameraInViewportCoordinates);
		float topOfCameraY = topOfCameraInWorldCoordinates.y;
		transform.position = new Vector2 (transform.position.x, topOfCameraY + spawnYOffset); 
		isDropping = true;
	}

	private void InitShadow () {
		shadowObject = new GameObject ();
		shadowObject.transform.position = transform.position;
		casterRenderer = GetComponent<SpriteRenderer> ();
		shadowRenderer = shadowObject.AddComponent<SpriteRenderer> ();
		shadowRenderer.sprite = casterRenderer.sprite;
		shadowRenderer.material = shadowMaterial;
		shadowRenderer.sortingLayerName = casterRenderer.sortingLayerName;
		shadowRenderer.sortingOrder = casterRenderer.sortingOrder - 10;
	}

	private void DropGarbage () {
		float yPos = Mathf.Max (shadowObject.transform.position.y, transform.position.y - (dropSpeed * Time.fixedDeltaTime));
		transform.position = new Vector2 (0f, yPos);
		if (yPos <= shadowObject.transform.position.y) {
			Object.Destroy (shadowObject);
			//TODO: init collider and rigid body
		}
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
