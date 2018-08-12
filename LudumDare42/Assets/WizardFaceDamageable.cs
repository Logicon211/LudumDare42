using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardFaceDamageable : MonoBehaviour, IDamageable<float> {

	public WasteWizard WW;
	private PolygonCollider2D faceCollider;

	// Use this for initialization
	void Start () {
		
		faceCollider= GetComponentInParent<PolygonCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SwitchCollider(bool boolIn){
		faceCollider.enabled = boolIn;
	}

	public void Damage(float damageTaken) {
			// Damages enemy and handles death shit
			WW.GetComponent<WasteWizard>().damageWizard(damageTaken);

	}

}
