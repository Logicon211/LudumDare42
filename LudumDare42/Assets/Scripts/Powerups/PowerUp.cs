using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour {

	protected PlayerController controller;

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			controller = other.gameObject.GetComponent<PlayerController>();
			GivePowerUp();
			DestroyPowerUp();
		}
	}

	public void DestroyPowerUp() {
		Destroy(gameObject);
	}

	public abstract void GivePowerUp();
}
