using UnityEngine;
using System;

// Utility methods related to ribbon paths.
public class PathUtil {

	// The height of the ceiling in the level. Used to draw the ribbon path.
	public static float ceilingHeight = 10;
	// The height of the floor. Kills anything that falls on it.
	public static float floorHeight = 0.1f;

	// Sets the x and z components of a transform's position while leaving the y component unmodified.
	public static void SetXZ (Transform target, Vector3 newPosition) {
		Vector3 setPosition = new Vector3 (newPosition.x, target.position.y, newPosition.z);
		target.position = setPosition;
	}

	// Sets the y component of a transform's position.
	public static void SetY (Transform target, float newY) {
		Vector3 setPosition = new Vector3 (target.position.x, newY, target.position.z);
		target.position = setPosition;
	}
	
	// Scales the y component of the target's position.
	public static void ScaleY (Transform target, float scale) {
		Vector3 setScale = new Vector3 (target.localScale.x, target.localScale.y * scale, target.localScale.z);
		target.localScale = setScale;
	}

	// Creates a new vector with a changed y component.
	public static Vector3 SetY (Vector3 vector, float newY) {
		return new Vector3 (vector.x, newY, vector.z);
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
			float pathDistance = PathUtil.DistancePointLine (position, pathComponent.GetStart (), pathComponent.GetEnd ());
			if (pathDistance < minDistance) {
				minDistance = pathDistance;
				closestPath = pathComponent;
				if (minDistance < 0.05f) {
					break;
				}
			}
		}
		pathMovement.currentPath = closestPath;
		pathMovement.pathProgress = PathUtil.ProjectionPointLine (position, closestPath.GetStart (), closestPath.GetEnd ()).x / closestPath.GetLength ().x;
	}

	// Makes a vector from a size 3 JSON array.
	public static Vector3 MakeVectorFromJSON (JSONObject json) {
		return new Vector3 (json.list [0].n, json.list [1].n, json.list [2].n);
	}
}
