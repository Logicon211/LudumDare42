using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LaserPowerup : PowerUp {

	public override void GivePowerUp() {
		controller.SetPowerUp(1);
	}
}
