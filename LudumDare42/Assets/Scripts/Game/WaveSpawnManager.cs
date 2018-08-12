using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveSpawnManager : MonoBehaviour {


	//This is the spawn list we will use. each array in the array will be 
	public int[,] spawnList = {{1, 1}, {2, 1}};
	public int test;
	
	int numOfLevels;
	private EnemySpawnController spawnController;

	private void Awake() {
		numOfLevels = spawnList.Length - 1;
		spawnController = gameObject.GetComponent<EnemySpawnController>();
	}

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public int GetNumOfLevels() {
		return numOfLevels;
	}
	
	public int GetNumOfEnemiesOnLevel(int level) {
		return spawnList[level, 0] + spawnList[level, 1];
	}

	public void SpawnWave(int level) {
		for (int i = 0; i < spawnList[level, 0]; i++) {
			spawnController.SpawnAtRandomLocation(0, false);
		}
		for (int i = 0; i < spawnList[level, 1]; i++) {
			spawnController.SpawnAtRandomLocation(1, false);
		}
	}
}
