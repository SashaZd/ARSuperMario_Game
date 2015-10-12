using UnityEngine;
using System.Collections;

// Rotates the object every frame.
public class Rotate : MonoBehaviour {

	// The speed that the object is rotated at.
	public float rotateSpeed = 1;

	// Update is called once per frame.
	void Update () {
		transform.Rotate (new Vector3 (0f, rotateSpeed, 0f));
	}
}
