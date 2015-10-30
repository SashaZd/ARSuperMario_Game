using UnityEngine;
using System.Collections;

// Item that allows the player to hit things with a fly swatter.
public class FlySwatterItem : Item {

	// The ammo form of the swatter.
	public FlySwatter swatterPrefab;
	
	// Activates the coffee power-up on the player.
	public override void HitPlayer (Player player) {
		FlySwatterPower power = player.GetComponent<FlySwatterPower> ();
		if (!power) {
			power = player.gameObject.AddComponent<FlySwatterPower> ();
			power.swatterPrefab = swatterPrefab;
		}
	}
}
