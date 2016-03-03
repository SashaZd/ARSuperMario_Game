/// <summary>
/// Boosts the player's speed and gives an extra hit.
/// </summary>
public class Coffee : Item {

	/// <summary>
	/// Activates the coffee power-up on the player.
	/// </summary>
	/// <param name="player">The player who collected the coffee.</param>
	public override void HitPlayer (Player player) {
		if (player.GetComponent<CoffeePower>() == null) {
			player.AddPower(player.gameObject.AddComponent<CoffeePower>());
		}
	}
}
