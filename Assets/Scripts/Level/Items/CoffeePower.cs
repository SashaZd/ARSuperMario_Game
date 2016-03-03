using UnityEngine;

/// <summary>
/// Power-up that speeds up the player.
/// </summary>
public class CoffeePower : MonoBehaviour, Power {

	/// <summary>
	/// Speeds up the player.
	/// </summary>
	private void Start() {
		Player player = GetComponent<Player>();
		player.baseMoveSpeed *= 2;
		player.runSpeed *= 2;
	}

	/// <summary>
	/// Does nothing.
	/// </summary>
	public void PowerKey () {
	}

	/// <summary>
	/// Slows the player down.
	/// </summary>
	public void OnRemove () {
		OnReset ();
	}

	/// <summary>
	/// Slows the player down.
	/// </summary>
	public void OnReset () {
		Player player = GetComponent<Player> ();
		player.baseMoveSpeed /= 2;
		player.runSpeed /= 2;
	}

	/// <summary>
	/// Returns the name of the power-up.
	/// </summary>
	/// <returns>The name of the power-up.</returns>
	public override string ToString () {
		return "Coffee";
	}
}
