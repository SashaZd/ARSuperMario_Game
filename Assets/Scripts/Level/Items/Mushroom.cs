/// <summary>
/// Makes the player grow in size and gain an extra hit.
/// </summary>
public class Mushroom : Item {

	/// <summary> The movement speed of the mushroom. </summary>
	private float moveSpeed = 0.005f;
	/// <summary> The pathing system for the mushroom. </summary>
	private PathMovement movement;

	/// <summary>
	/// Places the mushroom on the closest path.
	/// </summary>
	protected override void Start() {
		movement = GetComponent<PathMovement>();
		PathUtil.FindClosestPath(transform.position, movement);
		base.Start();
	}

	/// <summary>
	/// Makes the mushroom start moving after emerging from a question block.
	/// </summary>
	public override void EmergeFromBlock() {
		movement.moveSpeed = moveSpeed;
		base.EmergeFromBlock();
	}

	/// <summary>
	/// Increases the player's size.
	/// </summary>
	/// <param name="player">The player hit by the mushroom.</param>
	public override void HitPlayer(Player player) {
		if (player.GetComponent<MushroomPower>() == null) {
			player.AddPower(player.gameObject.AddComponent<MushroomPower>());
		}
	}
}
