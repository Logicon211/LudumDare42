using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GarbageSpawnController : MonoBehaviour {

	public float xMin = -1f;
	public float xMax = 1f;
	public float yMin = -1f;
	public float yMax = 1f;

	public const int CIRCLE_GARBAGE_INDEX = 0;
	public const int SQUARE_GARBAGE_INDEX = 1;
	public const int RECTANGLE_GARBAGE_INDEX = 2;

	public GameObject[] garbageArray;

	// Use this for initialization
	void Start () {
		
	}

	void Update () {
		//TODO: remove after debugging
		if (Input.GetKeyDown (KeyCode.P)) {
			SpawnRandomGarbageAtRandomLocation ();
		}
	}

	public void SpawnRandomGarbageAtLocation (float xPos, float yPos) {
		int garbageIndex = Random.Range (0, garbageArray.Length);
	}
	
	public void SpawnRandomGarbageAtRandomLocation () {
		int garbageIndex = Random.Range (0, garbageArray.Length);
		SpawnAtLocation (garbageArray[garbageIndex], Random.Range(xMin, xMax), Random.Range (yMin, yMax));
	}

	public void SpawnAtRandomLocation (int garbageIndex) {
		SpawnAtLocation (garbageArray[garbageIndex], Random.Range (xMin, xMax), Random.Range (yMin, yMax));
	}

	public void SpawnAtLocation (int garbageIndex, float xPos, float yPos) {
		SpawnAtLocation (garbageArray [garbageIndex], xPos, yPos);
	}

	private void SpawnAtLocation (GameObject garbageToSpawn, float xPos, float yPos) {
		//Make sure x and y are within the bounds of the level
		xPos = Mathf.Clamp(xPos, xMin, xMax);
		yPos = Mathf.Clamp (yPos, yMin, yMax);

		Vector2 position = new Vector2 (xPos, yPos);
		Quaternion rotation = Quaternion.Euler (0f, 0f, Random.Range (0f, 360f));
		Instantiate (garbageToSpawn, position, rotation);
	}
}
