using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour {

	protected PlayerController controller;

	private AudioSource audio;
	private PowerUpController p_controller;

	private void Awake() {
		audio = GetComponent<AudioSource>();
		p_controller = GameObject.FindWithTag("PowerUpController").GetComponent<PowerUpController>();
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			controller = other.gameObject.GetComponent<PlayerController>();
			p_controller.PlayPickupSound(audio.clip);
			GivePowerUp();
			DestroyPowerUp();
		}
	}

	public void DestroyPowerUp() {
		Destroy(gameObject);
	}

	public abstract void GivePowerUp();
}
