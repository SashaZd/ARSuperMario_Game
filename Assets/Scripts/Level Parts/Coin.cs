using UnityEngine;
using System.Collections;

// A collectible that adds to the player's score.
public class Coin : MonoBehaviour {
	
	// Update is called once per frame.
	void Update () {
		transform.Rotate (new Vector3 (0f, 2f, 0f));
	}

	// Removes the coin after it is collected by a player.
	void OnTriggerEnter(Collider collision) {
		if (collision.GetComponent<Collider>().tag == "Player") {
			gameObject.SetActive (false);
			collision.gameObject.GetComponent<Player> ().CollectCoin ();
		}
	}

	// Restores the coin if it was claimed.
	public void Reset () {
		gameObject.SetActive (true);
		transform.rotation = Quaternion.identity;
	}
}
