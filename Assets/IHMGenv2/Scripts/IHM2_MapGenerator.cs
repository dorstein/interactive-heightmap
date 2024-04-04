using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Diagnostics;
using System.IO;

public class IHM2_MapGenerator : MonoBehaviour {

	//Public Attributes
	public enum DrawMode{Heightmap,Colormap,Mesh};
	public DrawMode drawMode;
	public int mapWidth = 100;
	public int mapHeight = 100;
	public float noiseScale = 20f;
	public int Seed = 0;
	[Tooltip("Depth of the generated terrain")]
	public int mapDepth = 80;
	public int Iteration = 10;
	[Tooltip("")]
	public int Octaves = 3;
	[Tooltip("")]
	public int Frequency = 2; //Lacrunarity
	[Range(0,1)]
	public float Amplitude = 0.5f;  //Persistance
	public Vector2 Offset;
	[Tooltip("Here you can add Mountains to apear. X: 0 Y: 0 is the top left corner")]
	public Vector2Int[] Mountains;

	public TerrainType[] regions;

	public bool autoUpdate = false;



	//Private Attributes
	private float[,] map;
	private float black = -2f;
	private Vector2[] octavesOffset;
	private Color[] colorMap;

	/**
	 * Generate noise map with the given parameters.
	 */
	public void GenerateMap(){
		map = new float[mapWidth, mapHeight];
		IHM2_DiamondMountain diamond = new IHM2_DiamondMountain ();
		diamond.iterations = Iteration;
		CreateOffsets ();
		SetNoiseAttributes ();
		FillMapWithBlack ();
		map = diamond.DiamondMountain (map,Mountains);
		FillMap ();
		NormalizeMap ();

		IHM2_MapDisplay display = FindObjectOfType<IHM2_MapDisplay> ();
		if (drawMode == DrawMode.Heightmap) {
			if (map != null) {
				display.DrawTexture (IHM2_TextureGenerator.TextureFromHeightmap (map));
			}
		} else if (drawMode == DrawMode.Colormap) {
			if (colorMap != null) {
				display.DrawTexture (IHM2_TextureGenerator.TextureFromColormap (colorMap, mapWidth, mapHeight));
			}
		} else if (drawMode == DrawMode.Mesh) {
			if (map != null) {
				display.DrawMesh (IHM2_MeshGenerator.GenerateTerrainMesh (map), IHM2_TextureGenerator.TextureFromColormap (colorMap, mapWidth, mapHeight));
			}
		}
	}

	/**
	 * Runs a test Case
	 */
	public void runTestCase(){
		string path = "Assets/Test.txt";
		StreamWriter writer = new StreamWriter(path, true);
		CreateOffsets ();
		SetNoiseAttributes ();



		for (int i = 128; i <= 8192; i *= 2) {
			writer.WriteLine ("Size: " +i+ "Mountains: " +Mountains.GetLength(0)+":");
			for (int j = 0; j <= 9; j++) {

				map = new float[i, i];
				IHM2_DiamondMountain diamond = new IHM2_DiamondMountain ();
				Stopwatch sw = new Stopwatch ();

				sw.Start ();
				diamond.DiamondMountain (map, Mountains);
				sw.Stop ();

				writer.WriteLine("Time: "+sw.Elapsed+" ");
			}

		}
		writer.Close ();
		UnityEngine.Debug.Log ("Done");
	}

	public void GenerateTerrain(){
		if(map != null){
			IHM2_TerrainGenerator.generateTerrain (map,mapDepth,"Terrain");
		}
	}

	public void UpdateTerrain(){
		if (map != null) {
			IHM2_TerrainGenerator.updateTerrain (map,mapDepth);
		}

	}

	/**
	 * Create Map offsets with given seed.
	 */
	void CreateOffsets(){
		System.Random prng = new System.Random (Seed);
		octavesOffset = new Vector2[Octaves];
		for(int i = 0; i < Octaves; i++){
			float offsetX = prng.Next (-100000,100000) + Offset.x;
			float offsetY = prng.Next (-100000,100000) + Offset.y;
			octavesOffset [i] = new Vector2 (offsetX, offsetY);
		}
	}
	/**
	 *Set the Attributes for the NoiseGenerator
	 */
	void SetNoiseAttributes(){
		IHM2_NoiseGen.white = 1f;
		IHM2_NoiseGen.grey = 0.8f;
		IHM2_NoiseGen.maxNoiseHeight = 1f;
		IHM2_NoiseGen.minNoiseHeight = 0f;
		IHM2_NoiseGen.octaves = Octaves;
		IHM2_NoiseGen.noiseScale = noiseScale;
		IHM2_NoiseGen.lacunarity = Frequency;
		IHM2_NoiseGen.persistance = Amplitude;
		IHM2_NoiseGen.octaveOffsets = octavesOffset;

	}

	/**
	 * Fill the map with Black
	 */
	void FillMapWithBlack(){
		for (int i = 0; i < mapHeight; i++) {
			for (int j = 0; j < mapWidth; j++) {
				map [j, i] = black;
			}
		}
	}

	/**
	 * Create a colormap according to the user input.
	 */
	void CreateColourMap(){
		colorMap = new Color[mapWidth * mapHeight];
		for (int i = 0; i < mapHeight; i++) {
			for (int j = 0; j < mapWidth; j++) {
				float currentHeight = map [j, i];
				for (int k = 0; k < regions.Length; k++) {
					if (currentHeight <= regions [k].height) {
						colorMap [i * mapWidth + j] = regions [i].color;
						break;
					}
				}
			}
		}
	}

	/**
	 * Fill the remaining map with Noise values
	 */
	void FillMap(){
		for (int i = 0; i < mapHeight; i++) {
			for (int j = 0; j < mapWidth; j++) {
				if (map [j, i] == black) {
					map [j, i] = IHM2_NoiseGen.noiseValue(j, i);
				}
			}
		}
	}

	/**
	 * Normalize Heightmap to range 0 - 1
	 */
	void NormalizeMap(){
		for (int i = 0; i < mapHeight; i++) {
			for (int j = 0; j < mapWidth; j++) {
				map [j, i] = Mathf.InverseLerp (IHM2_NoiseGen.minNoiseHeight,IHM2_NoiseGen.maxNoiseHeight,map[j,i]);
			}
		}
	}

	/**
	 * Validate the user input
	 */
	void OnValidate(){
		if (mapWidth < 1) {
			mapWidth = 1;
		}
		if (mapHeight < 1) {
			mapHeight = 1;
		}
		if (mapDepth < 1) {
			mapDepth = 1;
		}
		if (Frequency < 1) {
			Frequency = 1;
		}
		if (Octaves < 0) {
			Octaves = 0;
		}
		if (noiseScale == 0) {
			noiseScale = 1;
		}
	}

	//Struct for the Regions
	[System.Serializable]
	public struct TerrainType{
		public string Name;
		public float height;
		public Color color;
	}

}
