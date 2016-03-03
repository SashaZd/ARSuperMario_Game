using UnityEngine;

/// <summary>
/// A block that can be hit from underneath to reveal an item.
/// </summary>
public class QuestionBlock : Block {

	/// <summary> Whether the block has been hit by the player. </summary>
	private bool wasHit = false;
	/// <summary> The material the question block appears as before being hit. </summary>
	private Material startMaterial;
	/// <summary> The material the question block will take on after being hit. </summary>
	[SerializeField]
	[Tooltip("The material the question block will take on after being hit.")]
	private Material hitMaterial;

	/// <summary> The amount to increase the object's height by every tick. </summary>
	private float contentIncrement;
	/// <summary> Timer used to animate contents coming out of the block. </summary>
	private float contentHeight = 0;
	/// <summary> The object that is emerging from the block. </summary>
	private Item contentObject;

	/// <summary>
	/// Initializes the question block material.
	/// </summary>
	private void Start() {
		startMaterial = GetComponent<Renderer>().material;
	}

	/// <summary>
	/// Animates the contents of the block emerging.
	/// </summary>
	private void Update() {
		if (GameMenuUI.paused) {
			return;
		}
		if (contentIncrement > 0) {
			PathUtil.SetY (contentObject.transform, contentObject.transform.position.y + contentIncrement);
			contentHeight += contentIncrement;
			if (contentHeight > contentIncrement * 60) {
				contentIncrement = 0;
				Rigidbody body = contentObject.GetComponent<Rigidbody>();
				if (body) {
					body.useGravity = true;
				}
				contentObject.GetComponent<Collider>().enabled = true;
				contentObject.GetComponent<Item>().EmergeFromBlock();
				contentObject = null;
			}
		}
	}

	/// <summary>
	/// Spawns the contents of the block above the block.
	/// </summary>
	public override void HitBlock() {
		if (!wasHit) {
			Tracker.Instance.logAction("Block hit");
			contentObject = Instantiate(contents);
			contentObject.transform.position = transform.position;
			contentIncrement = GetComponent<Collider>().bounds.extents.y / 30;
			Rigidbody body = contentObject.GetComponent<Rigidbody>();
			if (body) {
			    body.useGravity = false;
			    contentObject.GetComponent<PathMovement>().moveSpeed = 0;
			}
			contentObject.GetComponent<Collider>().enabled = false;
			wasHit = true;
			GetComponent<Renderer>().material = hitMaterial;
		}
	}

	/// <summary>
	/// Regenerates the contents of the block.
	/// </summary>
	public override void Reset() {
		wasHit = false;
		GetComponent<Renderer>().material = startMaterial;
		contentIncrement = 0;
		contentHeight = 0;
		contentObject = null;
	}
}
