using UnityEngine;
using System.Collections;

// A collectible item that has an effect on the player.
public class Item : MonoBehaviour {

	bool emerged = false;
	
	// Update is called once per frame.
	void Update () {
		if (PathUtil.OnFloor (gameObject)) {
			Destroy (gameObject);
		}
	}

	// Begins the item's activity after coming out of a question block.
	public virtual void EmergeFromBlock() {
		emerged = true;
	}

	// Powers up the player upon collision.
	void OnCollisionEnter (Collision collision) {
		if (emerged && collision.collider.tag == "Player") {
			Player player = collision.gameObject.GetComponent<Player> ();
			HitPlayer (player);
			Destroy (gameObject);
		}
	}

	// Triggers an effect upon hitting the player.
	public virtual void HitPlayer (Player player) {
	}

	// Removes the item if the level is reset.
	public void Reset () {
		Destroy (gameObject);
	}
}
