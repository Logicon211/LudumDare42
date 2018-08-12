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

	public Transform [] spawnLocations;

	private GameObject player;

	// Use this for initialization
	void Start () {
		player = GameObject.FindGameObjectWithTag("Player");
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SpawnAtRandomLocation (int enemyIndex) {
		Vector2 spawnLocation = PickSpawnPointNotOnPlayer();
		SpawnAtLocation (robotArray[enemyIndex], spawnLocation.x, spawnLocation.y);
	}

	private void SpawnAtLocation (GameObject enemyToSpawn, float xPos, float yPos) {
		//Make sure x and y are within the bounds of the level
		xPos = Mathf.Clamp(xPos, xMin, xMax);
		yPos = Mathf.Clamp (yPos, yMin, yMax);

		Vector2 position = new Vector2 (xPos, yPos);
		Quaternion rotation = Quaternion.Euler (0f, 0f, Random.Range (0f, 360f));
		//garbageToSpawn.GetComponent<GarbageController> ().dropFromSky = dropFromSky; 
		//Debug.Log ("drop from sky: " + dropFromSky);
		//Debug.Log ("garbageToSpawn dropFromSky: " + garbageToSpawn.GetComponent<GarbageController> ().dropFromSky);
		Instantiate (enemyToSpawn, position, rotation);
	}

	public void SpawnBoss (int enemyIndex) {
		Vector2 spawnLocation = PickSpawnPointFurthestFromPlayer();
		SpawnAtLocation (robotArray[enemyIndex], spawnLocation.x, spawnLocation.y);
		//SpawnAtLocation(robotArray[enemyIndex], spawnLocation.position.x, spawnLocation.position.y);
	}

	public void FinalBossActivate() {
		//Activate
		GameObject wizard = GameObject.Find("Waste Wizard");
		if(wizard != null) {
			WasteWizard wizardController = wizard.GetComponent<WasteWizard>();
			wizardController.ChangePhase();
		}
	}

	public Vector2 PickSpawnPointNotOnPlayer() {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player");
		}
		Transform closestPoint = null;
		foreach(Transform point in spawnLocations) {
			if(closestPoint == null || Vector2.Distance(point.position, player.transform.position) < Vector2.Distance(closestPoint.position, player.transform.position)) {
				closestPoint = point;
			}
		}

		Vector2 chosenPosition;
		bool spawnPointChosen = false;
		while (!spawnPointChosen) {
			chosenPosition = spawnLocations[Random.Range(0, spawnLocations.Length)].position;

			if(!chosenPosition.Equals(closestPoint.position)) {
				return chosenPosition;
			}
		}

		return new Vector2();
	}

	public Vector2 PickSpawnPointFurthestFromPlayer() {
		if (player == null) {
			player = GameObject.FindGameObjectWithTag("Player");
		}
		Transform furthestPoint = null;
		foreach(Transform point in spawnLocations) {
			if(furthestPoint == null || Vector2.Distance(point.position, player.transform.position) > Vector2.Distance(furthestPoint.position, player.transform.position)) {
				furthestPoint = point;
			}
		}

		return new Vector2(furthestPoint.position.x, furthestPoint.position.y);
	}
}
