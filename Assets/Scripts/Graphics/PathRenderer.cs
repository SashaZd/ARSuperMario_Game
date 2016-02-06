using UnityEngine;
using System.Collections;

// Displays a line tracing a part of the path.
public class PathRenderer {
	
	// The thickness of the rendered line.
	const float LINETHICKNESS = 0.05f;

	// The path component that this line is displaying.
	public PathComponent pathComponent;
	// The line being rendered.
	LineRenderer line;

	// The line immediately before this line in the path.
	public PathRenderer prevLine;
	// The line immediately after this line in the path.
	public PathRenderer nextLine;

	// The position of the start of the line.
	public Vector3 lineStart;
	// The position of the end of the line.
	public Vector3 lineEnd;

	public PathRenderer(PathComponent pathComponent, Vector3 lineStart, Vector3 lineEnd, Material lineMaterial) {
		this.pathComponent = pathComponent;
		this.lineStart = lineStart;
		this.lineEnd = lineEnd;

		line = new GameObject().AddComponent<LineRenderer> () as LineRenderer;
		line.gameObject.name = "PathRenderer";
		line.material = lineMaterial;
		line.transform.parent = pathComponent.transform;
		line.SetWidth(LINETHICKNESS, LINETHICKNESS);
		line.SetVertexCount(2);

		line.SetPosition(0, lineStart);
		line.SetPosition(1, lineEnd);
		SetVisible (false);
	}

	// Sets the line as visible or invisible.
	public void SetVisible (bool visible) {
		line.enabled = visible;
	}

	// Gets the distance from the player to the front of the line, following the path.
	public float GetFrontDistance (Player player) {
		PathComponent currentPath = pathComponent;
		PathComponent targetPath = player.GetCurrentPath ();
		float currentDistance = 0;
		if (currentPath != targetPath) {
			currentDistance += PathUtil.DistanceXZ (lineEnd, currentPath.end);
			currentPath = currentPath.nextPath;
			while (currentPath != null && currentPath != targetPath) {
				currentDistance += currentPath.GetMagnitude ();
				currentPath = currentPath.nextPath;
			}
			currentDistance += PathUtil.DistanceXZ (currentPath.start, player.transform.position);
		} else {
			currentDistance = PathUtil.DistanceXZ (lineEnd, player.transform.position);
		}
		return currentDistance;
	}

	// Gets the distance from the player to the back of the line, following the path.
	public float GetBackDistance (Player player) {
		PathComponent currentPath = pathComponent;
		PathComponent targetPath = player.GetCurrentPath ();
		float currentDistance = 0;
		if (currentPath != targetPath) {
			currentDistance += PathUtil.DistanceXZ (lineStart, currentPath.start);
			currentPath = currentPath.previousPath;
			while (currentPath != null && currentPath != targetPath) {
				currentDistance += currentPath.GetMagnitude ();
				currentPath = currentPath.previousPath;
			}
			currentDistance += PathUtil.DistanceXZ (currentPath.end, player.transform.position);
		} else {
			currentDistance = PathUtil.DistanceXZ (lineStart, player.transform.position);
		}
		return currentDistance;
	}
}
