using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour {

	protected PlayerController controller;

	private AudioSource audio;
	private PowerUpController p_controller;

	private Rigidbody2D RB;

	private void Awake() {
		audio = GetComponent<AudioSource>();
		p_controller = GameObject.FindWithTag("PowerUpController").GetComponent<PowerUpController>();
	}

	/// <summary>
	/// Start is called on the frame when a script is enabled just before
	/// any of the Update methods is called the first time.
	/// </summary>
	void Start()
	{
		RB = GetComponent<Rigidbody2D>();
		RB.velocity = new Vector2(40f, 0f);
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
