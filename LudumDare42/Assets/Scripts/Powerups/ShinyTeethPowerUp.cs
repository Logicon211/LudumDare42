using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShinyTeethPowerUp : PowerUp {

	public override void GivePowerUp() {
		controller.SetPowerUp(6);
	}
}
