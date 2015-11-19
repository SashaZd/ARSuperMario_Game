using UnityEngine;
using System.Collections;

// Allows the player to use a fly swatter as a weapon.
public class FlySwatterPower : MonoBehaviour, Power {

	// The swatter prefab to use to spawn a swatter.
	public FlySwatter swatterPrefab;
	// Instance of the swatter object.
	FlySwatter swatterInstance;

	// Cooldown timer for swinging the swatter.
	int cooldown = 0;
	
	// The amount of frames that the swing cooldown will last.
	const int MAXCOOLDOWN = 30;

	// Initializes the fly swatter object.
	void Start () {
		GetComponent<Player> ().LosePower (GetComponent<ToothpickPower> ());
		swatterInstance = GameObject.Instantiate (swatterPrefab) as FlySwatter;
		swatterInstance.gameObject.SetActive (false);
		swatterInstance.transform.parent = transform;
	}

	// Decrements the power-up cooldown.
	void Update () {
		if (cooldown > 0) {
			cooldown--;
		}
	}

	// Does an action when the player presses the power-up trigger key.
	public void PowerKey () {
		if (!swatterInstance.gameObject.activeSelf && cooldown <= 0) {
			cooldown = MAXCOOLDOWN;
			swatterInstance.Initiate ();
		}
	}
	
	// Set the swatter object to be destroyed.
	public void OnRemove () {
		swatterInstance.willDestroy = true;
	}
	
	// Destroy the swatter object.
	public void OnReset () {
		Destroy (swatterInstance.gameObject);
	}
}
