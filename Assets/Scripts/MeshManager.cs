using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MeshManager : MonoBehaviour
{

    public static readonly int maxVertCount = 32000; // 65000;

	public VoxelContainer world;
	public Material material;

	private Coord2[,,,] meshArrayStarts = new Coord2[VoxelContainer.sizeXZ, VoxelContainer.sizeY, VoxelContainer.sizeXZ, 6];

	List<VoxelMesh> meshChildren = new List<VoxelMesh> ();

	private int currentBatchNumber = 1;
	private int[,,] batchNumbers = new int[VoxelContainer.sizeXZ, VoxelContainer.sizeY, VoxelContainer.sizeXZ];
	private Queue<Coord3> currentQueue = new Queue<Coord3> ();
	
	bool isRefreshing = false;

	//
	public void Init ()
	{
		
		for (int idx0 = 0; idx0 < meshArrayStarts.GetLength (0); idx0++) {
			for (int idx1 = 0; idx1 < meshArrayStarts.GetLength (1); idx1++) {
				for (int idx2 = 0; idx2 < meshArrayStarts.GetLength (2); idx2++) {
					for (int idx3 = 0; idx3 < meshArrayStarts.GetLength (3); idx3++) {
						
						meshArrayStarts [idx0, idx1, idx2, idx3] = Coord2.unitNeg;

					}
				}
			}
		}

		currentBatchNumber = 1;
	}

	public void MarkVoxelForRefresh (Coord3 pos)
	{
		if (batchNumbers [pos.x, pos.y, pos.z] != currentBatchNumber) {
			batchNumbers [pos.x, pos.y, pos.z] = currentBatchNumber;
			currentQueue.Enqueue (pos);
		}
	}

	private bool IsInCurrentBatch (Coord3 pos)
	{
		if (!world.IsInRange (pos)) {
			return true;
		}

		return (batchNumbers [pos.x, pos.y, pos.z] == currentBatchNumber);
	}

	public void DoBatchRefresh ()
	{
		StartCoroutine (BatchRefreshCR ());
	}

	private IEnumerator BatchRefreshCR ()
	{
		if (isRefreshing) {
			yield break;
		}

		isRefreshing = true;

		while (currentQueue.Count > 0) {
			Coord3 pos = currentQueue.Dequeue ();
			RefreshVoxelMesh (pos);

			// Causes a bug occasionally where faces get lost
//			if (FrameLimit.IsOverLimit ()) {
//				yield return null;
//			}
		}

		foreach (VoxelMesh mesh in meshChildren) {
			mesh.Apply ();
		}

		currentBatchNumber += 1;

		isRefreshing = false;
	}

	private void RefreshVoxelMesh (Coord3 pos)
	{
		VoxelData thisData = world.GetVoxel (pos);

		for (int thisFaceIdx = 0; thisFaceIdx < 6; thisFaceIdx++) {
			Coord3 neighborPos = pos + faceTemplates [thisFaceIdx].normal;

			int neighborFaceIdx;
			if ((thisFaceIdx % 2) == 0) {
				neighborFaceIdx = thisFaceIdx + 1;
			} else {
				neighborFaceIdx = thisFaceIdx - 1;
			}

			VoxelData neighborData = world.GetVoxel (pos + faceTemplates [thisFaceIdx].normal);

			RefreshFace (pos, thisFaceIdx, thisData, neighborData);

			if (!IsInCurrentBatch (neighborPos)) {
				RefreshFace (neighborPos, neighborFaceIdx, neighborData, thisData);
			}
			
		}
	}

	private void RefreshFace (Coord3 thisPos, int thisFaceIdx, VoxelData thisData, VoxelData neighborData)
	{
		if (!world.IsInRange (thisPos)) {
			return;
		}

		Coord2 maStart = meshArrayStarts [thisPos.x, thisPos.y, thisPos.z, thisFaceIdx];

		if (thisData.def.color.a > 0 && neighborData.def.color.a < 255 && (thisPos.y > 0 || faceTemplates [thisFaceIdx].normal.y >= 0)) {
			// Draw face

			FaceTemplate curFace = faceTemplates [thisFaceIdx];

			if (maStart.x < 0) {
				maStart = AvailableMeshArrayStart ();
				meshArrayStarts [thisPos.x, thisPos.y, thisPos.z, thisFaceIdx] = maStart;
			}

			VoxelMesh curMesh = meshChildren [maStart.x];

			for (int idxOffset = 0; idxOffset < 4; idxOffset++) {
				int meshArrayIdx = maStart.y + idxOffset;

				//
				curMesh.vertices [meshArrayIdx] = thisPos + curFace.vertices [idxOffset];
				curMesh.normals [meshArrayIdx] = curFace.normal;

				// Checkerboard pattern
				Color32 color = thisData.def.color;
				if ((thisPos.x + thisPos.y + thisPos.z) % 2 == 0) {
					color = Color32.Lerp (color, Color.black, (1f / 30f));
				}
				curMesh.colors [meshArrayIdx] = color;
			}


		} else {
			// Erase face

			if (maStart.x < 0) {
				// Do nothing

			} else {
				VoxelMesh curMesh = meshChildren [maStart.x];

				curMesh.availableArrayStarts.Enqueue (maStart.y);
				meshArrayStarts [thisPos.x, thisPos.y, thisPos.z, thisFaceIdx] = Coord2.unitNeg;

				for (int idxOffset = 0; idxOffset < 4; idxOffset++) {
					int meshArrayIdx = maStart.y + idxOffset;

					//
					curMesh.vertices [meshArrayIdx] = Vector3.zero;
					curMesh.normals [meshArrayIdx] = Vector3.zero;
					curMesh.colors [meshArrayIdx] = Color.clear;
				}

			}

		}

	}

	private Coord2 AvailableMeshArrayStart ()
	{
		for (int childIdx = 0; childIdx < meshChildren.Count; childIdx++) {
			if (meshChildren [childIdx].availableArrayStarts.Count > 0) {
				return new Coord2 (childIdx, meshChildren [childIdx].availableArrayStarts.Dequeue ());
			}
		}

		CreateChildMesh ();

		return new Coord2 (meshChildren.Count - 1, meshChildren [meshChildren.Count - 1].availableArrayStarts.Dequeue ());
	}

	private void CreateChildMesh ()
	{
		VoxelMesh vMesh = new GameObject ("VoxelMesh" + meshChildren.Count).AddComponent<VoxelMesh> ();
		vMesh.transform.SetParent (this.transform);
		vMesh.Init (material);
		meshChildren.Add (vMesh);
	}

	//
	private class FaceTemplate
	{
		public Coord3 normal;
		public Vector3[] vertices;
		//
		public FaceTemplate (Coord3 nrml, params Vector3[] vrts)
		{
			this.normal = nrml;
			this.vertices = vrts;
		}
	}

	private FaceTemplate[] faceTemplates = new FaceTemplate[] {
		new FaceTemplate (new Coord3 (-1, 0, 0), new Vector3 (0, 0, 0), new Vector3 (0, 0, 1), new Vector3 (0, 1, 1), new Vector3 (0, 1, 0)), // Left
		new FaceTemplate (new Coord3 (+1, 0, 0), new Vector3 (1, 0, 0), new Vector3 (1, 1, 0), new Vector3 (1, 1, 1), new Vector3 (1, 0, 1)), // Right
		new FaceTemplate (new Coord3 (0, -1, 0), new Vector3 (0, 0, 0), new Vector3 (1, 0, 0), new Vector3 (1, 0, 1), new Vector3 (0, 0, 1)), // Bottom
		new FaceTemplate (new Coord3 (0, +1, 0), new Vector3 (0, 1, 0), new Vector3 (0, 1, 1), new Vector3 (1, 1, 1), new Vector3 (1, 1, 0)), // Top
		new FaceTemplate (new Coord3 (0, 0, -1), new Vector3 (0, 0, 0), new Vector3 (0, 1, 0), new Vector3 (1, 1, 0), new Vector3 (1, 0, 0)), // Front
		new FaceTemplate (new Coord3 (0, 0, +1), new Vector3 (0, 0, 1), new Vector3 (1, 0, 1), new Vector3 (1, 1, 1), new Vector3 (0, 1, 1)), // Back
	};

}
