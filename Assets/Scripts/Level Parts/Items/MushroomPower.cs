using UnityEngine;
using System.Collections;

// Makes the player larger.
public class MushroomPower : MonoBehaviour, Power {

	// Increase the player's size.
	void Start () {
		GetComponent<Player> ().SetSize (2);
	}
	
	// Do nothing.
	public void PowerKey () {
	}
	
	// Shrink the player.
	public void OnRemove () {
		OnReset ();
	}
	
	// Shrink the player.
	public void OnReset () {
		GetComponent<Player> ().SetSize (1);
	}
}
