using System.Collections.Generic;

// Used to add a path for a level.
public struct PathInput {
	// The position of a path vertex.
	public float x, y, z;

	public PathInput (float x, float y, float z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public PathInput (JSONObject json) {
		this.x = json.list [0].n;
		this.y = json.list [1].n;
		this.z = json.list [2].n;
	}
}

// Used to add virtual platforms to a level.
public struct PlatformInput {
	// The position of a corner of the platform.
	public float startX, startY, startZ;
	// The position of the opposite platform corner from the start corner.
	public float endX, endY, endZ;

	public PlatformInput (float startX, float startY, float startZ, float endX, float endY, float endZ) {
		this.startX = startX;
		this.startY = startY;
		this.startZ = startZ;
		this.endX = endX;
		this.endY = endY;
		this.endZ = endZ;
	}

	public PlatformInput (JSONObject json) {
		JSONObject corners = json.GetField ("platform").list [0];
		this.startX = corners.list [0].n;
		this.startY = corners.list [1].n;
		this.startZ = corners.list [2].n;
		this.endX = corners.list [3].n;
		this.endY = corners.list [4].n;
		this.endZ = corners.list [5].n;
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
}

// Used to add coins into a level.
public struct CoinInput {
	// The position of the coin.
	public float x, y, z;

	public CoinInput (float x, float y, float z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}

	public CoinInput (JSONObject json) {
		this.x = json.list [0].n;
		this.y = json.list [1].n;
		this.z = json.list [2].n;
	}
}

// Used to add blocks into a level.
public struct BlockInput {
	// The index of the type of block.
	public int blockIndex;
	// The index of the type of item inside the block.
	public int contentIndex;
	// The position of the block.
	public float x, y, z;

	public BlockInput (int blockIndex, int contentIndex, float x, float y, float z) {
		this.blockIndex = blockIndex;
		this.contentIndex = contentIndex;
		this.x = x;
		this.y = y;
		this.z = z;
	}
}