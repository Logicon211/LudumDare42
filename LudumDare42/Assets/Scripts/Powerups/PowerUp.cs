using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class PowerUp : MonoBehaviour {

	protected PlayerController controller;
	public AudioClip[] clips;

	private AudioSource audio;
	private PowerUpController p_controller;

	private Rigidbody2D RB;
	private BoxCollider2D powerupCollider;


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
		powerupCollider = GetComponent<BoxCollider2D>();
		powerupCollider.enabled = false;

		Invoke("EnableCollider", 1.4f);

		var x = Random.Range(-1f, 1f);
		var y = Random.Range(-1f, 1f);
		var direction = new Vector2(x, y);
		direction = direction.normalized * 10f;
		
		RB.velocity = direction;
	}

	private void OnTriggerEnter2D(Collider2D other) {
		if (other.gameObject.tag == "Player") {
			controller = other.gameObject.GetComponent<PlayerController>();
			p_controller.PlayPickupSound(clips);
			GivePowerUp();
			DestroyPowerUp();
		}
	}

	public void DestroyPowerUp() {
		Destroy(gameObject);
	}

	public void EnableCollider() {
		powerupCollider.enabled = true;
	}

	public abstract void GivePowerUp();
}
