using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardFistController : MonoBehaviour {

	public Material shadowMaterial;

	public float spawnYOffset = 5f;
	public float dropSpeed = 15f;
	public float timeForFistToRemainOnGround = 1;

	private float defaultFistOnGroundTime;

	private Collider2D collider;

	private SpriteRenderer casterRenderer;
	private SpriteRenderer shadowRenderer;

	private GameObject shadowObject;
	private bool isDropping = false;
	private bool isOnGround = false;
	public GarbageSpawnController garbage;

	private Vector2 shadowPosition;

	// Use this for initialization
	void Start () {
		defaultFistOnGroundTime = timeForFistToRemainOnGround;
		casterRenderer = GetComponent<SpriteRenderer> ();
		casterRenderer.enabled = false;
		collider = GetComponent<Collider2D> ();
		collider.enabled = false;
		shadowPosition = new Vector2 (0f, 0f);
	}
	
	// Update is called once per frame
	void Update () {

//		if (Input.GetKeyDown (KeyCode.R) && gameObject.name.Equals ("rightHandTest")) {
//			DropFistAtLocation (5, 0);
//		}
//
//		if (Input.GetKeyDown (KeyCode.L) && gameObject.name.Equals ("leftHandTest")) {
//			DropFistAtLocation (-5, 0);
//		}

		if (isDropping) {
			DropFist ();
		}
		if (isOnGround) {
			FistOnGround ();
		}
	}

	public void DropFistAtLocation (float xPos, float yPos) {
		shadowPosition = new Vector2 (xPos, yPos);
		InitShadow ();
		casterRenderer.enabled = true;
		StartFistDrop ();
	}

	private void InitShadow () {
		shadowObject = new GameObject ();
		shadowObject.name = "Fist Shadow";
		shadowObject.transform.position = new Vector3 (shadowPosition.x, shadowPosition.y, -1);
		shadowObject.transform.localScale = transform.localScale;
		shadowObject.transform.rotation = transform.rotation;
		shadowRenderer = shadowObject.AddComponent<SpriteRenderer> ();
		shadowRenderer.sprite = casterRenderer.sprite;
		shadowRenderer.material = shadowMaterial;
		shadowRenderer.sortingLayerName = casterRenderer.sortingLayerName;
		shadowRenderer.sortingOrder = casterRenderer.sortingOrder + 10;
	}

	private void StartFistDrop () {
		//Disable collider until fist drops into place
		collider.enabled = false;

		//Find top of camera to ensure fist never spawns in view of player
		Vector2 topOfCameraInViewportCoordinates = new Vector2 (1f, 1f);
		Vector2 topOfCameraInWorldCoordinates = Camera.main.ViewportToWorldPoint (topOfCameraInViewportCoordinates);
		float topOfCameraY = topOfCameraInWorldCoordinates.y;

		//Spawn fist above cameras max y value
		transform.position = new Vector2 (shadowPosition.x, topOfCameraY + spawnYOffset);
		casterRenderer.sortingLayerName = "Falling Fist";
		isDropping = true;
	}

	private void DropFist () {
		float yPos = Mathf.Max (shadowObject.transform.position.y, transform.position.y - (dropSpeed * Time.fixedDeltaTime));
		transform.position = new Vector3 (transform.position.x, yPos, -4);
		if (yPos <= shadowObject.transform.position.y) {
			Destroy (shadowObject);
			collider.enabled = true;
			casterRenderer.sortingLayerName = "Fists";
			isDropping = false;
			isOnGround = true;
			CheckCollisionWithPlayer ();
		}
	}

	private void CheckCollisionWithPlayer () {
			garbage.SpawnRandomGarbageAtRandomLocation(true);
			garbage.SpawnRandomGarbageAtRandomLocation(true);
			garbage.SpawnRandomGarbageAtRandomLocation(true);
			garbage.SpawnRandomGarbageAtRandomLocation(true);

			Camera.main.GetComponent<Camera_controller>().CameraShake();
		Collider2D[] contacts = new Collider2D[5];
		ContactFilter2D filter = new ContactFilter2D ();
		collider.GetComponent<Collider2D> ().OverlapCollider (filter.NoFilter(), contacts);
		foreach (Collider2D collision in contacts) {
			if (collision != null) {
				PlayerController playerController = collision.gameObject.GetComponent<PlayerController> ();
				if (playerController != null) {
					playerController.Damage (30f);
					return;
				}
			}
		}
	}

	private void FistOnGround () {
		timeForFistToRemainOnGround -= Time.deltaTime;
		if (timeForFistToRemainOnGround <= 0) {
			timeForFistToRemainOnGround = defaultFistOnGroundTime;
			isOnGround = false;
			casterRenderer.enabled = false;
			collider.enabled = false;
		}
	}
}
