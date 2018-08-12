using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpeedPowerUp : PowerUp {

	public override void GivePowerUp() {
		controller.PickupSpeedBoost();
	}
}
