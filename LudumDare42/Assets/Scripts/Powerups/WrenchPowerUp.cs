using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WrenchPwerUp : PowerUp {

	public override void GivePowerUp() {
		controller.SetPowerUp(4);
	}
}
