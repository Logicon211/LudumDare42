using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class WaveSpawnManager : MonoBehaviour {


	//This is the spawn list we will use. each array in the array will be 
	public int[,] spawnList = {{2, 0, 0}, {1, 1, 0}, {2, 1, 0}, {-1, 0, 0}, {2, 0, 1}, {2, 2, 0}, {-2, 0, 0}, {2, 1, 2}};
	public int test;
	
	int numOfLevels;
	private EnemySpawnController spawnController;

	private void Awake() {
		numOfLevels = spawnList.GetLength(0) - 1;
		Debug.Log("number of levels -1 " + numOfLevels);
		spawnController = gameObject.GetComponent<EnemySpawnController>();
	}

	public int GetNumOfLevels() {
		return numOfLevels;
	}
	
	public int GetNumOfEnemiesOnLevel(int level) {
		int total = spawnList[level, 0] + spawnList[level, 1];
		return total;
	}

	public int GetNumOfBossesOnLevel(int level) {
		int total = 0;
		if(spawnList[level, 2] > 0) {
			total++;
		}
		return total;
	}

	public void SpawnWave(int level) {
		for (int i = 0; i < spawnList[level, 0]; i++) {
			spawnController.SpawnAtRandomLocation(0);
		}
		for (int i = 0; i < spawnList[level, 1]; i++) {
			spawnController.SpawnAtRandomLocation(1);
		}

		if(spawnList[level, 2] == 1) {
			//Senator Swill
			spawnController.SpawnBoss(EnemySpawnController.FIRST_BOSS_INDEX);

		} else if (spawnList[level, 2] == 2) {
			//Waste Wizard boss mode
			Debug.Log("START BOSS");
			spawnController.FinalBossActivate();
		}
	}
}
