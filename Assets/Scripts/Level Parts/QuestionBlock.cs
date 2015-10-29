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

	// The amount to increase the object's height by every tick.
	float contentIncrement;
	// Timer used to animate contents coming out of the block.
	float contentHeight = 0;
	// The object that is emerging from the block.
	GameObject contentObject;

	// Use this for initialization.
	void Start () {
		startMaterial = GetComponent<Renderer> ().material;
	}

	// Update is called once per frame.
	void Update () {
		if (contentIncrement > 0) {
			PathUtil.SetY (contentObject.transform, contentObject.transform.position.y + contentIncrement);
			contentHeight += contentIncrement;
			if (contentHeight > contentIncrement * 60) {
				contentIncrement = 0;
				if (contentObject.GetComponent<Rigidbody> ()) {
					contentObject.GetComponent<Rigidbody> ().useGravity = true;
				}
				contentObject.GetComponent<Collider> ().enabled = true;
				contentObject.GetComponent<Item> ().EmergeFromBlock ();
				contentObject = null;
			}
		}
	}

	// Spawns the contents of the block above the block.
	public override void HitBlock () {
		if (!wasHit) {
			contentObject = Instantiate (contents);
			contentObject.transform.position = transform.position;
			contentIncrement = GetComponent<Collider> ().bounds.extents.y / 30;
			if (contentObject.GetComponent<Rigidbody> ()) {
			    contentObject.GetComponent<Rigidbody> ().useGravity = false;
			    contentObject.GetComponent<PathMovement> ().moveSpeed = 0;
			}
			contentObject.GetComponent<Collider> ().enabled = false;
			wasHit = true;
			GetComponent<Renderer> ().material = hitMaterial;
		}
	}

	// Regenerates the contents of the block.
	public override void Reset () {
		wasHit = false;
		GetComponent<Renderer> ().material = startMaterial;
		contentIncrement = 0;
		contentHeight = 0;
		contentObject = null;
	}
}
