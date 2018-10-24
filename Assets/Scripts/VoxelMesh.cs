using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VoxelMesh : MonoBehaviour
{

	private static readonly int maxVertCount = MeshManager.maxVertCount;

	private MeshFilter meshFilter;
	private MeshRenderer meshRenderer;
	private Mesh mesh;

	public Vector3[] vertices = new Vector3[maxVertCount];
	public Vector3[] normals = new Vector3[maxVertCount];
	public Color32[] colors = new Color32[maxVertCount];
	public int[] triangles = new int[(maxVertCount * 6) / 4];

	public Queue<int> availableArrayStarts = new Queue<int> (maxVertCount / 4);

	//
	public void Init (Material material)
	{
		gameObject.name = "VoxelMesh";

		meshFilter = gameObject.AddComponent<MeshFilter> ();
		mesh = meshFilter.mesh;
		mesh.MarkDynamic ();
		meshRenderer = gameObject.AddComponent<MeshRenderer> ();
		meshRenderer.material = material;

		gameObject.AddComponent<MeshCollider> ();

		//
		for (int v = 0; v < vertices.Length; v++) {
			vertices [v] = Vector3.zero;
			normals [v] = Vector3.zero;
			colors [v] = Color.clear;
		}

		int vStart = 0;
		for (int t = 0; vStart < vertices.Length; t += 6) {
			triangles [t + 0] = vStart + 0;
			triangles [t + 1] = vStart + 1;
			triangles [t + 2] = vStart + 2;

			triangles [t + 3] = vStart + 2;
			triangles [t + 4] = vStart + 3;
			triangles [t + 5] = vStart + 0;

			vStart += 4;
		}

		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.colors32 = colors;
		mesh.triangles = triangles;

		Bounds b = new Bounds ();
		b.SetMinMax (Vector3.zero, new Vector3 (VoxelContainer.sizeXZ, VoxelContainer.sizeY, VoxelContainer.sizeXZ));
		mesh.bounds = b;

		for (int v = 0; v < vertices.Length; v += 4) {
			availableArrayStarts.Enqueue (v);
		}

	}

	public void Apply ()
	{
		mesh.vertices = vertices;
		mesh.normals = normals;
		mesh.colors32 = colors;
	}

}
