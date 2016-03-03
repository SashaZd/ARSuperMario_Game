using UnityEngine;

/// <summary>
/// Makes the player larger.
/// </summary>
public class MushroomPower : MonoBehaviour, Power {

	/// <summary>
	/// Increases the player's size.
	/// </summary>
	private void Start() {
		GetComponent<Player>().SetSize(2);
	}

	/// <summary>
	/// Does nothing.
	/// </summary>
	public void PowerKey() {
	}

	/// <summary>
	/// Shrinks the player.
	/// </summary>
	public void OnRemove() {
		OnReset();
	}

	/// <summary>
	/// Shrinks the player.
	/// </summary>
	public void OnReset() {
		GetComponent<Player>().SetSize(1);
	}

	/// <summary>
	/// Returns the name of the power-up.
	/// </summary>
	/// <returns>The name of the power-up.</returns>
	public override string ToString() {
		return "Mushroom";
	}
}
