using UnityEngine;

/// <summary>
/// Item that allows the player to hit things with a fly swatter.
/// </summary>
public class FlySwatterItem : Item {

	/// <summary> The object form of the swatter. </summary>
	[SerializeField]
	[Tooltip("The object form of the swatter.")]
	private FlySwatter swatterPrefab;

	/// <summary>
	/// Activates the swatter power-up on the player.
	/// </summary>
	/// <param name="player">The player who collected the fly swatter.</param>
	public override void HitPlayer (Player player) {
		FlySwatterPower power = player.GetComponent<FlySwatterPower>();
		if (!power) {
			power = player.gameObject.AddComponent<FlySwatterPower>();
			power.swatterPrefab = swatterPrefab;
			player.AddPower(power);
		}
	}
}
