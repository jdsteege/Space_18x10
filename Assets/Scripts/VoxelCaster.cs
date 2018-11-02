using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelCaster
{

	private VoxelContainer world;

	public void Init (VoxelContainer world)
	{
		this.world = world;
	}

	public VoxelRaycastHitInfo Raycast (Ray ray, float maxDistance)
	{
		VoxelRaycastHitInfo result = new VoxelRaycastHitInfo ();

		//
		float maxDistSqr = maxDistance * maxDistance;
		Vector3 castPoint = ray.origin;
		Coord3 curPos = new Coord3 (castPoint);
		Coord3 prevPos = curPos;

		//
		Coord3 crossOffsets = new Coord3 (
			                      (ray.direction.x >= 0) ? 1 : 0,
			                      (ray.direction.y >= 0) ? 1 : 0,
			                      (ray.direction.z >= 0) ? 1 : 0
		                      );
		Coord3 nextOffsets = new Coord3 (
			                     ray.direction.x >= 0 ? 1 : -1,
			                     ray.direction.y >= 0 ? 1 : -1,
			                     ray.direction.z >= 0 ? 1 : -1
		                     );

		Vector3 dir = ray.direction;
		if (dir.x == 0f) {
			dir.x = (1f / 10000f);
		}
		if (dir.y == 0f) {
			dir.y = (1f / 10000f);
		}
		if (dir.z == 0f) {
			dir.z = (1f / 10000f);
		}

		//
		while (!result.didHit && (castPoint - ray.origin).sqrMagnitude <= maxDistSqr) {

			prevPos = curPos;

			float crossX = Mathf.FloorToInt (castPoint.x) + crossOffsets.x;
			if (crossX == castPoint.x) {
				crossX += nextOffsets.x;
			}

			float crossY = Mathf.FloorToInt (castPoint.y) + crossOffsets.y;
			if (crossY == castPoint.y) {
				crossY += nextOffsets.y;
			}

			float crossZ = Mathf.FloorToInt (castPoint.z) + crossOffsets.z;
			if (crossZ == castPoint.z) {
				crossZ += nextOffsets.z;
			}

			float weightX = (crossX - castPoint.x) / dir.x;
			float weightY = (crossY - castPoint.y) / dir.y;
			float weightZ = (crossZ - castPoint.z) / dir.z;

			if (weightX <= weightY && weightX <= weightZ) {
				float diffY = weightX * dir.y;
				float diffZ = weightX * dir.z;

				castPoint = new Vector3 (crossX, castPoint.y + diffY, castPoint.z + diffZ);
				curPos = curPos + new Coord3 (nextOffsets.x, 0, 0);
				
			} else if (weightY <= weightX && weightY <= weightZ) {
				float diffX = weightY * dir.x;
				float diffZ = weightY * dir.z;

				castPoint = new Vector3 (castPoint.x + diffX, crossY, castPoint.z + diffZ);
				curPos = curPos + new Coord3 (0, nextOffsets.y, 0);
				
			} else {
				float diffX = weightZ * dir.x;
				float diffY = weightZ * dir.y;

				castPoint = new Vector3 (castPoint.x + diffX, castPoint.y + diffY, crossZ);
				curPos = curPos + new Coord3 (0, 0, nextOffsets.z);

			}

			if (world.GetVoxel (curPos).Def.color.a > 0) {
				result.didHit = true;
				result.voxelPos = curPos;
				result.faceNeighbor = prevPos;
			}

		}

		return result;
	}

}

public class VoxelRaycastHitInfo
{
	public bool didHit = false;
	public Coord3 voxelPos = Coord3.unitNeg;
	public Coord3 faceNeighbor = Coord3.unitNeg;
}