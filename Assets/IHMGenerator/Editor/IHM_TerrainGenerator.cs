using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class IHM_TerrainGenerator {
	Terrain terrainComponent;
	GameObject terrain;
	TerrainData terrainData;
	int width, height, depth;

	public void generateTerrain(float[,] map, int depth, string name){
		width = map.GetLength (0);
		height = map.GetLength (0);
		this.depth = depth;
		terrain = new GameObject (name);
		terrain.AddComponent<Terrain> ();
		terrain.AddComponent<TerrainCollider> ();
		terrainComponent = terrain.GetComponent<Terrain> ();
		terrainComponent.terrainData = new TerrainData ();


		terrainComponent.terrainData = populateTerrain (terrainComponent.terrainData, map);

		//AssetDatabase.CreateAsset (terrain, "Assets/Terrain/"+name+".asset");

	}

	public void updateTerrain(float[,] map, int depth){
		if (terrain != null) {
			this.depth = depth;
			terrainComponent.terrainData = populateTerrain (terrainComponent.terrainData, map);
		}
	}

	TerrainData populateTerrain(TerrainData td,float[,] heights){

		td.heightmapResolution = width + 1;
		td.size = new Vector3 (width, depth, height);

		td.SetHeights (0, 0, heights);

		return td;
	}
}
