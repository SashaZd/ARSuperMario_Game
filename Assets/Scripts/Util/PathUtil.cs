using UnityEngine;
using System;

// Utility methods related to ribbon paths.
public class PathUtil {

	// The height of the ceiling in the level. Used to draw the ribbon path.
	public static float ceilingHeight = 10;
	// The height of the floor. Kills anything that falls on it.
	public static float floorHeight = -2;

	// Sets the x and z components of a transform's position while leaving the y component unmodified.
	public static void SetXZ (Transform target, Vector3 newPosition) {
		Vector3 setPosition = new Vector3 (newPosition.x, target.position.y, newPosition.z);
		target.position = setPosition;
	}

	// Sets the x and z components of a transform's position while leaving the y component unmodified.
	public static void SetXZ (Transform target, Vector2 newPosition) {
		Vector3 setPosition = new Vector3 (newPosition.x, target.position.y, newPosition.y);
		target.position = setPosition;
	}

	// Sets the y component of a transform's position.
	public static void SetY (Transform target, float newY) {
		Vector3 setPosition = new Vector3 (target.position.x, newY, target.position.z);
		target.position = setPosition;
	}

	// Creates a new vector with a changed x component.
	public static Vector3 SetX (Vector3 vector, float newX) {
		return new Vector3 (newX, vector.y, vector.z);
	}
	
	// Creates a new vector with a changed y component.
	public static Vector3 SetY (Vector3 vector, float newY) {
		return new Vector3 (vector.x, newY, vector.z);
	}

	// Creates a new vector with a changed z component.
	public static Vector3 SetZ (Vector3 vector, float newZ) {
		return new Vector3 (vector.x, vector.y, newZ);
	}
	
	// Scales the y component of the target's position.
	public static void ScaleY (Transform target, float scale) {
		Vector3 setScale = new Vector3 (target.localScale.x, target.localScale.y * scale, target.localScale.z);
		target.localScale = setScale;
	}

	// Creates a new vector with the y component set to 0.
	public static Vector3 RemoveY (Vector3 vector) {
		return SetY (vector, 0);
	}

	// Checks if an entity is touching the "floor is lava".
	public static bool OnFloor (GameObject entity) {
		return entity.transform.position.y - entity.GetComponent<Collider> ().bounds.extents.y * 1.05f < floorHeight;
	}

	// Finds the closest distance between a point and a line.
	public static float DistancePointLine (Vector3 point, Vector3 lineStart, Vector3 lineEnd) {
		// Compute the area of the triangle formed by the point and the line.
		// deconstruct it into the height (distance) with the triangle area formula b*h/2.
		return Vector3.Magnitude (Vector3.Cross (point - lineStart, point - lineEnd)) / Vector3.Magnitude (lineEnd - lineStart); 
	}

	// Finds the orthogonal projection of a point onto a line.
	public static Vector3 ProjectionPointLine (Vector3 point, Vector3 lineStart, Vector3 lineEnd) {
		// Make a vector out of the point and the start of the line and project it to the line.
		Vector3 lineVector = lineEnd - lineStart;
		Vector3 pointVector = point - lineStart;
		return Vector3.Dot (lineVector, pointVector) / Mathf.Pow (Vector3.Magnitude (lineVector), 2) * lineVector;
	}

	// Finds the closest path to the object and assigns its path position accordingly.
	public static void FindClosestPath (Vector3 position, PathMovement pathMovement) {
		PathComponent closestPath = null;
		float minDistance = (float)Int32.MaxValue;
		foreach (PathComponent pathComponent in LevelManager.GetInstance ().fullPath) {
			float pathDistance = DistancePointLine (RemoveY (position), RemoveY (pathComponent.GetStart ()), RemoveY (pathComponent.GetEnd ()));
			if (pathDistance < minDistance) {
				minDistance = pathDistance;
				closestPath = pathComponent;
				if (minDistance < 0.05f) {
					break;
				}
			}
		}
		pathMovement.currentPath = closestPath;
		pathMovement.startPath = closestPath;
		pathMovement.pathProgress = ProjectionPointLine (position, closestPath.GetStart (), closestPath.GetEnd ()).x / closestPath.GetLength ().x;
	}

	// Moves the object towards a target position, ignoring y.
	public static void MoveTowardsXZ (Transform current, Vector3 target, float maxDeltaDistance) {
		Vector2 currentPositionXZ = Vector2From3 (current.position);
		Vector2 targetPositionXZ = Vector2From3 (target);
		Vector2 travelDistance = Vector2.MoveTowards (currentPositionXZ, targetPositionXZ, maxDeltaDistance);
		SetXZ (current, travelDistance);
	}

	// Creates a 2D vector from a 3D vector's x and z components.
	public static Vector2 Vector2From3 (Vector3 vector) {
		return new Vector2 (vector.x, vector.z);
	}

	// Gets the magnitude of the x and z components of a vector.
	public static float GetMagnitudeXZ (Vector3 vector) {
		return Vector2.SqrMagnitude (Vector2From3 (vector));
	}

	// Gets the distance between two vectors, ignoring height.
	public static float DistanceXZ (Vector3 vector1, Vector3 vector2) {
		return Vector3.Distance (RemoveY (vector1), RemoveY (vector2));
	}

	// Makes a vector from a size 3 JSON array.
	public static Vector3 MakeVectorFromJSON (JSONObject json) {
		Vector3 returnVector = new Vector3 (json.list [0].n, json.list [1].n, json.list [2].n);
		return returnVector * LevelManager.GetInstance ().scaleMultiplier;
	}
}
