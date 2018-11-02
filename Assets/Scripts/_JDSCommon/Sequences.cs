using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class Sequences
{

	// This nay happen after some Monobehaviour.Awake() are called.
	static Sequences ()
	{
		CalculateAll ();
	}

	//
	public static Coord2[][] squareLayers;
	public static Coord2[] squares;
	public static Coord2[][] circleLayers;
	public static Coord2[] circles;

	static void CalculateAll ()
	{

		int maxDist = 32;
		int maxTaxi = maxDist * 2;

		List<Coord2>[] tempSquare = new List<Coord2>[maxDist + 1];
		List<Coord2>[] tempCircle = new List<Coord2>[maxDist + 1];

		for (int taxiDist = 0; taxiDist <= maxTaxi; taxiDist++) {
			for (int y = 0 - taxiDist; y <= taxiDist; y++) {
				int x = Util.Abs (y) - taxiDist;

				for (int side = 0; side < 2; side++) {

					Coord2 pos = new Coord2 (x, y);

					//
					int squareDist = Util.SquareDistance (Coord2.zero, pos);
					if (squareDist > maxDist) {
						x = 0 - x;
						continue;
					}

					if (tempSquare [squareDist] == null) {
						tempSquare [squareDist] = new List<Coord2> ();
					}
					if (!tempSquare [squareDist].Contains (pos)) {
						tempSquare [squareDist].Add (pos);
					}

					//
					int circleDist = Mathf.RoundToInt (Coord2.Distance (Coord2.zero, pos));
					if (circleDist > maxDist) {
						x = 0 - x;
						continue;
					}

					if (tempCircle [circleDist] == null) {
						tempCircle [circleDist] = new List<Coord2> ();
					}
					if (!tempCircle [circleDist].Contains (pos)) {
						tempCircle [circleDist].Add (pos);
					}

					x = 0 - x;
				}

			}
		}

		{
			squareLayers = new Coord2[tempSquare.Length][];
			squares = new Coord2[(maxDist * 2 + 1) * (maxDist * 2 + 1)];
			int flatSquareIdx = 0;

			for (int i = 0; i < tempSquare.Length; i++) {
				squareLayers [i] = new Coord2[tempSquare [i].Count];

				for (int j = 0; j < tempSquare [i].Count; j++) {

					squareLayers [i] [j] = tempSquare [i] [j];
					squares [flatSquareIdx++] = tempSquare [i] [j];

				}
			}
		}

		{
			circleLayers = new Coord2[tempCircle.Length][];
			circles = new Coord2[(maxDist * 2 + 1) * (maxDist * 2 + 1)];
			int flatCircleIdx = 0;

			for (int i = 0; i < tempCircle.Length; i++) {
				circleLayers [i] = new Coord2[tempCircle [i].Count];

				for (int j = 0; j < tempCircle [i].Count; j++) {

					circleLayers [i] [j] = tempCircle [i] [j];
					circles [flatCircleIdx++] = tempCircle [i] [j];

				}
			}
		}

	}
	
}
