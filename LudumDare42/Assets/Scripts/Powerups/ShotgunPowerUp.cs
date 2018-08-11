using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotgunPowerUp : PowerUp {

	public override void GivePowerUp() {
		controller.SetPowerUp(2);
	}
}
