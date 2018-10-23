using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Sequences
{

	// This nay happe after some Monobehaviour.Awake() are called.
	static Sequences ()
	{
		CalculateAll ();
	}

	//
	public static Vector2Int[][] squareLayers;
	public static Vector2Int[] squares;
	public static Vector2Int[][] circleLayers;
	public static Vector2Int[] circles;

	static void CalculateAll ()
	{

		int maxDist = 32;
		int maxTaxi = maxDist * 2;

		List<Vector2Int>[] tempSquare = new List<Vector2Int>[maxDist + 1];
		List<Vector2Int>[] tempCircle = new List<Vector2Int>[maxDist + 1];

		for (int taxiDist = 0; taxiDist <= maxTaxi; taxiDist++) {
			for (int y = 0 - taxiDist; y <= taxiDist; y++) {
				int x = Util.Abs (y) - taxiDist;

				for (int side = 0; side < 2; side++) {

					Vector2Int pos = new Vector2Int (x, y);

					//
					int squareDist = Util.SquareDistance (Vector2Int.zero, pos);
					if (squareDist > maxDist) {
						x = 0 - x;
						continue;
					}

					if (tempSquare [squareDist] == null) {
						tempSquare [squareDist] = new List<Vector2Int> ();
					}
					if (!tempSquare [squareDist].Contains (pos)) {
						tempSquare [squareDist].Add (pos);
					}

					//
					int circleDist = Mathf.RoundToInt (Vector2Int.Distance (Vector2Int.zero, pos));
					if (circleDist > maxDist) {
						x = 0 - x;
						continue;
					}

					if (tempCircle [circleDist] == null) {
						tempCircle [circleDist] = new List<Vector2Int> ();
					}
					if (!tempCircle [circleDist].Contains (pos)) {
						tempCircle [circleDist].Add (pos);
					}

					x = 0 - x;
				}

			}
		}

		{
			squareLayers = new Vector2Int[tempSquare.Length][];
			squares = new Vector2Int[(maxDist * 2 + 1) * (maxDist * 2 + 1)];
			int flatSquareIdx = 0;

			for (int i = 0; i < tempSquare.Length; i++) {
				squareLayers [i] = new Vector2Int[tempSquare [i].Count];

				for (int j = 0; j < tempSquare [i].Count; j++) {

					squareLayers [i] [j] = tempSquare [i] [j];
					squares [flatSquareIdx++] = tempSquare [i] [j];

				}
			}
		}

		{
			circleLayers = new Vector2Int[tempCircle.Length][];
			circles = new Vector2Int[(maxDist * 2 + 1) * (maxDist * 2 + 1)];
			int flatCircleIdx = 0;

			for (int i = 0; i < tempCircle.Length; i++) {
				circleLayers [i] = new Vector2Int[tempCircle [i].Count];

				for (int j = 0; j < tempCircle [i].Count; j++) {

					circleLayers [i] [j] = tempCircle [i] [j];
					circles [flatCircleIdx++] = tempCircle [i] [j];

				}
			}
		}

	}
	
}
