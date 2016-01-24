using UnityEngine;
using System.Collections;

// Allows the player to throw toothpicks at things.
public class ToothpickPower : MonoBehaviour, Power {

	// The toothpick prefab to spawn when firing toothpicks.
	public Toothpick toothpickPrefab;
	// Instantiated toothpicks in a factory.
	Toothpick[] toothpicks = new Toothpick[2];

	// The number of toothpicks the player can fire.
	int numToothpicks = 10;
	// Cooldown timer for throwing toothpicks.
	int cooldown = 0;

	// The amount of frames that the throwing cooldown will last.
	const int MAXCOOLDOWN = 30;

	// Initializes the toothpick factory.
	void Start () {
		GetComponent<Player> ().LosePower (GetComponent<FlySwatterPower> ());
		for (int i = 0; i < toothpicks.Length; i++) {
			toothpicks[i] = GameObject.Instantiate (toothpickPrefab) as Toothpick;
		}
	}

	// Update is called once per frame.
	void Update () {
		if (cooldown > 0) {
			cooldown--;
		}
	}

	// Does an action when the player presses the power-up trigger key.
	public void PowerKey () {
		if (cooldown <= 0) {
			Toothpick toothpick = GetToothpick ();
			if (toothpick != null) {
				cooldown = MAXCOOLDOWN;
				Vector3 facing = new Vector3 (Mathf.Sin (transform.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Cos (transform.eulerAngles.y * Mathf.PI / 180));
				Vector3 spawnPosition = transform.position + GetComponent<Collider> ().bounds.extents.z * facing;
				toothpick.transform.position = spawnPosition;
				toothpick.transform.rotation = transform.rotation;
				toothpick.direction = facing;
				toothpick.Initiate ();
				if (--numToothpicks <= 0) {
					GetComponent<Player> ().LosePower (this);
				}
			}
		}
	}

	
	// Destroys the toothpicks.
	public void OnRemove () {
		OnReset ();
	}
	
	// Removes the power-up from the player when the level is reset.
	public void OnReset () {
		foreach (Toothpick toothpick in toothpicks) {
			if (toothpick.gameObject.activeSelf) {
				toothpick.willDestroy = true;
			} else {
				Destroy (toothpick.gameObject);
			}
		}
	}

	// Adds a number of toothpicks to the ammo stockpile.
	public void AddToothpicks (int numAdd) {
		numToothpicks += numAdd;
	}

	// Gets a free toothpick from the toothpick factory.
	Toothpick GetToothpick () {
		foreach (Toothpick toothpick in toothpicks) {
			if (!toothpick.gameObject.activeSelf) {
				return toothpick;
			}
		}
		return null;
	}

	public override string ToString () {
		return "Toothpicks x" + numToothpicks;
	}
}
