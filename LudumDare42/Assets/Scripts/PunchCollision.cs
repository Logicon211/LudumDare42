using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PunchCollision : MonoBehaviour {

	PlayerController playerController;
	public GameObject hitEffect;
	// Use this for initialization
	void Start () {
		playerController = transform.parent.gameObject.GetComponent<PlayerController>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Sent when another object enters a trigger collider attached to this
	/// object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerEnter2D(Collider2D other)
	{
		if(playerController.Dashing) {

			IDamageable<float> damageable = other.gameObject.GetComponent<IDamageable<float>>();
			if(damageable != null) {
				damageable.Damage(playerController.dashDamage);
				Instantiate(hitEffect, other.transform.position, Quaternion.identity);
			}
		}
	}
}
