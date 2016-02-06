using UnityEngine;
using System;

// Utility functions related to generating random values.
public class RandomUtil {

	// The random number generator to be used.
	public static System.Random random = new System.Random();

	// Gets a random number between a minimum value (inclusive) and a maximum value (exclusive).
	public static int RandomInt (int min, int max) {
		return random.Next (min, max);
	}

	// Gets a random boolean value.
	public static bool RandomBoolean() {
		return RandomInt (0, 2) == 0;
	}
}
