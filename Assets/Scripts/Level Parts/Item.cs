using UnityEngine;
using System.Collections;

// A collectible item that has an effect on the player.
public class Item : MonoBehaviour {
	
	// Update is called once per frame.
	void Update () {
		if (PathUtil.OnFloor (gameObject)) {
			Destroy (gameObject);
		}
	}

	// Begins the item's activity after coming out of a question block.
	public virtual void EmergeFromBlock() {
	}

	// Removes the item if the level is reset.
	public void Reset () {
		Destroy (gameObject);
	}
}
