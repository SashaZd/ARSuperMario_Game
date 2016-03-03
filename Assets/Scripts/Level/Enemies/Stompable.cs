using UnityEngine;

/// <summary>
/// An enemy that will kill the player on contact.
/// Landing on it from above will kill it instead.
/// </summary>
public class Stompable : MonoBehaviour {

	/// <summary>
	/// Fires when the enemy hits something.
	/// </summary>
	/// <param name="collision">The collision that the enemy was involved in.</param>
	private void OnCollisionEnter(Collision collision) {
		if (collision.collider.tag == "Player") {
			// Check if the player is high enough to flatten the enemy.
			float playerStompHeight = collision.collider.transform.position.y - collision.collider.bounds.extents.y;
			float enemyHeadHeight = transform.position.y + GetComponent<Collider>().bounds.extents.y * 0.49f;
			if (playerStompHeight > enemyHeadHeight || collision.collider.transform.GetComponent<Rigidbody>().velocity.y < -0.01f) {
				collision.gameObject.GetComponent<Player>().StompEnemy();
				GetComponent<Enemy>().KillEntity();
			} else {
				// If not, the player gets damaged.
				collision.gameObject.GetComponent<Player>().TakeDamage();
			}
		}
	}
}
