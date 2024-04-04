using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IHM2_MeshGenerator{


	public static IHM2_MeshData GenerateTerrainMesh(float[,] map){
		int width = map.GetLength (0);
		int height = map.GetLength (1);
		float topLeftX = (width - 1) / -2f;
		float topLeftZ = (height - 1) / 2f;

		IHM2_MeshData meshData = new IHM2_MeshData (width, height);
		int vertexIndex = 0;

		for (int j = 0; j < height; j++) {
			for (int i = 0; i < width; i++) {
				meshData.verticies [vertexIndex] = new Vector3 (topLeftX + i, map [i, j], topLeftZ - j);
				meshData.uvs [vertexIndex] = new Vector2 (i / (float)width, j / (float)height);
				if (i < width - 1 && j < height - 1) {
					meshData.AddTriangle (vertexIndex, vertexIndex + width + 1, vertexIndex + width);
					meshData.AddTriangle (vertexIndex + width + 1, vertexIndex, vertexIndex + 1);
				}
				vertexIndex++;
			}
		}

		return meshData;
	}

}
	