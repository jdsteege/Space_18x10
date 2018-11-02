using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelContainer : MonoBehaviour
{

    public VoxelCaster caster = new VoxelCaster();
    public int sizeX;
    public int sizeY;
    public int sizeZ;

    private ChunkData[,,] chunks;
    public Coord3 localMaxPos;
    //private bool isLoaded;

    public Coord3 WorldOrigin
    {
        get { return new Coord3(transform.position); }
    }
    public Coord3 LocalCenter
    {
        get { return localMaxPos * 0.5f; }
    }

    private void Start()
    {
        Init(new Coord3(sizeX, sizeY, sizeZ));
    }

    void Init(Coord3 sizeInChunks)
    {
        //isLoaded = false;

        localMaxPos = (sizeInChunks * ChunkData.ChunkWidth) - Coord3.one;

        caster.Init(this);

        //
        chunks = new ChunkData[sizeInChunks.x, sizeInChunks.y, sizeInChunks.z];
        for (int i = 0; i < chunks.GetLength(0); i++)
        {
            for (int j = 0; j < chunks.GetLength(1); j++)
            {
                for (int k = 0; k < chunks.GetLength(2); k++)
                {
                    GameObject chunkObject = new GameObject("Chunk(" + i + "," + j + "," + k + ")", typeof(ChunkData));
                    chunkObject.transform.SetParent(this.transform);

                    chunkObject.GetComponent<ChunkData>().Init(this, new Coord3(i, j, k));
                    chunks[i, j, k] = chunkObject.GetComponent<ChunkData>();
                }
            }
        }

        //isLoaded = true;
    }

    public bool IsInRange(Coord3 localPos)
    {
        return localPos.IsInRange(Coord3.zero, localMaxPos);
    }

    public ChunkData GetChunk(Coord3 localPos)
    {
        if (IsInRange(localPos))
        {
            return chunks[localPos.x / ChunkData.ChunkWidth, localPos.y / ChunkData.ChunkWidth, localPos.z / ChunkData.ChunkWidth];
        }
        else
        {
            return null;
        }
    }

    public VoxelData GetVoxel(Coord3 localPos)
    {
        if (IsInRange(localPos))
        {
            return GetChunk(localPos).GetVoxel(localPos);

        }
        else
        {
            return VoxelData.empty;
        }
    }

    public void SetVoxelID(Coord3 localPos, VoxelTypeID id)
    {
        VoxelData voxel = new VoxelData(id);

        SetVoxelData(localPos, voxel);
    }

    public void SetVoxelData(Coord3 localPos, VoxelData voxel)
    {

        if (!IsInRange(localPos))
        {
            return;
        }

        GetChunk(localPos).SetVoxelData(localPos, voxel);
    }

}
