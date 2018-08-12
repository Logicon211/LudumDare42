using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletController : MonoBehaviour {

	public GameObject hitEffect;
	public float damage = 10f;

	// Use this for initialization
	void Start () {
		
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
		IDamageable<float> damagable = other.GetComponent<IDamageable<float>>();
		if(damagable != null) {
			damagable.Damage(damage);
			Instantiate(hitEffect, transform.position, Quaternion.identity);
			Destroy(gameObject);
		}
	}
}
