using UnityEngine;
using System.Collections;

// Utility methods related to ribbon paths.
public class PathUtil {

	// The height of the ceiling in the level. Used to draw the ribbon path.
	public static float ceilingHeight = 10;
	// The height of the floor. Kills anything that falls on it.
	public static float floorHeight = 0.051f;

	// Sets the x and z position of a transform while leaving the y component unmodified.
	public static void SetXZ (Transform target, Vector3 newPosition) {
		Vector3 setPosition = new Vector3 (newPosition.x, target.position.y, newPosition.z);
		target.position = setPosition;
	}

	// Returns a new vector with the y component of the previous vector set to 0.
	public static Vector3 RemoveY (Vector3 vector) {
		vector.y = 0;
		return vector;
	}

	// Creates and returns a 3D vector from a PathInput struct.
	public static Vector3 MakeVectorFromPathInput (PathInput input) {
		return new Vector3 (input.x, input.y, input.z);
	}

	public static bool OnFloor (GameObject entity) {
		return entity.transform.position.y - entity.GetComponent<Collider> ().bounds.extents.y < floorHeight;
	}
}
