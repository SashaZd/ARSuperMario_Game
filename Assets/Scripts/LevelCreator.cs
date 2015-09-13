using UnityEngine;
using System.Collections;

// Creates a ribbon path from input.
public class LevelCreator : MonoBehaviour {

	// Path resource to be instantiated from.
	public PathComponent pathPrefab;
	// Goal resource to be instantiated from.
	public GameObject goalPrefab;
	// Virtual platform resource to be instantiated from.
	public GameObject virtualPlatformPrefab;
	// The material to draw path lines with.
	public Material lineMaterial;

	// The player.
	public Player playerPrefab;
	// Possible types of enemies.
	public BasicEnemy[] possibleEnemies;

	// Use this for initialization.
	void Start () {
		PathInput[] pathInput = {new PathInput (3.329739f, 0.055f, -0.259506f),
			new PathInput (1.329739f, 0.055f, -0.259504f),
			new PathInput (1.334739f, 0.0548638f, -0.75489f),
			new PathInput (0.939739f, 0.0548638f, -0.74989f),
			new PathInput (0.9347399f, 0.5741258f, 0.7979478f)};
		PlatformInput[] platformInput = {new PlatformInput (0.6260657f, 0.964f, 0.055f, 1.1260657f, 1.064f, 0.555f)};
		PathInput[] enemyPath = {new PathInput (1.329739f, 0.055f, -0.259504f), 
			new PathInput (1.334739f, 0.0548638f, -0.75489f)};
		EnemyInput[] enemyInput = {new EnemyInput (0, enemyPath)};
		CreateLevel (pathInput, platformInput, enemyInput);
	}

	// Creates a level from the given input.
	public void CreateLevel (PathInput[] pathInput, PlatformInput[] platformInput, EnemyInput[] enemyInput) {
		LevelManager levelManager = LevelManager.GetInstance ();
		PathComponent[] fullPath = new PathComponent[pathInput.Length - 1];

		// Construct the path from the input vectors.
		for (int i = 0; i < fullPath.Length; i++) {
			// Make and position the path component.
			PathComponent pathComponent = CreatePath (pathInput[i], pathInput[i + 1]);
			fullPath[i] = pathComponent;
			pathComponent.lineMaterial = lineMaterial;

			// Link paths together.
			if (i > 0) {
				pathComponent.previousPath = fullPath[i - 1];
				fullPath[i - 1].nextPath = pathComponent;
			}
		}

		// Set the player on a path.
		Player player = Instantiate (playerPrefab) as Player;
		player.GetComponent<PathMovement> ().currentPath = fullPath[0];
		player.GetComponent<PathMovement> ().startPath = fullPath[0];
		foreach (PathComponent pathComponent in fullPath) {
			pathComponent.transform.parent = player.transform;
		}
		levelManager.player = player;

		// Create the goal at the end of the path.
		GameObject goal = Instantiate (goalPrefab);
		goal.name = "Goal";
		goal.transform.parent = levelManager.transform;
		goal.transform.position = fullPath[fullPath.Length - 1].end + Vector3.up * 0.05f;
		
		// Create virtual platforms from the input.
		foreach (PlatformInput input in platformInput) {
			GameObject virtualPlatform = Instantiate (virtualPlatformPrefab);
			virtualPlatform.name = "VirtualPlatform";
			virtualPlatform.transform.parent = levelManager.transform;
			Vector3 startCorner = new Vector3 (input.startX, input.startY, input.startZ);
			Vector3 endCorner = new Vector3 (input.endX, input.endY, input.endZ);
			virtualPlatform.transform.position = (startCorner + endCorner) / 2;
			virtualPlatform.transform.localScale = endCorner - startCorner;
		}

		// Create enemies from the input.
		BasicEnemy[] enemies = new BasicEnemy[enemyInput.Length];
		levelManager.enemyPaths = new PathComponent[enemyInput.Length][];
		for (int i = 0; i < enemyInput.Length; i++) {
			if (enemyInput[i].enemyIndex < possibleEnemies.Length) {
				BasicEnemy enemy = Instantiate (possibleEnemies[enemyInput[i].enemyIndex]) as BasicEnemy;
				enemies[i] = enemy;
				enemy.transform.parent = levelManager.transform;
				int pathLength = enemyInput[i].path.Length - 1;
				PathComponent[] enemyPath = new PathComponent[pathLength];
				for (int j = 0; j < pathLength; j++) {
					PathComponent pathComponent = CreatePath (enemyInput[i].path[j], enemyInput[i].path[j + 1]);
					enemyPath[j] = pathComponent;
					if (j > 0) {
						pathComponent.previousPath = enemyPath[j - 1];
						enemyPath[j - 1].nextPath = pathComponent;
					}
					pathComponent.transform.parent = enemy.transform;
				}
				levelManager.enemyPaths[i] = enemyPath;
				enemy.GetComponent<PathMovement> ().currentPath = enemyPath[0];
				enemy.GetComponent<PathMovement> ().startPath = enemyPath[0];
			}
		}

		// Pass the needed data to the level manager to store.
		levelManager.fullPath = fullPath;
		levelManager.enemies = enemies;
		Destroy (gameObject);
	}

	// Creates a path component from a start and end point.
	PathComponent CreatePath (PathInput startInput, PathInput endInput) {
		Vector3 start = PathUtil.MakeVectorFromPathInput(startInput);
		Vector3 end = PathUtil.MakeVectorFromPathInput(endInput);
		Vector3 center = (start + end) / 2;
		PathComponent path = Instantiate (pathPrefab, center, Quaternion.LookRotation (end - start, Vector3.up)) as PathComponent;
		path.start = start;
		path.end = end;
		path.transform.Rotate (new Vector3 (0, -90, 0));
		Vector3 tempScale = path.transform.localScale;
		tempScale.x *= Vector3.Magnitude (end - start);
		path.transform.localScale = tempScale;
		return path;
	}
}
