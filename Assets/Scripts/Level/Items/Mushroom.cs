using UnityEngine;
using System;

// Makes the player grow in size and gain an extra hit.
public class Mushroom : Item {

	// The movement speed of the mushroom.
	public float moveSpeed = 0.005f;
	// The pathing system for the mushroom.
	PathMovement movement;
	
	// Use this for initialization.
	protected override void Start () {
		movement = GetComponent<PathMovement> ();
		PathUtil.FindClosestPath (transform.position, movement);
		base.Start ();
	}

	// Makes the mushroom start moving after emerging from a question block.
	public override void EmergeFromBlock () {
		movement.moveSpeed = moveSpeed;
		base.EmergeFromBlock ();
	}

	// Increases the player's size.
	public override void HitPlayer (Player player) {
		if (player.GetComponent<MushroomPower> () == null) {
			player.AddPower (player.gameObject.AddComponent<MushroomPower> ());
		}
	}
}
