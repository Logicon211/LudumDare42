using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour {

	public GameObject[] powerupArray;

	private int arrayLength;
	private AudioSource audio;

	private void Awake() {
		audio = GetComponent<AudioSource>();
		arrayLength = powerupArray.Length - 1;
	}

	public GameObject SpawnPowerUpAtLocation(int powerupIndex, float x, float y) {
		if (powerupIndex <= arrayLength) {
			return Instantiate(powerupArray[powerupIndex], new Vector2(x, y), Quaternion.identity);
		}
		return null;
	}

	public GameObject SpawnRandomPowerUpAtLocation(float x, float y) {
		int powerupIndex = Random.Range (0, arrayLength + 1);
		return Instantiate(powerupArray[powerupIndex], new Vector2(x, y), Quaternion.identity);
	}

	public void PlayPickupSound(AudioClip clip) {
		audio.PlayOneShot(clip, 1);
	}
}
