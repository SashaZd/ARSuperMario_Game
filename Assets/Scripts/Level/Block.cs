using UnityEngine;

/// <summary>
/// A block that has an effect when hit from underneath.
/// </summary>
public class Block : MonoBehaviour {

	/// <summary> The item inside the block. </summary>
	public Item contents;

	/// <summary>
	/// Checks if a player hit the block from underneath.
	/// </summary>
	/// <param name="collision">The collision that the block was involved in.</param>
	private void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Player") {
			float playerHeadHeight = collision.transform.position.y + collision.collider.bounds.extents.y / 2;
			float blockBaseHeight = transform.position.y - GetComponent<Collider>().bounds.extents.y * 0.55f;
			if (playerHeadHeight < blockBaseHeight) {
				HitBlock();
			}
		}
	}

	/// <summary>
	/// Triggers an effect when the player hits the block from underneath.
	/// </summary>
	public virtual void HitBlock () {
	}

	/// <summary>
	/// Resets the block.
	/// </summary>
	public virtual void Reset () {
	}
}
