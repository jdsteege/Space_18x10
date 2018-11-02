using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum VoxelTypeID : byte
{
    Empty,
    Rock,
    Sand,
    Water,
}

public class VoxelDef
{

    private static VoxelDef[] defs;

    public VoxelTypeID id;
    //public int weight;
    public Color32 color;
    //public float gravity;
    //public bool isFluid;

    //
    public VoxelDef(VoxelTypeID typeId)
    {
        //
        {
            this.id = typeId;
            //weight = 0;
            color = new Color32(255, 0, 255, 255);
            //gravity = 0f;
            //isFluid = false;
        }

        //
        if (id == VoxelTypeID.Empty)
        {
            //weight = 0;
            color = new Color32(255, 255, 255, 0);
            //gravity = 0f;
        }

        if (id == VoxelTypeID.Rock)
        {
            //weight = 100;
            color = new Color32(172, 172, 172, 255);
            //gravity = 0f;
        }

        if (id == VoxelTypeID.Sand)
        {
            //weight = 80;
            color = new Color32(255, 231, 165, 255);
            //gravity = 1f;
        }

        if (id == VoxelTypeID.Water)
        {
            //weight = 100;
            color = new Color32(0, 0, 200, 255);
            //gravity = 1f;
            //isFluid = true;
        }

        //
        {

        }
    }

    //
    public static void InitDefs()
    {
        defs = new VoxelDef[System.Enum.GetNames(typeof(VoxelTypeID)).Length];

        for (int i = 0; i < defs.Length; i++)
        {
            defs[i] = new VoxelDef((VoxelTypeID)i);
        }
    }

    public static VoxelDef GetDef(VoxelTypeID typeId)
    {
        return defs[(int)typeId];
    }

}

public struct VoxelData
{
    public static readonly VoxelData empty = new VoxelData(VoxelTypeID.Empty);

    public readonly VoxelTypeID id;

    //
    public VoxelData(VoxelTypeID p_id)
    {
        id = p_id;
    }

    //
    public VoxelDef Def
    {
        get
        {
            return VoxelDef.GetDef(this.id);
        }
    }
}
