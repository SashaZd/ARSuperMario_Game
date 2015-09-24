using UnityEngine;
using System.Collections;

// A block that can be hit from underneath to reveal an item.
public class QuestionBlock : Block {

	// Whether the block has been hit by the player.
	bool wasHit = false;
	// The material the question block appears as before being hit.
	Material startMaterial;
	// The material the question block will take on after being hit.
	public Material hitMaterial;

	// Use this for initialization.
	void Start () {
		startMaterial = GetComponent<Renderer> ().material;
	}

	// Spawns the contents of the block above the block.
	public override void HitBlock () {
		if (!wasHit) {
			GameObject newObject = Instantiate (contents);
			newObject.transform.parent = LevelManager.GetInstance ().gameObject.transform.FindChild ("Items").transform;
			newObject.transform.position = new Vector3 (transform.position.x, transform.position.y + GetComponent<Collider> ().bounds.extents.y, transform.position.z);
			wasHit = true;
			GetComponent<Renderer> ().material = hitMaterial;
		}
	}

	// Regenerates the contents of the block.
	public override void Reset () {
		wasHit = false;
		GetComponent<Renderer> ().material = startMaterial;
	}
}
