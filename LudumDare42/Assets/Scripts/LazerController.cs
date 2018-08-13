using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LazerController : MonoBehaviour {

	public float damagePerFrame = 2f;
	public float damageToGarbagePerFrame = 0.5f;
	public GameObject hitEffect;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	/// <summary>
	/// Sent each frame where another object is within a trigger collider
	/// attached to this object (2D physics only).
	/// </summary>
	/// <param name="other">The other Collider2D involved in this collision.</param>
	void OnTriggerStay2D(Collider2D other)
	{
		IDamageable<float> damageable = other.gameObject.GetComponent<IDamageable<float>>();
		if(damageable != null) {
			if(other.gameObject.layer == LayerMask.NameToLayer("Terrain")) {
				damageable.Damage(damageToGarbagePerFrame);
			} else {
				damageable.Damage(damagePerFrame);
			}
			Instantiate(hitEffect, other.transform.position, Quaternion.identity);
		}
	}
}
