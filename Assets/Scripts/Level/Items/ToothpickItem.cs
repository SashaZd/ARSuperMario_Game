using UnityEngine;

/// <summary>
/// Gives the player toothpicks as ammo.
/// </summary>
public class ToothpickItem : Item {

	/// <summary> The ammo form of the toothpick. </summary>
	[SerializeField]
	[Tooltip("The ammo form of the toothpick.")]
	private Toothpick toothpickPrefab;

	/// <summary>
	/// Activates the coffee power-up on the player.
	/// </summary>
	/// <param name="player">The player who collected the toothpick power-up.</param>
	public override void HitPlayer(Player player) {
		ToothpickPower power = player.GetComponent<ToothpickPower>();
		if (power) {
			power.AddToothpicks(1);
		} else {
			power = player.gameObject.AddComponent<ToothpickPower>();
			power.toothpickPrefab = toothpickPrefab;
			player.AddPower(power);
		}
	}
}
