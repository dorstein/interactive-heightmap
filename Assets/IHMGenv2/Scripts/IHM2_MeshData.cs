using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHM2_MeshData {

	public Vector3[] verticies;
	public int[] triangles;
	public Vector2[] uvs;

	int triangleIndex = 0;

	public IHM2_MeshData(int meshWidth, int meshHeight){
		verticies = new Vector3[meshWidth * meshHeight];
		uvs = new Vector2[meshWidth * meshHeight];
		triangles = new int[(meshWidth - 1) * (meshHeight - 1) * 6];

	}

	public void AddTriangle(int a, int b, int c){
		triangles [triangleIndex] = a;
		triangles [triangleIndex + 1] = b;
		triangles [triangleIndex + 2] = c;
		triangleIndex += 3;
	}

	public Mesh CreateMesh(){
		Mesh mesh = new Mesh ();
		mesh.vertices = verticies;
		mesh.triangles = triangles;
		mesh.uv = uvs;
		mesh.RecalculateNormals ();
		return mesh;
	}

}
