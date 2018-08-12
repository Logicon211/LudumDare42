using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawnController : MonoBehaviour {

	public float xMin = -1f;
	public float xMax = 1f;
	public float yMin = -1f;
	public float yMax = 1f;

	public const int MELEE_ROBOT_INDEX = 0;
	public const int RANGED_ROBOT_INDEX = 1;
	public const int FIRST_BOSS_INDEX = 2;
	public const int SECOND_BOSS_INDEX = 3;


	public GameObject[] robotArray;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnAtRandomLocation (int garbageIndex, bool dropFromSky = true) {
		SpawnAtLocation (robotArray[garbageIndex], Random.Range (xMin, xMax), Random.Range (yMin, yMax), dropFromSky);
	}

	private void SpawnAtLocation (GameObject garbageToSpawn, float xPos, float yPos, bool dropFromSky = true) {
		//Make sure x and y are within the bounds of the level
		xPos = Mathf.Clamp(xPos, xMin, xMax);
		yPos = Mathf.Clamp (yPos, yMin, yMax);

		Vector2 position = new Vector2 (xPos, yPos);
		Quaternion rotation = Quaternion.Euler (0f, 0f, Random.Range (0f, 360f));
		//garbageToSpawn.GetComponent<GarbageController> ().dropFromSky = dropFromSky; 
		//Debug.Log ("drop from sky: " + dropFromSky);
		//Debug.Log ("garbageToSpawn dropFromSky: " + garbageToSpawn.GetComponent<GarbageController> ().dropFromSky);
		Instantiate (garbageToSpawn, position, rotation);
	}
}
