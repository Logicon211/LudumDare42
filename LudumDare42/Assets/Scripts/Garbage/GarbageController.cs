using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageController : MonoBehaviour {

	public float garbageSpawnYOffset = 5f;

	private SpriteRenderer casterRenderer;
	private SpriteRenderer shadowRenderer;

	private GameObject shadowObject;
	private bool isGarbageDropping;

	// Use this for initialization
	void Start () {
		//InitShadow ();
		//StartGarbageDrop ();
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


}
