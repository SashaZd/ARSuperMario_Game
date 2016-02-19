using UnityEngine;
using System.Collections;

// A collectible that adds to the player's score.
public class Coin : Item {

	// Removes the coin after it is collected by a player.
	public override void HitPlayer (Player player) {
		player.score += 100;
	}
}
