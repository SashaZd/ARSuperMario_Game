using UnityEngine;
using System;

// A part of the path of the level.
public class PathComponent : MonoBehaviour {

	// The position of the start of the path.
	Vector3 start;
	// The position of the end of the path.
	Vector3 end;

	// The vector length of the path.
	Vector3 length;
	// The normalized direction of the path when moving forward.
	Vector3 direction;
	// The scalar magnitude of the path's length.
	float magnitude;

	// The path directly after this path.
	public PathComponent nextPath;
	// The path directly before this path.
	public PathComponent previousPath;

	// The length of individual segments of the rendered line.
	const float LINELENGTH = 0.05f;
	// The thickness of the rendered line.
	const float LINETHICKNESS = 0.05f;
	// The material to draw the line with.
	public Material lineMaterial;

	// Use this for initialization.
	void Start () {
		if (magnitude == 0) {
			if (start == end) {
				// If start and end points aren't already set, find them from the center point and rotation.
				magnitude = (float)Math.Pow((double)(transform.localScale.x * transform.localScale.x + transform.localScale.z * transform.localScale.z), 0.5);

				float radius = magnitude / 2;
				Vector3 rotation = transform.eulerAngles * (float)Math.PI / 180;
				Vector3 offset = new Vector3 (radius * (float)Math.Cos (rotation.y),
				                              0,
				                              radius * -(float)Math.Sin (rotation.y));
				start = new Vector3 (transform.position.x - offset.x, 
				                 transform.position.y - offset.y,
				                 transform.position.z - offset.z);
				end = new Vector3 (transform.position.x + offset.x, 
				               transform.position.y + offset.y,
							   transform.position.z + offset.z);

				length = PathUtil.RemoveY (end - start);
				direction = Vector3.Normalize (length);
			} else {
				CalculateDimensions ();
			}
		}
		if (lineMaterial != null) {
			DrawLine ();
		}
	}

	// Gets the start point of the path.
	public Vector3 GetStart () {
		return start;
	}

	// Sets the start point of the path.
	public void SetStart (Vector3 newStart) {
		start = newStart;
		CalculateDimensions ();
	}

	// Gets the end point of the path.
	public Vector3 GetEnd () {
		return end;
	}

	// Sets the end point of the path.
	public void SetEnd (Vector3 newEnd) {
		end = newEnd;
		CalculateDimensions ();
	}

	// Sets both endpoints of the path.
	public void SetPath (Vector3 newStart, Vector3 newEnd) {
		start = newStart;
		end = newEnd;
		CalculateDimensions ();
	}

	// Gets the vector length of the path.
	public Vector3 GetLength () {
		return length;
	}

	// Gets the magnitude of the path's length.
	public float GetMagnitude() {
		return magnitude;
	}
	
	// Gets the magnitude of a fraction of the path.
	public float GetMagnitudeFromProgress (float pathProgress) {
		return magnitude * pathProgress;
	}
	
	// Gets how far along the path something is based on its current distance along the path.
	public float GetProgressFromMagnitude (float currentDistance) {
		return currentDistance / magnitude;
	}

	// Moves the path progress forward or backward depending on movement speed.
	public float IncrementPathProgress (float pathProgress, float moveSpeed, bool forward) {
		return forward ? pathProgress + moveSpeed / magnitude : pathProgress - moveSpeed / magnitude;
	}

	// Gets a position at a specified part (fraction) of the path.
	public Vector3 GetPositionInPath (float pathProgress) {
		if (magnitude == 0) {
			Start ();
		}
		return start + length * pathProgress;
	}

	// Gets a position at a specified length in the path.
	public Vector3 GetPositionInPathMagnitude (float magnitudeProgress) {
		return start + length * magnitudeProgress / magnitude;
	}

	// Gets a normalized vector representing the direction of the path, forward or backward.
	public Vector3 GetDirection (bool forward) {
		return forward ? direction : -direction;
	}

	// Draws a dotted line along the ribbon path.
	public void DrawLine () {
		int numLines = (int)(magnitude / LINELENGTH / 2) + 1;
		for (int i = 0; i < numLines; i++) {

			// Get the start and end points of the segment.
			Vector3 lineStart = GetPositionInPathMagnitude (i * 2 * LINELENGTH);
			Vector3 lineEnd;
			// Check if a shorter segment needs to be drawn at the end of the line.
			if (i == numLines - 1) {
				if (i * 2 * LINELENGTH > magnitude) {
					break;
				} else {
					lineEnd = GetPositionInPathMagnitude (magnitude);
				}
			} else {
				lineEnd = GetPositionInPathMagnitude ((i * 2 + 1) * LINELENGTH);
			}

			// Find the height to draw the segment at.
			RaycastHit hit;
			Vector3 lineCenter = (lineStart + lineEnd) / 2;
			lineCenter.y = PathUtil.ceilingHeight;
			Physics.Raycast (lineCenter, Vector3.down, out hit, PathUtil.ceilingHeight * 1.1f);
			lineStart.y = hit.point.y;
			lineEnd.y = hit.point.y;

			// Draw the segment.
			LineRenderer line = new GameObject().AddComponent<LineRenderer> () as LineRenderer;
			line.gameObject.name = "LineRenderer";
			line.material = lineMaterial;
			line.transform.parent = transform;
			line.SetWidth(LINETHICKNESS, LINETHICKNESS);
			line.SetVertexCount(2);

			line.SetPosition(0, lineStart);
			line.SetPosition(1, lineEnd);
		}
	}

	// Calculates certain variables in the path.
	void CalculateDimensions () {
		length = PathUtil.RemoveY (end - start);
		direction = Vector3.Normalize (length);
		magnitude = Vector3.Magnitude (length);
	}

}
