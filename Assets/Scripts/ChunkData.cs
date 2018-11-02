using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkData : MonoBehaviour
{

    //
    public static readonly int ChunkWidth = 12;

    //
    public Coord3 chunkId;
    public Coord3 chunkLocalOrigin;
    public bool isNatural;
    private VoxelData[,,] voxels;

    public VoxelContainer voxelContainer;
    private ChunkMesh meshController;

    //
    public void Init(VoxelContainer parentContainer, Coord3 chunkId)
    {
        voxelContainer = parentContainer;
        this.chunkId = chunkId;
        chunkLocalOrigin = chunkId * ChunkWidth;
        this.transform.localPosition = chunkLocalOrigin;
        isNatural = true;
        voxels = null;

        meshController = this.gameObject.AddComponent<ChunkMesh>();
        meshController.Init();


        for (int i = 0; i < ChunkWidth; i++)
        {
            for (int j = 0; j < ChunkWidth; j++)
            {
                for (int k = 0; k < ChunkWidth; k++)
                {
                    Coord3 chunkPos = new Coord3(i, j, k);

                    meshController.MarkVoxelForRefresh(chunkPos);
                }
            }
        }
    }

    public void MarkAsChanged()
    {
        if (!isNatural)
        {
            return;
        }

        voxels = new VoxelData[ChunkWidth, ChunkWidth, ChunkWidth];

        for (int i = 0; i < ChunkWidth; i++)
        {
            for (int j = 0; j < ChunkWidth; j++)
            {
                for (int k = 0; k < ChunkWidth; k++)
                {
                    Coord3 localPos = chunkLocalOrigin + new Coord3(i, j, k);

                    voxels[localPos.x, localPos.y, localPos.z] = VoxelData.empty;
                }
            }
        }

        isNatural = false;
    }

    public Coord3 ChunkToLocal(Coord3 chunkPos)
    {
        return chunkPos + chunkLocalOrigin;
    }

    public Coord3 LocalToChunk(Coord3 localPos)
    {
        return localPos - chunkLocalOrigin;
    }

    public bool IsInRangeLocal(Coord3 localPos)
    {
        return IsInRangeChunk(LocalToChunk(localPos));
    }

    public bool IsInRangeChunk(Coord3 chunkPos)
    {
        return chunkPos.IsInRange(Coord3.zero, (Coord3.one * ChunkWidth) - Coord3.one);
    }

    public VoxelData GetVoxel(Coord3 localPos)
    {
        if(chunkId.x == 1)
        {
            int a = 1;
            a += 1;
        }

        if (!IsInRangeLocal(localPos))
        {
            return VoxelData.empty;
        }

        if (isNatural)
        {
            return new VoxelData(NaturalVoxelID(localPos));
        }
        else
        {
            Coord3 chunkPos = LocalToChunk(localPos);
            return voxels[chunkPos.x, chunkPos.y, chunkPos.z];
        }
    }

    public VoxelTypeID NaturalVoxelID(Coord3 localPos)
    {
        if (!IsInRangeLocal(localPos))
        {
            return VoxelTypeID.Empty;
        }

        VoxelTypeID result = VoxelTypeID.Empty;

        if (Coord3.Distance(voxelContainer.LocalCenter, localPos) < ((voxelContainer.localMaxPos.x / 2f) - 1))
        {
            result = VoxelTypeID.Rock;
        }

        return result;
    }

    public void SetVoxelData(Coord3 localPos, VoxelData voxel)
    {

        if (!IsInRangeLocal(localPos))
        {
            return;
        }

        MarkAsChanged();

        Coord3 chunkPos = localPos - chunkLocalOrigin;

        if (voxels[chunkPos.x, chunkPos.y, chunkPos.z].id == voxel.id)
        {
            return;
        }

        voxels[chunkPos.x, chunkPos.y, chunkPos.z] = voxel;

        meshController.MarkVoxelForRefresh(chunkPos);
    }

}
