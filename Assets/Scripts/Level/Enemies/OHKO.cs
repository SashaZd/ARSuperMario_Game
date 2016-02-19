using UnityEngine;
using System.Collections;

// Kills the player instantly (regardless of power-up) upon hitting a trigger.
public class OHKO : MonoBehaviour {

	// Kills the player instantly upon collision.
	void OnTriggerEnter (Collider collider) {
		if (collider.tag == "Player") {
			collider.gameObject.GetComponent<Player> ().KillPlayer ();
		}
	}
}
