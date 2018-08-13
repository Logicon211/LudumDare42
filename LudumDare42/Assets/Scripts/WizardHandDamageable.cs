using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardHandDamageable : MonoBehaviour, IDamageable<float> {

	public WasteWizard WW;
	private PolygonCollider2D handCollider;

	// Use this for initialization
	void Start () {
		handCollider= GetComponentInParent<PolygonCollider2D>();
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

	public void SwitchCollider(bool boolIn){
		handCollider.enabled = boolIn;
	}

	public void Damage(float damageTaken) {
		// Damages enemy and handles death shit
		WW.GetComponent<WasteWizard>().damageWizard(damageTaken);

}
}
