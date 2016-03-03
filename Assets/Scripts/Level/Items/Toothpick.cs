using UnityEngine;

/// <summary>
/// A projectile that the player can throw.
/// </summary>
public class Toothpick : Item {

	/// <summary> The rigidbody of the toothpick. </summary>
	private Rigidbody body;

	/// <summary> The initial direction the toothpick will travel in. </summary>
	[HideInInspector]
	public Vector3 direction;
	/// <summary> The speed that the toothpick moves at. </summary>
	private const float TRAVELSPEED = 4f;

	/// <summary> Whether to destroy the toothpick after it stops moving. </summary>
	[HideInInspector]
	public bool willDestroy = false;

	/// <summary>
	/// Initializes the toothpick.
	/// </summary>
	protected override void Start() {
		body = GetComponent<Rigidbody>();
		base.Start();
	}

	/// <summary>
	/// Sets up the toothpick just before it is thrown.
	/// </summary>
	public void Initiate() {
		body.velocity = direction * TRAVELSPEED;
		gameObject.SetActive(true);
		Update();
	}

	/// <summary>
	/// Makes the toothpick face the direction it is moving.
	/// </summary>
	private void Update() {
		if (body.velocity != Vector3.zero) {
			transform.rotation = Quaternion.LookRotation(body.velocity);
			transform.eulerAngles -= new Vector3(-90, 0, 0);
		}
	}

	/// <summary>
	/// Kills an enemy if the toothpick hits it.
	/// </summary>
	/// <param name="collision">The collision that the toothpick was involved in.</param>
	private void OnCollisionEnter(Collision collision) {
		Enemy enemy = GetComponent<Collider>().GetComponent<Enemy>();
		if (enemy != null) {
			enemy.KillEntity();
		}
		if (willDestroy) {
			Destroy(gameObject);
		} else {
			gameObject.SetActive(false);
		}
	}
}
