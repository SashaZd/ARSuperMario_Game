using UnityEngine;
using System.Collections;

// A collectible item that has an effect on the player.
public class Item : MonoBehaviour {

	void Update () {
		if (PathUtil.OnFloor (gameObject)) {
			Destroy (gameObject);
		}
	}

	// Removes the item if the level is reset.
	public void Reset () {
		Destroy (gameObject);
	}
}
