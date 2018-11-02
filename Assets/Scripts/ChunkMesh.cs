using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChunkMesh : MonoBehaviour
{

    //
    public Material material;

    //
    private MeshFilter meshFilter;
    private MeshRenderer meshRenderer;
    private Mesh mesh;
    private ChunkData dataController;

    // TODO: when drawing natural chunk, dont need to store all this data.
    //
    private List<Vector3> vertices = new List<Vector3>();
    private List<Vector3> normals = new List<Vector3>();
    private List<Color32> colors = new List<Color32>();
    private List<int> triangles = new List<int>();

    //
    private int[,,,] meshArrayStarts = new int[ChunkData.ChunkWidth, ChunkData.ChunkWidth, ChunkData.ChunkWidth, 6];
    public Queue<int> availableArrayStarts = new Queue<int>();

    //
    private int currentBatchNumber;
    private int[,,] batchNumbers = new int[ChunkData.ChunkWidth, ChunkData.ChunkWidth, ChunkData.ChunkWidth];
    private Queue<Coord3> currentQueue = new Queue<Coord3>();

    bool isRefreshing = false;

    //
    public void Init()
    {
        currentBatchNumber = 1;
        dataController = GetComponent<ChunkData>();

        meshFilter = gameObject.AddComponent<MeshFilter>();
        mesh = meshFilter.mesh;
        mesh.MarkDynamic();
        meshRenderer = gameObject.AddComponent<MeshRenderer>();
        meshRenderer.material = material;

        gameObject.AddComponent<MeshCollider>();

        {
            Bounds b = new Bounds();
            Vector3 min = dataController.voxelContainer.WorldOrigin + dataController.chunkLocalOrigin;
            b.SetMinMax(min, min + new Vector3(ChunkData.ChunkWidth, ChunkData.ChunkWidth, ChunkData.ChunkWidth));
            mesh.bounds = b;
        }

        for (int idx0 = 0; idx0 < meshArrayStarts.GetLength(0); idx0++)
        {
            for (int idx1 = 0; idx1 < meshArrayStarts.GetLength(1); idx1++)
            {
                for (int idx2 = 0; idx2 < meshArrayStarts.GetLength(2); idx2++)
                {
                    for (int idx3 = 0; idx3 < meshArrayStarts.GetLength(3); idx3++)
                    {

                        meshArrayStarts[idx0, idx1, idx2, idx3] = -1;

                    }
                }
            }
        }

    }

    public void Apply()
    {
        mesh.SetVertices(vertices);
        mesh.SetNormals(normals);
        mesh.SetColors(colors);
        mesh.SetTriangles(triangles, 0);
    }

    public void MarkVoxelForRefresh(Coord3 localPos)
    {
        if (batchNumbers[localPos.x, localPos.y, localPos.z] != currentBatchNumber)
        {
            batchNumbers[localPos.x, localPos.y, localPos.z] = currentBatchNumber;
            currentQueue.Enqueue(localPos);
        }
    }

    private bool IsInCurrentBatch(Coord3 localPos)
    {
        if (!dataController.IsInRange(localPos))
        {
            return true;
        }

        return (batchNumbers[localPos.x, localPos.y, localPos.z] == currentBatchNumber);
    }

    private void Update()
    {
        DoBatchRefresh();
    }

    public void DoBatchRefresh()
    {
        StartCoroutine(BatchRefreshCR());
    }

    private IEnumerator BatchRefreshCR()
    {
        if (isRefreshing || currentQueue.Count == 0)
        {
            yield break;
        }

        isRefreshing = true;

        while (currentQueue.Count > 0)
        {
            Coord3 localPos = currentQueue.Dequeue();
            RefreshVoxelMesh(localPos);

            // Causes a bug occasionally where faces get lost
            //			if (FrameLimit.IsOverLimit ()) {
            //				yield return null;
            //			}
        }

        Apply();

        currentBatchNumber += 1;

        isRefreshing = false;
    }

    private void RefreshVoxelMesh(Coord3 localPos)
    {
        VoxelData thisData = dataController.GetVoxel(localPos);

        for (int thisFaceIdx = 0; thisFaceIdx < 6; thisFaceIdx++)
        {
            Coord3 neighborPos = localPos + faceTemplates[thisFaceIdx].normal;

            int neighborFaceIdx;
            if ((thisFaceIdx % 2) == 0)
            {
                neighborFaceIdx = thisFaceIdx + 1;
            }
            else
            {
                neighborFaceIdx = thisFaceIdx - 1;
            }

            VoxelData neighborData = dataController.GetVoxel(localPos + faceTemplates[thisFaceIdx].normal);

            RefreshFace(localPos, thisFaceIdx, thisData, neighborData);

            if (!IsInCurrentBatch(neighborPos))
            {
                RefreshFace(neighborPos, neighborFaceIdx, neighborData, thisData);
            }

        }
    }

    private void RefreshFace(Coord3 thisLocalPos, int thisFaceIdx, VoxelData thisData, VoxelData neighborData)
    {
        if (!dataController.IsInRange(thisLocalPos))
        {
            return;
        }

        int maStart = meshArrayStarts[thisLocalPos.x, thisLocalPos.y, thisLocalPos.z, thisFaceIdx];

        if (thisData.Def.color.a > 0 && neighborData.Def.color.a < 255 && (thisLocalPos.y > 0 || faceTemplates[thisFaceIdx].normal.y >= 0))
        {
            // Draw face

            FaceTemplate curFace = faceTemplates[thisFaceIdx];

            if (maStart < 0)
            {
                maStart = GetAvailableMeshArrayStart();
                meshArrayStarts[thisLocalPos.x, thisLocalPos.y, thisLocalPos.z, thisFaceIdx] = maStart;
            }

            for (int idxOffset = 0; idxOffset < 4; idxOffset++)
            {
                int meshArrayIdx = maStart + idxOffset;

                //
                vertices[meshArrayIdx] = thisLocalPos + curFace.vertices[idxOffset];
                normals[meshArrayIdx] = curFace.normal;

                // Checkerboard pattern
                Color32 color = thisData.Def.color;
                if ((thisLocalPos.x + thisLocalPos.y + thisLocalPos.z) % 2 == 0)
                {
                    color = Color32.Lerp(color, Color.black, (1f / 30f));
                }
                colors[meshArrayIdx] = color;
            }


        }
        else
        {
            // Erase face

            if (maStart < 0)
            {
                // Do nothing

            }
            else
            {
                availableArrayStarts.Enqueue(maStart);
                meshArrayStarts[thisLocalPos.x, thisLocalPos.y, thisLocalPos.z, thisFaceIdx] = -1;

                for (int idxOffset = 0; idxOffset < 4; idxOffset++)
                {
                    int meshArrayIdx = maStart + idxOffset;

                    //
                    vertices[meshArrayIdx] = Vector3.zero;
                    normals[meshArrayIdx] = Vector3.zero;
                    colors[meshArrayIdx] = Color.clear;
                }

            }

        }

    }

    private int GetAvailableMeshArrayStart()
    {
        if (availableArrayStarts.Count > 0)
        {
            return availableArrayStarts.Dequeue();
        }
        else
        {
            int startIdx = vertices.Count;

            for (int i = 0; i < 4; i++)
            {
                vertices.Add(Vector3.zero);
                normals.Add(Vector3.zero);
                colors.Add(Color.clear);
            }

            triangles.Add(startIdx + 0);
            triangles.Add(startIdx + 1);
            triangles.Add(startIdx + 2);
            triangles.Add(startIdx + 2);
            triangles.Add(startIdx + 3);
            triangles.Add(startIdx + 0);

            return startIdx;
        }

    }

    //
    private class FaceTemplate
    {
        public Vector3 normal;
        public Vector3[] vertices;
        //
        public FaceTemplate(Vector3 nrml, params Vector3[] vrts)
        {
            this.normal = nrml;
            this.vertices = vrts;
        }
    }

    private FaceTemplate[] faceTemplates = new FaceTemplate[] {
        new FaceTemplate (new Vector3 (-1, 0, 0), new Vector3 (0, 0, 0), new Vector3 (0, 0, 1), new Vector3 (0, 1, 1), new Vector3 (0, 1, 0)), // Left
		new FaceTemplate (new Vector3 (+1, 0, 0), new Vector3 (1, 0, 0), new Vector3 (1, 1, 0), new Vector3 (1, 1, 1), new Vector3 (1, 0, 1)), // Right
		new FaceTemplate (new Vector3 (0, -1, 0), new Vector3 (0, 0, 0), new Vector3 (1, 0, 0), new Vector3 (1, 0, 1), new Vector3 (0, 0, 1)), // Bottom
		new FaceTemplate (new Vector3 (0, +1, 0), new Vector3 (0, 1, 0), new Vector3 (0, 1, 1), new Vector3 (1, 1, 1), new Vector3 (1, 1, 0)), // Top
		new FaceTemplate (new Vector3 (0, 0, -1), new Vector3 (0, 0, 0), new Vector3 (0, 1, 0), new Vector3 (1, 1, 0), new Vector3 (1, 0, 0)), // Front
		new FaceTemplate (new Vector3 (0, 0, +1), new Vector3 (0, 0, 1), new Vector3 (1, 0, 1), new Vector3 (1, 1, 1), new Vector3 (0, 1, 1)), // Back
	};

}
