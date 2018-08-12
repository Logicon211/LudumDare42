using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerUp : PowerUp {

	public override void GivePowerUp() {
		controller.PickupShield();
	}
}
