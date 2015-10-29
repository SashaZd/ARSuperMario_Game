using UnityEngine;
using System.Collections;

// Allows the player to throw toothpicks at things.
public class ToothpickPower : MonoBehaviour {

	// The toothpick prefab to spawn when firing toothpicks.
	public Toothpick toothpickPrefab;

	// The number of toothpicks the player can fire.
	int numToothpicks = 100;
	// Cooldown timer for throwing toothpicks.
	int cooldown = 0;

	// The amount of frames that the throwing cooldown will last.
	const int MAXCOOLDOWN = 30;

	// Update is called once per frame.
	void Update () {
		if (!GetComponent<Player> ().HasWon ()) {
			if (cooldown > 0) {
				cooldown--;
			}
			if (cooldown == 0 && Input.GetKey (KeyCode.Return)) {
				cooldown = MAXCOOLDOWN;
				Vector3 facing = new Vector3 (Mathf.Sin (transform.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Cos (transform.eulerAngles.y * Mathf.PI / 180));
				Vector3 spawnPosition = transform.position + GetComponent<Collider> ().bounds.extents.z * facing;
				Toothpick toothpick = GameObject.Instantiate (toothpickPrefab, spawnPosition, this.transform.rotation) as Toothpick;
				toothpick.direction = facing;
				if (--numToothpicks <= 0) {
					Destroy (this);
				}
			}
		}
	}

	// Adds a number of toothpicks to the ammo stockpile.
	public void AddToothpicks (int numAdd) {
		numToothpicks += numAdd;
	}
}
