using UnityEngine;
using System.Collections.Generic;

// Used to add a path for a level.
public struct PathInput {
	// The position of a path vertex.
	public Vector3 position;

	public PathInput (float x, float y, float z) {
		position = new Vector3 (x, y, z);
	}

	public PathInput (JSONObject json) {
		position = PathUtil.MakeVectorFromJSON (json);
	}
}

// Used to add virtual platforms to a level.
public struct PlatformInput {
	// The position of a corner of the platform.
	public List<Vector3> vertices;

	public PlatformInput (JSONObject json) {
		vertices = new List<Vector3> ();
		foreach (JSONObject vertex in json.GetField ("platform").list) {
			vertices.Add (PathUtil.MakeVectorFromJSON (vertex));
		}
	}
}

// Used to add enemies into a level.
public struct EnemyInput {
	// The index of the type of enemy.
	public int enemyIndex;
	// The vertices of the path the enemy will follow.
	public List<PathInput> path;

	public EnemyInput (int enemyIndex, List<PathInput> path) {
		this.enemyIndex = enemyIndex;
		this.path = path;
	}

	public EnemyInput (JSONObject json) {
		JSONObject enemyPathJSON = json.GetField ("enemy_path");
		path = new List<PathInput> (enemyPathJSON.list.Count);
		foreach (JSONObject pathComponent in enemyPathJSON.list) {
			path.Add (new PathInput (pathComponent));
		}
		enemyIndex = (int) json.GetField ("enemy_type").n;
	}
}

// Used to add coins into a level.
public struct CoinInput {
	// The position of the coin.
	public Vector3 position;

	public CoinInput (float x, float y, float z) {
		position = new Vector3 (x, y, z);
	}

	public CoinInput (JSONObject json) {
		position = PathUtil.MakeVectorFromJSON (json);
	}
}

// Used to add blocks into a level.
public struct BlockInput {
	// The index of the type of block.
	public int blockIndex;
	// The index of the type of item inside the block.
	public int contentIndex;
	// The position of the block.
	public Vector3 position;

	public BlockInput (int blockIndex, int contentIndex, float x, float y, float z) {
		this.blockIndex = blockIndex;
		this.contentIndex = contentIndex;
		position = new Vector3 (x, y, z);
	}
}