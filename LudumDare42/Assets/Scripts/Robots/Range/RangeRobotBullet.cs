using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangeRobotBullet : MonoBehaviour {

	public float damage = 25f;

	// Update is called once per frame
	void Update () {
		transform.Rotate(Vector3.forward * 1000f * Time.deltaTime);
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player" ) {
			other.gameObject.GetComponent<PlayerController>().Damage(damage);
			Destroy(gameObject);
		}
	}
}
