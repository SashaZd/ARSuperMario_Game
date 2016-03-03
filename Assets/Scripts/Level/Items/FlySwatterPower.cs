using UnityEngine;

/// <summary>
/// Allows the player to use a fly swatter as a weapon.
/// </summary>
public class FlySwatterPower : MonoBehaviour, Power {

	/// <summary> The swatter prefab to use to spawn a swatter. </summary>
	[HideInInspector]
	public FlySwatter swatterPrefab;
	/// <summary> Instance of the swatter object. </summary>
	private FlySwatter swatterInstance;

	/// <summary> Cooldown timer for swinging the swatter. </summary>
	private int cooldown = 0;
	
	/// <summary> The amount of frames that the swing cooldown will last. </summary>
	private const int MAXCOOLDOWN = 30;

	/// <summary>
	/// Initializes the fly swatter object.
	/// </summary>
	private void Start() {
		GetComponent<Player>().LosePower(GetComponent<ToothpickPower>());
		swatterInstance = GameObject.Instantiate(swatterPrefab) as FlySwatter;
		swatterInstance.gameObject.SetActive(false);
		swatterInstance.transform.parent = transform;
	}

	/// <summary>
	/// Decrements the power-up cooldown.
	/// </summary>
	private void Update() {
		if (cooldown > 0) {
			cooldown--;
		}
	}

	/// <summary>
	/// Deploys the fly swatter if possible.
	/// </summary>
	public void PowerKey() {
		if (!swatterInstance.gameObject.activeSelf && cooldown <= 0) {
			cooldown = MAXCOOLDOWN;
			swatterInstance.Initiate();
		}
	}

	/// <summary>
	/// Sets the swatter object to be destroyed.
	/// </summary>
	public void OnRemove() {
		swatterInstance.willDestroy = true;
	}

	/// <summary>
	/// Destroys the swatter object.
	/// </summary>
	public void OnReset() {
		Destroy(swatterInstance.gameObject);
	}

	/// <summary>
	/// Returns the name of the power-up.
	/// </summary>
	/// <returns>The name of the power-up.</returns>
	public override string ToString() {
		return "Fly Swatter";
	}
}
