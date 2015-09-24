// Used to add a path for a level.
public struct PathInput {
	// The position of a path vertex.
	public float x, y, z;

	public PathInput (float x, float y, float z) {
		this.x = x;
		this.y = y;
		this.z = z;
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
}

// Used to add enemies into a level.
public struct EnemyInput {
	// The index of the type of enemy.
	public int enemyIndex;
	// The vertices of the path the enemy will follow.
	public PathInput[] path;

	public EnemyInput (int enemyIndex, PathInput[] path) {
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