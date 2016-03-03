using UnityEngine;
using System;

/// <summary>
/// Utility methods related to ribbon paths.
/// </summary>
public class PathUtil {

	/// <summary> The height of the ceiling in the level. Used to draw the ribbon path. </summary>
	public static float ceilingHeight = 10;
	/// <summary> The height of the floor. Kills anything that falls on it. </summary>
	public static float floorHeight = -2;

	/// <summary>
	/// Sets the x and z components of a transform's position while leaving the y component unmodified.
	/// </summary>
	/// <param name="target">The transform to set a position for.</param>
	/// <param name="newPosition">The position to set the x and z components of the transform to.</param>
	public static void SetXZ(Transform target, Vector3 newPosition) {
		Vector3 setPosition = new Vector3(newPosition.x, target.position.y, newPosition.z);
		target.position = setPosition;
	}

	/// <summary>
	/// Sets the x and z components of a transform's position while leaving the y component unmodified.
	/// </summary>
	/// <param name="target">The transform to set a position for.</param>
	/// <param name="newPosition">The position to set the x and z components of the transform's position to.</param>
	public static void SetXZ(Transform target, Vector2 newPosition) {
		Vector3 setPosition = new Vector3(newPosition.x, target.position.y, newPosition.y);
		target.position = setPosition;
	}

	/// <summary>
	/// Sets the y component of a transform's position.
	/// </summary>
	/// <param name="target">The transform to set a position for.</param>
	/// <param name="newY">The y coordinate to set the transform's position to.</param>
	public static void SetY(Transform target, float newY) {
		Vector3 setPosition = new Vector3(target.position.x, newY, target.position.z);
		target.position = setPosition;
	}

	/// <summary>
	/// Creates a new vector with a changed x component.
	/// </summary>
	/// <returns>A new vector with the specified x component.</returns>
	/// <param name="vector">The vector to create a new vector from.</param>
	/// <param name="newX">The x component of the new vector.</param>
	public static Vector3 SetX(Vector3 vector, float newX) {
		return new Vector3(newX, vector.y, vector.z);
	}

	/// <summary>
	/// Creates a new vector with a changed y component.
	/// </summary>
	/// <returns>A new vector with the specified y component.</returns>
	/// <param name="vector">The vector to create a new vector from.</param>
	/// <param name="newX">The y component of the new vector.</param>
	public static Vector3 SetY(Vector3 vector, float newY) {
		return new Vector3(vector.x, newY, vector.z);
	}

	/// <summary>
	/// Creates a new vector with a changed z component.
	/// </summary>
	/// <returns>A new vector with the specified z component.</returns>
	/// <param name="vector">The vector to create a new vector from.</param>
	/// <param name="newX">The z component of the new vector.</param>
	public static Vector3 SetZ(Vector3 vector, float newZ) {
		return new Vector3(vector.x, vector.y, newZ);
	}

	/// <summary>
	/// Scales the y component of the target's position.
	/// </summary>
	/// <param name="target">The transform to scale the position of.</param>
	/// <param name="scale">The amount to scale the y component of the transform's scale by.</param>
	public static void ScaleY(Transform target, float scale) {
		Vector3 setScale = new Vector3(target.localScale.x, target.localScale.y * scale, target.localScale.z);
		target.localScale = setScale;
	}

	/// <summary>
	/// Creates a new vector with the y component set to 0.
	/// </summary>
	/// <returns>A new vector with the y component set to 0.</returns>
	/// <param name="vector">The vector to create a new vector from.</param>
	public static Vector3 RemoveY(Vector3 vector) {
		return SetY (vector, 0);
	}

	/// <summary>
	/// Checks if an entity is touching the "floor is lava".
	/// </summary>
	/// <returns>Whether the entity is touching the "floor is lava".</returns>
	/// <param name="entity">The entity to check.</param>
	public static bool OnFloor(GameObject entity) {
		return entity.transform.position.y - entity.GetComponent<Collider>().bounds.extents.y * 1.05f < floorHeight;
	}

	/// <summary>
	/// Finds the closest distance between a point and a line.
	/// </summary>
	/// <returns>The closest distance between the point and the line.</returns>
	/// <param name="point">The point to use in finding the distance.</param>
	/// <param name="lineStart">The start of the line used in finding the distance.</param>
	/// <param name="lineEnd">The end of the line used in finding the distance.</param>
	public static float DistancePointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd) {
		// Compute the area of the triangle formed by the point and the line.
		// deconstruct it into the height (distance) with the triangle area formula b*h/2.
		return Vector3.Magnitude(Vector3.Cross(point - lineStart, point - lineEnd)) / Vector3.Magnitude(lineEnd - lineStart); 
	}

	/// <summary>
	/// Finds the orthogonal projection of a point onto a line.
	/// </summary>
	/// <returns>The orthogonal projection of a point onto a line.</returns>
	/// <param name="point">The point to project.</param>
	/// <param name="lineStart">The start of the line to project the point onto.</param>
	/// <param name="lineEnd">The end of the line to project the point onto.</param>
	public static Vector3 ProjectionPointLine(Vector3 point, Vector3 lineStart, Vector3 lineEnd) {
		// Make a vector out of the point and the start of the line and project it to the line.
		Vector3 lineVector = lineEnd - lineStart;
		Vector3 pointVector = point - lineStart;
		return Vector3.Dot(lineVector, pointVector) / Mathf.Pow(Vector3.Magnitude (lineVector), 2) * lineVector;
	}

	/// <summary>
	/// Finds the closest path to the object and assigns its path position accordingly.
	/// </summary>
	/// <param name="position">The position of the object.</param>
	/// <param name="pathMovement">The path movement component of the object.</param>
	public static void FindClosestPath(Vector3 position, PathMovement pathMovement) {
		PathComponent closestPath = null;
		float minDistance = (float)Int32.MaxValue;
		foreach (PathComponent pathComponent in LevelManager.Instance.fullPath) {
			float pathDistance = DistancePointLine(RemoveY(position), RemoveY(pathComponent.Start), RemoveY(pathComponent.End));
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
		pathMovement.pathProgress = ProjectionPointLine(position, closestPath.Start, closestPath.End).x / closestPath.Length.x;
	}

	/// <summary>
	/// Moves the object towards a target position, ignoring y.
	/// </summary>
	/// <param name="current">The transform to move.</param>
	/// <param name="target">The target position to move towards.</param>
	/// <param name="maxDeltaDistance">The maximum distance that the transform can move to.</param>
	public static void MoveTowardsXZ(Transform current, Vector3 target, float maxDeltaDistance) {
		Vector2 currentPositionXZ = Vector2From3(current.position);
		Vector2 targetPositionXZ = Vector2From3(target);
		Vector2 travelDistance = Vector2.MoveTowards(currentPositionXZ, targetPositionXZ, maxDeltaDistance);
		SetXZ(current, travelDistance);
	}

	/// <summary>
	/// Creates a 2D vector from a 3D vector's x and z components.
	/// </summary>
	/// <returns>A 2D vector from the 3D vector's x and z components.</returns>
	/// <param name="vector">The 3D vector to create a 2D vector with.</param>
	public static Vector2 Vector2From3(Vector3 vector) {
		return new Vector2(vector.x, vector.z);
	}

	/// <summary>
	/// Gets the magnitude of the x and z components of a vector.
	/// </summary>
	/// <returns>The magnitude of the x and z components of a vector.</returns>
	/// <param name="vector">The vector to get a magnitude for.</param>
	public static float GetMagnitudeXZ(Vector3 vector) {
		return Vector2.SqrMagnitude(Vector2From3(vector));
	}

	/// <summary>
	/// Gets the distance between two vectors, ignoring height.
	/// </summary>
	/// <returns>The distance between the two vectors, ignoring height.</returns>
	/// <param name="vector1">One vector to get the distance between.</param>
	/// <param name="vector2">The other vector to get the distance between.</param>
	public static float DistanceXZ(Vector3 vector1, Vector3 vector2) {
		return Vector3.Distance(RemoveY(vector1), RemoveY(vector2));
	}

	/// <summary>
	/// Makes a vector from a size 3 JSON array.
	/// </summary>
	/// <returns>A vector from the size 3 JSON array. </returns>
	/// <param name="json">The size 3 JSON array to create a vector from.</param>
	public static Vector3 MakeVectorFromJSON(JSONObject json) {
		Vector3 returnVector = new Vector3(json.list[0].n, json.list[1].n, json.list[2].n);
		return returnVector * LevelManager.Instance.scaleMultiplier;
	}
}
