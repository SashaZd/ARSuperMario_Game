using UnityEngine;
using System;

// Utility functions related to generating random values.
/// <summary>
/// Random util.
/// </summary>
public class RandomUtil {

	/// <summary>
	/// Gets a random integer between a minimum value (inclusive) and a maximum value (exclusive).
	/// </summary>
	/// <returns>A random integer between the minimum value (inclusive) and the maximum value (exclusive). </returns>
	/// <param name="min">The minimum value of the integer.</param>
	/// <param name="max">The maximum value of the integer.</param>
	public static int RandomInt (int min, int max) {
		return (int)(UnityEngine.Random.value * (max - min) + min);
	}

	/// <summary>
	/// Gets a random boolean value.
	/// </summary>
	/// <returns>A random boolean value.</returns>
	public static bool RandomBoolean() {
		return RandomInt(0, 2) == 0;
	}
}
