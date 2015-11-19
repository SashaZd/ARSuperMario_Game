using UnityEngine;
using System.Collections;

// A power-up for the player.
public interface Power {

	// Does an action when the player presses the power-up trigger key.
	void PowerKey ();

	// Removes the power-up from the player when hit.
	void OnRemove ();

	// Removes the power-up from the player when the level is reset.
	void OnReset ();
}
