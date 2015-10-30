using UnityEngine;
using System.Collections;

// Allows the player to use a fly swatter as a weapon.
public class FlySwatterPower : MonoBehaviour {

	// The toothpick prefab to spawn when firing toothpicks.
	public FlySwatter swatterPrefab;

	// Cooldown timer for throwing toothpicks.
	int cooldown = 0;
	
	// The amount of frames that the throwing cooldown will last.
	const int MAXCOOLDOWN = 30;
	
	// Update is called once per frame.
	void Update () {
		if (!GetComponent<Player> ().HasWon ()) {
			if (cooldown > 0) {
				cooldown--;
			}
			if (cooldown == 0 && Input.GetKey (KeyCode.Return)) {
				cooldown = MAXCOOLDOWN;
				FlySwatter swatter = GameObject.Instantiate (swatterPrefab) as FlySwatter;
				swatter.transform.parent = LevelManager.GetInstance ().player.transform;
				swatter.transform.localPosition = Vector3.forward * 0.5f;
				swatter.transform.localRotation = Quaternion.identity;
			}
		}
	}
}
