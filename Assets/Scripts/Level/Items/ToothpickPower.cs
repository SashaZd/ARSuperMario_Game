using UnityEngine;

/// <summary>
/// Allows the player to throw toothpicks at things.
/// </summary>
public class ToothpickPower : MonoBehaviour, Power {

	/// <summary> The toothpick prefab to spawn when firing toothpicks. </summary>
	[HideInInspector]
	public Toothpick toothpickPrefab;
	/// <summary> Instantiated toothpicks in a factory. </summary>
	private Toothpick[] toothpicks = new Toothpick[2];

	/// <summary> The number of toothpicks the player can fire. </summary>
	private int numToothpicks = 10;
	/// <summary> Cooldown timer for throwing toothpicks. </summary>
	private int cooldown = 0;

	/// <summary> The amount of frames that the throwing cooldown will last. </summary>
	private const int MAXCOOLDOWN = 30;

	/// <summary>
	/// Initializes the toothpick factory.
	/// </summary>
	private void Start() {
		GetComponent<Player>().LosePower(GetComponent<FlySwatterPower>());
		for (int i = 0; i < toothpicks.Length; i++) {
			toothpicks[i] = GameObject.Instantiate(toothpickPrefab) as Toothpick;
		}
	}

	/// <summary>
	/// Decrements the cooldown for throwing toothpicks.
	/// </summary>
	private void Update () {
		if (cooldown > 0) {
			cooldown--;
		}
	}

	/// <summary>
	/// Throws a toothpick if possible.
	/// </summary>
	public void PowerKey () {
		if (cooldown <= 0) {
			Toothpick toothpick = GetToothpick();
			if (toothpick != null) {
				cooldown = MAXCOOLDOWN;
				Vector3 facing = new Vector3(Mathf.Sin(transform.eulerAngles.y * Mathf.PI / 180), 0, Mathf.Cos(transform.eulerAngles.y * Mathf.PI / 180));
				Vector3 spawnPosition = transform.position + GetComponent<Collider>().bounds.extents.z * facing;
				toothpick.transform.position = spawnPosition;
				toothpick.transform.rotation = transform.rotation;
				toothpick.direction = facing;
				toothpick.Initiate();
				if (--numToothpicks <= 0) {
					GetComponent<Player>().LosePower(this);
				}
			}
		}
	}

	/// <summary>
	/// Destroys the toothpicks.
	/// </summary>
	public void OnRemove() {
		OnReset();
	}

	/// <summary>
	/// Removes the power-up from the player when the level is reset.
	/// </summary>
	public void OnReset() {
		foreach (Toothpick toothpick in toothpicks) {
			if (toothpick.gameObject.activeSelf) {
				toothpick.willDestroy = true;
			} else {
				Destroy(toothpick.gameObject);
			}
		}
	}

	/// <summary>
	/// Adds a number of toothpicks to the ammo stockpile.
	/// </summary>
	/// <param name="numAdd">The number of toothpicks to add.</param>
	public void AddToothpicks(int numAdd) {
		numToothpicks += numAdd;
	}

	/// <summary>
	/// Gets a free toothpick from the toothpick factory.
	/// </summary>
	/// <returns>A free toothpick from the toothpick factory.</returns>
	private Toothpick GetToothpick() {
		foreach (Toothpick toothpick in toothpicks) {
			if (!toothpick.gameObject.activeSelf) {
				return toothpick;
			}
		}
		return null;
	}

	/// <summary>
	/// Returns the name of the power-up and the number of toothpicks remaining.
	/// </summary>
	/// <returns>The name of the power-up and the number of toothpicks remaining.</returns>
	public override string ToString () {
		return "Toothpicks x" + numToothpicks;
	}
}
