/// <summary>
/// A power-up for the player.
/// </summary>
public interface Power {

	/// <summary>
	/// Does an action when the player presses the power-up trigger key.
	/// </summary>
	void PowerKey();

	/// <summary>
	/// Removes the power-up from the player when hit.
	/// </summary>
	void OnRemove();

	/// <summary>
	/// Removes the power-up from the player when the level is reset.
	/// </summary>
	void OnReset();
}
