using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelContainer : MonoBehaviour
{
	
	public static readonly int sizeXZ = 128;
	public static readonly int sizeY = 32;

	public MeshManager meshManager;
	//public TickManager tickManager;
	public VoxelCaster caster = new VoxelCaster ();

	private static readonly Coord3 maxPos = new Coord3 (sizeXZ - 1, sizeY - 1, sizeXZ - 1);
	private VoxelData[,,] voxels;
	private bool isLoaded;

	void Awake ()
	{
		Init ();
	}

	void Init ()
	{
		isLoaded = false;

		VoxelDef.InitDefs ();

		meshManager.Init ();
		caster.Init (this);

		//
		voxels = new VoxelData[sizeXZ, sizeY, sizeXZ];

		for (int x = 0; x < sizeXZ; x++) {
			for (int y = 0; y < sizeY; y++) {
				for (int z = 0; z < sizeXZ; z++) {

					VoxelTypeID newId = VoxelTypeID.Air;

					if (y < 2) {
						newId = VoxelTypeID.Rock;
					} else if (y < 5) {
						newId = VoxelTypeID.Sand;
//					} else if (y < 6 && Random.value < 0.3f) {
//						newId = VoxelTypeID.Sand;
					} else {
						newId = VoxelTypeID.Air;
					}

					SetVoxelID (new Coord3 (x, y, z), newId);

				}
			}
		}



		for (int x = 0; x < sizeXZ; x++) {
			for (int y = 0; y < sizeY; y++) {
				for (int z = 0; z < sizeXZ; z++) {

					meshManager.MarkVoxelForRefresh (new Coord3 (x, y, z));

				}
			}
		}

		isLoaded = true;
	}

	public bool IsInRange (Coord3 pos)
	{
		return pos.IsInRange (Coord3.zero, maxPos);
	}

	public VoxelData GetVoxel (Coord3 pos)
	{
		if (IsInRange (pos)) {
			return voxels [pos.x, pos.y, pos.z];
			
		} else {
			return VoxelData.air;
		}
	}

	public void SetVoxelID (Coord3 pos, VoxelTypeID id)
	{
		VoxelData voxel = new VoxelData (id);

		SetVoxelData (pos, voxel);
	}

	public void SetVoxelData (Coord3 pos, VoxelData voxel)
	{

		if (!IsInRange (pos)) {
			return;
		}
		if (voxels [pos.x, pos.y, pos.z].id == voxel.id) {
			return;
		}

//		VoxelData oldData = voxels [pos.x, pos.y, pos.z];
		voxels [pos.x, pos.y, pos.z] = voxel;

		if (!isLoaded) {
			return;
		}

		//tickManager.OnVoxelChange (pos);

		meshManager.MarkVoxelForRefresh (pos);
		
	}

}
