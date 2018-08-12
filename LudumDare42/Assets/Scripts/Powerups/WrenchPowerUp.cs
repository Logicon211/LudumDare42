using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchPowerUp : PowerUp {

	public override void GivePowerUp() {
		controller.PickupWrench();
	}
}
