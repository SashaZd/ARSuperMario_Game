using UnityEngine;
using System.Collections;

// Power-up that speeds up the player.
public class CoffeePower : MonoBehaviour, Power {

	// Speeds up the player.
	void Start () {
		Player player = GetComponent<Player> ();
		player.baseMoveSpeed *= 2;
		player.runSpeed *= 2;
	}

	// Do nothing.
	public void PowerKey () {
	}
	
	// Slows the player down.
	public void OnRemove () {
		OnReset ();
	}
	
	// Slows the player down.
	public void OnReset () {
		Player player = GetComponent<Player> ();
		player.baseMoveSpeed /= 2;
		player.runSpeed /= 2;
	}
}
