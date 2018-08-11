using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour {

	public GameObject[] powerupArray;

	private int arrayLength;

	private void Awake() {
		arrayLength = powerupArray.Length - 1;
	}

	public void SpawnPowerUpAtLocation(int powerupIndex, float x, float y) {
		if (powerupIndex <= arrayLength) {
			Instantiate(powerupArray[powerupIndex], new Vector2(x, y), Quaternion.identity);
		}
	}

	public void SpawnRandomPowerUpAtLocation(float x, float y) {
		int powerupIndex = Random.Range (0, arrayLength + 1);
		Instantiate(powerupArray[powerupIndex], new Vector2(x, y), Quaternion.identity);
	}

}
