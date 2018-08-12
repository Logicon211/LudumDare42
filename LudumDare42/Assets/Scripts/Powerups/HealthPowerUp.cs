using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthPowerUp : PowerUp {

	public override void GivePowerUp() {
		controller.PickupHealth();
	}
}

