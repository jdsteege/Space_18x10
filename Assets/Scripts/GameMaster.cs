using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameMaster : MonoBehaviour {

    public Material planetMaterial;

    void Awake ()
    {

        VoxelDef.InitDefs();

    }
	
}
