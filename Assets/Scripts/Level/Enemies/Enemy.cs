using UnityEngine;

/// <summary>
/// An entity that tries to kill the player.
/// </summary>
public class Enemy : MonoBehaviour {

	/// <summary> Whether the enemy can be destroyed. </summary>
	[SerializeField]
	[Tooltip("Whether the enemy can be destroyed.")]
	private bool invincible = false;

	/// <summary>
	/// Checks if the enemy is touching the floor, and kills it if it is.
	/// </summary>
	private void Update() {
		if (PathUtil.OnFloor(gameObject)) {
			KillEntity();
		}
	}

	/// <summary>
	/// Resets the position and direction of the enemy.
	/// </summary>
	public void Reset() {
		gameObject.SetActive(true);
		Movement movement = GetComponent(typeof(Movement)) as Movement;
		if (movement != null) {
			movement.Reset();
		}
	}

	/// <summary>
	/// Kills the enemy.
	/// </summary>
	public void KillEntity() {
		Tracker.Instance.logAction("Enemy killed: " + this.name.Replace("(Clone)", ""));
		if (!invincible) {
			gameObject.SetActive(false);
		}
	}
}
