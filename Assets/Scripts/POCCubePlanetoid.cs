using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class POCCubePlanetoid : MonoBehaviour
{

    //
    public int size = 3;
    private Vector3Int middle;

    //
    private VoxelData[,,] blocks;
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> normals = new List<Vector3>();
    private List<int> triangles = new List<int>();

    //
    void Start()
    {
        VoxelDef.InitDefs();

        blocks = new VoxelData[size, size, size];
        int extent = (size - 1) / 2;
        middle = new Vector3Int(extent, extent, extent);

        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    Vector3Int pos = new Vector3Int(i, j, k);

                    if (Vector3Int.Distance(middle, pos) <= extent)
                    {
                        blocks[i, j, k] = new VoxelData(VoxelTypeID.Rock);

                    }
                    else
                    {
                        blocks[i, j, k] = new VoxelData(VoxelTypeID.Air);

                    }

                }
            }
        }

        //
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                for (int k = 0; k < size; k++)
                {
                    Vector3Int pos = new Vector3Int(i, j, k);
                    RefreshVoxelMesh(pos);
                }
            }
        }

        GetComponent<MeshFilter>().sharedMesh.Clear();
        GetComponent<MeshFilter>().sharedMesh.SetVertices(vertices);
        GetComponent<MeshFilter>().sharedMesh.SetNormals(normals);
        GetComponent<MeshFilter>().sharedMesh.SetTriangles(triangles, 0);

        GetComponent<MeshFilter>().sharedMesh.RecalculateBounds();
    }

    public bool IsInRange(Vector3Int pos)
    {
        return (pos.x >= 0 && pos.x < size && pos.y >= 0 && pos.y < size && pos.z >= 0 && pos.z < size);
    }

    public VoxelData GetVoxel(Vector3Int pos)
    {
        if (!IsInRange(pos))
        {
            return VoxelData.air;
        }
        return blocks[pos.x, pos.y, pos.z];
    }

    //
    private void RefreshVoxelMesh(Vector3Int pos)
    {
        VoxelData thisData = GetVoxel(pos);

        for (int thisFaceIdx = 0; thisFaceIdx < 6; thisFaceIdx++)
        {
            //Vector3Int neighborPos = pos + faceTemplates[thisFaceIdx].normal;

            //int neighborFaceIdx;
            //if ((thisFaceIdx % 2) == 0)
            //{
            //    neighborFaceIdx = thisFaceIdx + 1;
            //}
            //else
            //{
            //    neighborFaceIdx = thisFaceIdx - 1;
            //}

            VoxelData neighborData = GetVoxel(pos + faceTemplates[thisFaceIdx].normal);

            RefreshFace(pos, thisFaceIdx, thisData, neighborData);

            //if (!IsInCurrentBatch(neighborPos))
            //{
            //    RefreshFace(neighborPos, neighborFaceIdx, neighborData, thisData);
            //}

        }
    }

    private void RefreshFace(Vector3Int thisPos, int thisFaceIdx, VoxelData thisData, VoxelData neighborData)
    {
        if (!IsInRange(thisPos))
        {
            return;
        }

        //Coord2 maStart = meshArrayStarts[thisPos.x, thisPos.y, thisPos.z, thisFaceIdx];

        if (thisData.def.color.a > 0 && neighborData.def.color.a < 255 && (thisPos.y > 0 || faceTemplates[thisFaceIdx].normal.y >= 0))
        {
            // Draw face

            FaceTemplate curFace = faceTemplates[thisFaceIdx];

            //if (maStart.x < 0)
            //{
            //    maStart = AvailableMeshArrayStart();
            //    meshArrayStarts[thisPos.x, thisPos.y, thisPos.z, thisFaceIdx] = maStart;
            //}

            //VoxelMesh curMesh = meshChildren[maStart.x];

            int vertStartIdx = vertices.Count;

            for (int idxOffset = 0; idxOffset < 4; idxOffset++)
            {
                vertices.Add(thisPos + curFace.vertices[idxOffset]);
                normals.Add(((Vector3)(vertices[vertices.Count - 1] - middle)).normalized);

                //    int meshArrayIdx = maStart.y + idxOffset;

                //    //
                //    curMesh.vertices[meshArrayIdx] = thisPos.ToVector3() + curFace.vertices[idxOffset];
                //    curMesh.normals[meshArrayIdx] = curFace.normal.ToVector3();

                //    // Checkerboard pattern
                //    Color32 color = thisData.def.color;
                //    if ((thisPos.x + thisPos.y + thisPos.z) % 2 == 0)
                //    {
                //        color = Color32.Lerp(color, Color.black, (1f / 30f));
                //    }
                //    curMesh.colors[meshArrayIdx] = color;
            }

            triangles.Add(vertStartIdx + 0);
            triangles.Add(vertStartIdx + 1);
            triangles.Add(vertStartIdx + 2);
            triangles.Add(vertStartIdx + 2);
            triangles.Add(vertStartIdx + 3);
            triangles.Add(vertStartIdx + 0);

        }
        else
        {
            //// Erase face

            //if (maStart.x < 0)
            //{
            //    // Do nothing

            //}
            //else
            //{
            //    VoxelMesh curMesh = meshChildren[maStart.x];

            //    curMesh.availableArrayStarts.Enqueue(maStart.y);
            //    meshArrayStarts[thisPos.x, thisPos.y, thisPos.z, thisFaceIdx] = Coord2.unitNeg;

            //    for (int idxOffset = 0; idxOffset < 4; idxOffset++)
            //    {
            //        int meshArrayIdx = maStart.y + idxOffset;

            //        //
            //        curMesh.vertices[meshArrayIdx] = Vector3.zero;
            //        curMesh.normals[meshArrayIdx] = Vector3.zero;
            //        curMesh.colors[meshArrayIdx] = Color.clear;
            //    }

            //}

        }

    }

    //
    private class FaceTemplate
    {
        public Vector3Int normal;
        public Vector3[] vertices;
        //
        public FaceTemplate(Vector3Int nrml, params Vector3[] vrts)
        {
            this.normal = nrml;
            this.vertices = vrts;
        }
    }

    private FaceTemplate[] faceTemplates = new FaceTemplate[] {
        new FaceTemplate (new Vector3Int (-1, 0, 0), new Vector3 (0, 0, 0), new Vector3 (0, 0, 1), new Vector3 (0, 1, 1), new Vector3 (0, 1, 0)), // Left
		new FaceTemplate (new Vector3Int (+1, 0, 0), new Vector3 (1, 0, 0), new Vector3 (1, 1, 0), new Vector3 (1, 1, 1), new Vector3 (1, 0, 1)), // Right
		new FaceTemplate (new Vector3Int (0, -1, 0), new Vector3 (0, 0, 0), new Vector3 (1, 0, 0), new Vector3 (1, 0, 1), new Vector3 (0, 0, 1)), // Bottom
		new FaceTemplate (new Vector3Int (0, +1, 0), new Vector3 (0, 1, 0), new Vector3 (0, 1, 1), new Vector3 (1, 1, 1), new Vector3 (1, 1, 0)), // Top
		new FaceTemplate (new Vector3Int (0, 0, -1), new Vector3 (0, 0, 0), new Vector3 (0, 1, 0), new Vector3 (1, 1, 0), new Vector3 (1, 0, 0)), // Front
		new FaceTemplate (new Vector3Int (0, 0, +1), new Vector3 (0, 0, 1), new Vector3 (1, 0, 1), new Vector3 (1, 1, 1), new Vector3 (0, 1, 1)), // Back
	};

}
