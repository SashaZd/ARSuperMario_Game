using UnityEngine;

/// <summary>
/// A collectible item that has an effect on the player.
/// </summary>
public class Item : MonoBehaviour {

	/// <summary> The initial position of the item when the level started. </summary>
	private Vector3 initPosition;
	/// <summary> Whether the item has its initial position initiated. </summary>
	private bool hasInit;
	/// <summary> Whether the item has emerged from the block containing it. </summary>
	private bool emerged = false;
	/// <summary> Whether the item will be untracked by the level manager. </summary>
	[HideInInspector]
	public bool willRemove = false;

	/// <summary>
	/// Registers the item in the level manager.
	/// </summary>
	protected virtual void Start() {
		LevelManager manager = LevelManager.Instance;
		transform.parent = manager.gameObject.transform.FindChild("Items").transform;
		manager.items.Add(this);
	}

	/// <summary>
	/// Destroys the item if it falls on the floor.
	/// </summary>
	private void Update() {
		if (PathUtil.OnFloor(gameObject)) {
			Remove();
		}
	}

	/// <summary>
	/// Begins the item's activity after coming out of a question block.
	/// </summary>
	public virtual void EmergeFromBlock() {
		emerged = true;
	}

	/// <summary>
	/// Powers up the player upon collision.
	/// </summary>
	/// <param name="collision">The collision that the item was involved in.</param>
	private void OnCollisionEnter(Collision collision) {
		OnTriggerEnter(collision.collider);
	}

	/// <summary>
	/// Powers up the player upon collision.
	/// </summary>
	/// <param name="collider">The collider that the item hit.</param>
	private void OnTriggerEnter(Collider collider) {
		if (emerged && collider.tag == "Player") {
			Player player = collider.gameObject.GetComponent<Player>();
			HitPlayer(player);
			Remove();
		}
	}

	/// <summary>
	/// Triggers an effect upon hitting the player.
	/// </summary>
	/// <param name="player">The player hit by the item.</param>
	public virtual void HitPlayer(Player player) {
	}

	/// <summary>
	/// Sets the initial position of the item when the level starts.
	/// </summary>
	/// <param name="newPosition">The initial position of the item.</param>
	public void SetInitPosition(Vector3 newPosition) {
		initPosition = newPosition;
		hasInit = true;
		emerged = true;
	}

	/// <summary>
	/// Removes the item from the world.
	/// </summary>
	private void Remove() {
		if (hasInit) {
			gameObject.SetActive (false);
		} else {
			LevelManager manager = LevelManager.Instance;
			if (manager.itemIterating) {
				willRemove = true;
			} else {
				manager.items.Remove (this);
			}
			if (this != null) {
				Destroy(gameObject);
			}
		}
	}

	/// <summary>
	/// Removes the item if the level is reset.
	/// </summary>
	public void Reset () {
		if (hasInit) {
			transform.rotation = Quaternion.identity;
			transform.position = initPosition;
			gameObject.SetActive(true);
			Movement movement = GetComponent<Movement>();
			if (movement != null) {
				movement.Reset();
			}
		} else {
			Remove();
		}
	}
}
