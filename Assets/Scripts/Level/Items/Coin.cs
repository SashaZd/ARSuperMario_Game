/// <summary>
/// A collectible that adds to the player's score.
/// </summary>
public class Coin : Item {

	/// <summary>
	/// Removes the coin after it is collected by a player.
	/// </summary>
	/// <param name="player">The player who collected the coin.</param>
	public override void HitPlayer (Player player) {
		player.score += 100;
	}
}
