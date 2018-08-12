using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikeBall : MonoBehaviour {

	public float spikeBallDamage = 18f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Sent when an incoming collider makes contact with this object's
	/// collider (2D physics only).
	/// </summary>
	/// <param name="other">The Collision2D data associated with this collision.</param>
	void OnCollisionEnter2D(Collision2D other)
	{
		PlayerController player = other.gameObject.GetComponent<PlayerController>();
		if(player != null) {
			player.Damage(spikeBallDamage);
			//TODO: Add a force to him maybe?
		}
	}
}
