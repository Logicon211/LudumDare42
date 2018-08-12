using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPowerUp : PowerUp {

	public override void GivePowerUp() {
		controller.PickupLazer();
	}
}
