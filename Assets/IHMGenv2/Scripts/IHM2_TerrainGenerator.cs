using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IHM2_TerrainGenerator {
	static Terrain terrainComponent{ set; get; }
	static GameObject terrain{ set; get;}
	static TerrainData terrainData{ set; get;}
	static int width{ set; get; }
	static int height{ set; get; }
	static int depth{ set; get; }

	public static void generateTerrain(float[,] map, int deep, string name){
		width = map.GetLength (0);
		height = map.GetLength (1);
		depth = deep;
		terrain = new GameObject (name);
		terrain.AddComponent<Terrain> ();
		terrain.AddComponent<TerrainCollider> ();
		terrainComponent = terrain.GetComponent<Terrain> ();
		terrainComponent.terrainData = new TerrainData ();
	

		terrainComponent.terrainData = populateTerrain (terrainComponent.terrainData, map);

		//AssetDatabase.CreateAsset (terrain, "Assets/Terrain/"+name+".asset");

	}

	public static void updateTerrain(float[,] map, int deep){
		if (terrain != null) {
			depth = deep;
			terrainComponent.terrainData = populateTerrain (terrainComponent.terrainData, map);
		}
	}

	static TerrainData populateTerrain(TerrainData td,float[,] heights){

		td.heightmapResolution = width + 1;
		td.size = new Vector3 (width, depth, height);

		td.SetHeights (0, 0, heights);

		return td;
	}
}
