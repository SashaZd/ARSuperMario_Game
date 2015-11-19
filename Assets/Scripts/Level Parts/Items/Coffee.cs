using UnityEngine;
using System.Collections;

// Boosts the player's speed and gives an extra hit.
public class Coffee : Item {

	// Activates the coffee power-up on the player.
	public override void HitPlayer (Player player) {
		if (player.GetComponent<CoffeePower> () == null) {
			player.AddPower (player.gameObject.AddComponent<CoffeePower> ());
		}
	}
}
