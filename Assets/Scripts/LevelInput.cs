// Used to add a path for a level.
public struct PathInput {
	public float x;
	public float y;
	public float z;

	public PathInput (float x, float y, float z) {
		this.x = x;
		this.y = y;
		this.z = z;
	}
}

// Used to add virtual platforms to a level.
public struct PlatformInput {
	public float startX;
	public float startY;
	public float startZ;
	public float endX;
	public float endY;
	public float endZ;

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
	public int enemyIndex;
	public int pathNumber;
	public float pathProgress;

	public EnemyInput (int enemyIndex, int pathNumber, float pathProgress) {
		this.enemyIndex = enemyIndex;
		this.pathNumber = pathNumber;
		this.pathProgress = pathProgress;
	}
}