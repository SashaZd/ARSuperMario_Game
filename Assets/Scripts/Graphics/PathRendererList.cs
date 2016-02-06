using UnityEngine;
using Sanford.Collections.Generic;
using System.Collections;

// Keeps track of which parts of the paths to outline.
public class PathRendererList {

	// The maximum number of lines to render.
	const int CAPACITY = 10;

	// All currently rendered path lines.
	Deque<PathRenderer> pathList = new Deque<PathRenderer>();

	public PathRendererList(PathComponent startPath) {
		Init (startPath);
	}

	// Fills the path list with the first lines in the path.
	public void Init (PathComponent startPath) {
		if (pathList.Count > 0) {
			foreach (PathRenderer pathRenderer in pathList) {
				pathRenderer.SetVisible (false);
			}
			pathList.Clear ();
		}
		if (startPath != null) {
			PathRenderer current = startPath.firstLine;
			while (pathList.Count < CAPACITY && current != null) {
				PushFront (current);
				current = current.nextLine;
			}
		}
	}

	// Puts a path renderer at the front of the list. Evicts the back of the list if at capacity.
	private void PushFront (PathRenderer pathRenderer) {
		if (pathList.Count >= CAPACITY) {
			pathList.PopBack ().SetVisible (false);
		}
		pathList.PushFront (pathRenderer);
		pathRenderer.SetVisible (true);
	}

	// Puts a path renderer at the back of the list. Evicts the front of the list if at capacity.
	private void PushBack (PathRenderer pathRenderer) {
		if (pathList.Count >= CAPACITY) {
			pathList.PopFront ().SetVisible (false);
		}
		pathList.PushBack (pathRenderer);
		pathRenderer.SetVisible (true);
	}

	// Checks for changing the displayed line based on player movement.
	public void CheckList (Player player, bool forward) {
		if (forward) {
			CheckForward (player);
		} else {
			CheckBackward (player);
		}
	}

	// Checks for changing the displayed line when the player moves forwards.
	public void CheckForward (Player player) {
		PathRenderer nextLine = pathList.PeekFront ().nextLine;
		if (nextLine != null && nextLine.GetBackDistance (player) < pathList.PeekBack ().GetFrontDistance (player)) {
			PushFront (nextLine);
		}
	}

	// Checks for changing the displayed line when the player moves backwards.
	public void CheckBackward (Player player) {
		PathRenderer prevLine = pathList.PeekBack ().prevLine;
		if (prevLine != null && prevLine.GetFrontDistance (player) < pathList.PeekFront ().GetBackDistance (player)) {
			PushBack (prevLine);
		}
	}
}
