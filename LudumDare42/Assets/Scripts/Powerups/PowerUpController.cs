using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUpController : MonoBehaviour {

	public GameObject[] powerupArray;

	private int arrayLength;
	private AudioSource AS;

	private GameObject player;
	public Transform [] spawnLocations;

	private void Awake() {
		AS = GetComponent<AudioSource>();
		arrayLength = powerupArray.Length - 1;
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		player = GameObject.FindGameObjectWithTag("Player");
	}

	public GameObject SpawnPowerUpAtLocation(int powerupIndex, float x, float y) {
		if (powerupIndex <= arrayLength) {
			return Instantiate(powerupArray[powerupIndex], new Vector2(x, y), Quaternion.identity);
		}
		return null;
	}

	public GameObject SpawnRandomPowerUpAtLocation(float x, float y) {
		Random.seed = System.DateTime.Now.Millisecond;
		int powerupIndex = Random.Range (0, arrayLength + 1);
		return Instantiate(powerupArray[powerupIndex], new Vector2(x, y), Quaternion.identity);
	}

	public void PlayPickupSound(AudioClip[] clips) {
		if(clips.Length > 0) {
			int clipToPlay = Random.Range(0, clips.Length);
			Debug.Log(clipToPlay);
			if(!AS.isPlaying) {
				AS.clip = clips[clipToPlay];
				AS.Play();
			}
		}
		//AS.PlayOneShot(clip, 1);
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
