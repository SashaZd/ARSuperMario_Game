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
		EnemyInput[] enemyInput = {new EnemyInput (0, 1, 0.5f)};
		CreateLevel (pathInput, platformInput, enemyInput);
	}

	// Creates a level from the given input.
	public void CreateLevel (PathInput[] pathInput, PlatformInput[] platformInput, EnemyInput[] enemyInput) {
		LevelManager levelManager = LevelManager.GetInstance ();
		PathComponent[] fullPath = new PathComponent[pathInput.Length - 1];

		// Construct the path from the input vectors.
		for (int i = 0; i < fullPath.Length; i++) {
			// Make and position the path component.
			Vector3 start = PathUtil.MakeVectorFromPathInput(pathInput[i]);
			Vector3 end = PathUtil.MakeVectorFromPathInput(pathInput[i + 1]);
			Vector3 center = (start + end) / 2;
			PathComponent path = Instantiate (pathPrefab, center, Quaternion.LookRotation (end - start, Vector3.up)) as PathComponent;
			fullPath[i] = path;
			path.transform.parent = levelManager.transform;
			path.start = start;
			path.end = end;
			path.transform.Rotate (new Vector3 (0, -90, 0));
			Vector3 tempScale = path.transform.localScale;
			tempScale.x *= Vector3.Magnitude (end - start);
			path.transform.localScale = tempScale;
			path.lineMaterial = lineMaterial;

			// Link paths together.
			if (i > 0) {
				path.previousPath = fullPath[i - 1];
				fullPath[i - 1].nextPath = path;
			}
		}

		// Create the player.
		Player player = Instantiate (playerPrefab) as Player;
		player.GetComponent<PathMovement> ().currentPath = fullPath[0];
		player.GetComponent<PathMovement> ().startPath = fullPath[0];
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
		for (int i = 0; i < enemyInput.Length; i++) {
			if (enemyInput[i].enemyIndex < possibleEnemies.Length) {
				BasicEnemy enemy = Instantiate (possibleEnemies[enemyInput[i].enemyIndex]) as BasicEnemy;
				enemies[i] = enemy;
				enemy.transform.parent = levelManager.transform;
				enemy.GetComponent<PathMovement> ().currentPath = fullPath[enemyInput[i].pathNumber];
				enemy.GetComponent<PathMovement> ().startPath = fullPath[enemyInput[i].pathNumber];
				enemy.GetComponent<PathMovement> ().pathProgress = enemyInput[i].pathProgress;
			}
		}

		// Pass the needed data to the level manager to store.
		levelManager.fullPath = fullPath;
		levelManager.enemies = enemies;
		Destroy (gameObject);
	}
}
