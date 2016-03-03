using UnityEngine;

/// <summary>
/// A melee weapon the player can use.
/// </summary>
public class FlySwatter : MonoBehaviour {

	/// <summary> The rigidbody of the swatter. </summary>
	private Rigidbody body;
	/// <summary> The maximum amount of time the swatter can exist. </summary>
	private const int TIMELIMIT = 18;
	/// <summary> The time that the swatter has existed. </summary>
	private int timer = 0;
	/// <summary> The initial local scale of the swatter. </summary>
	private Vector3 initScale;
	/// <summary> Whether the swatter has hit something. </summary>
	private bool hit;
	/// <summary> Whether hit will be set to true next frame. </summary>
	private bool willHit = false;
	/// <summary> Whether the swatter will be destroyed after its animation finishes. </summary>
	[HideInInspector]
	public bool willDestroy = false;

	/// <summary>
	/// Moves the fly swatter to its beginning position.
	/// </summary>
	public void Initiate() {
		timer = 0;
		willHit = false;
		hit = false;
		transform.localPosition = Vector3.forward * 0.5f;
		transform.localRotation = Quaternion.identity;
		transform.localScale = Vector3.one;
		initScale = transform.localScale;
		transform.parent.GetComponent<Player>().canMove = false;
		gameObject.SetActive(true);
	}

	/// <summary>
	/// Moves the fly swatter when it is deployed.
	/// </summary>
	private void Update () {
		if (GameMenuUI.paused) {
			return;
		}
		if (willHit) {
			hit = true;
		}
		if (!hit) {
			float changeAngle = transform.localEulerAngles.x + 10;
			float angleSin = Mathf.Sin(changeAngle * Mathf.Deg2Rad);
			transform.localEulerAngles = PathUtil.SetX(transform.localEulerAngles, changeAngle);
			transform.localScale = PathUtil.SetY(transform.localScale, initScale.y + angleSin / 1);
			if (changeAngle == 100) {
				willHit = true;
			}
		}
		if (timer++ > TIMELIMIT) {
			transform.parent.GetComponent<Player>().canMove = true;
			if (willDestroy) {
				Destroy(gameObject);
			} else {
				gameObject.SetActive (false);
			}
		}
	}

	/// <summary>
	/// Triggers events when colliding with certain objects.
	/// </summary>
	/// <param name="collider">The collider that the fly swatter hit.</param>
	private void OnTriggerEnter(Collider collider) {
		if (!collider.isTrigger && !hit) {
			Enemy enemy = collider.GetComponent<Enemy>();
			if (enemy != null) {
				enemy.KillEntity();
			}
			hit = true;
		}
	}
}
