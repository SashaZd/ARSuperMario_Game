using UnityEngine;
using System.Collections;

// A collectible item that has an effect on the player.
public class Item : MonoBehaviour {

	// The initial position of the item when the level started.
	Vector3 initPosition;
	// Whether the item has its initial position initiated.
	bool hasInit;
	// Whether the item has emerged from the block containing it.
	bool emerged = false;
	// Whether the item will be untracked by the level manager.
	public bool willRemove = false;

	// Use this for initialization.
	protected virtual void Start () {
		LevelManager manager = LevelManager.GetInstance ();
		transform.parent = manager.gameObject.transform.FindChild ("Items").transform;
		manager.items.Add (this);
	}
	
	// Update is called once per frame.
	void Update () {
		if (PathUtil.OnFloor (gameObject)) {
			Remove ();
		}
	}

	// Begins the item's activity after coming out of a question block.
	public virtual void EmergeFromBlock() {
		emerged = true;
	}

	// Powers up the player upon collision.
	void OnCollisionEnter (Collision collision) {
		OnTriggerEnter (collision.collider);
	}

	// Powers up the player upon collision.
	void OnTriggerEnter (Collider collider) {
		if (emerged && collider.tag == "Player") {
			Player player = collider.gameObject.GetComponent<Player> ();
			HitPlayer (player);
			Remove ();
		}
	}

	// Triggers an effect upon hitting the player.
	public virtual void HitPlayer (Player player) {
	}

	// Sets the initial position of the item when the level starts.
	public void SetInitPosition (Vector3 newPosition) {
		initPosition = newPosition;
		hasInit = true;
		emerged = true;
	}

	// Removes the item from the world.
	public void Remove () {
		if (hasInit) {
			gameObject.SetActive (false);
		} else {
			LevelManager manager = LevelManager.GetInstance ();
			if (manager.itemIterating) {
				willRemove = true;
			} else {
				manager.items.Remove (this);
			}
			Destroy (gameObject);
		}
	}

	// Removes the item if the level is reset.
	public void Reset () {
		if (hasInit) {
			gameObject.SetActive (true);
			transform.rotation = Quaternion.identity;
			transform.position = initPosition;
		} else {
			Remove ();
		}
	}
}
