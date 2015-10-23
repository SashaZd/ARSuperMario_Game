using UnityEngine;
using System.Collections;

// Gives the player toothpicks as ammo.
public class ToothpickItem : Item {

	// The ammo form of the toothpick.
	public Toothpick toothpickPrefab;

	// Activates the coffee power-up on the player.
	public override void HitPlayer (Player player) {
		ToothpickPower power = player.GetComponent<ToothpickPower> ();
		if (power) {
			power.AddToothpicks (1);
		} else {
			power = player.gameObject.AddComponent<ToothpickPower> ();
			power.toothpickPrefab = toothpickPrefab;
		}
	}
}
