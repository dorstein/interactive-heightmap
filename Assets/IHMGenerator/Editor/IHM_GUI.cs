using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public class IHM_GUI : EditorWindow {
	float seed = 1.0f;
	int gridNum = 4;
	//int oldNum = 3;
	int gridCount = 0;
	int n;
	float scale =20f;
	int octaves = 3;
	int frequency = 2; 
	float amplitude = 0.5f; 
	int depth = 120;
	bool autoUpdate = false;
	//int[] selections = { 99, 99, 99, 99, 99, 99, 99, 99 };
	int selectionA = 99;
	int selectionB = 99;
	int selectionC = 99;
	int selectionD = 99;
	int mapSize = 512;
	float[,] map;
	string [] texts = new string[]{" "," "," "," "};
	int[] sectors = new int[4];
	string mapName = "Heightmap";


	IHM_DiamondMountain algo = new IHM_DiamondMountain();
	IHM_TerrainGenerator tGen = new IHM_TerrainGenerator ();
	//Texture2D drawMap = Texture2D.blackTexture;
	public Texture2D heightmap;

	//Add menu item to the Window menu
	[MenuItem("Window/IHM Generator")]

	public static void ShowWindow(){
		//If window does not exist, create one
		EditorWindow.GetWindow<IHM_GUI>("IHM Generator");
		createFolder ();

	}

	void OnGUI(){
		GUILayout.Label ("Interactive Heightmap Generator", EditorStyles.boldLabel);
		//GUI Box which holds the the GUI
		Rect rH = EditorGUILayout.BeginHorizontal ();
			GUILayout.FlexibleSpace ();
		if(autoUpdate){
			EditorGUI.BeginChangeCheck ();
		}
			//Left GUI box which holds the Heightmap
			Rect rV = EditorGUILayout.BeginVertical ();
				//drawPlane = EditorGUILayout.RectField (new Rect(10,50,256,256));
				//EditorGUI.DrawRect (new Rect(10, 30, 256, 256), matCol);

				/*
				if(oldNum != gridNum){
					oldNum = gridNum;
					for(int i = 0; i < gridNum; i++){
						gridCount++;
						selections [i] = GUILayout.SelectionGrid (selections[i], texts, gridNum);

					}
				Debug.Log ("GridCount: "+gridCount);
				}*/
				
				selectionA = GUILayout.SelectionGrid (selectionA,texts,gridNum);
				selectionB = GUILayout.SelectionGrid (selectionB,texts,gridNum);
				selectionC = GUILayout.SelectionGrid (selectionC,texts,gridNum);
				selectionD = GUILayout.SelectionGrid (selectionD,texts,gridNum);

				GUILayout.FlexibleSpace ();
				Rect rH2 = EditorGUILayout.BeginHorizontal();
				if(heightmap){
					Texture2D preview = AssetPreview.GetAssetPreview (heightmap);
					//GUI.DrawTexture (new Rect (rH2.x,rH2.y,100, 100), preview, ScaleMode.ScaleToFit);
					//EditorGUI.DrawPreviewTexture (new Rect (rH2.x, rH2.y, 100, 100), heightmap,ScaleMode.ScaleToFit);
					GUILayout.Label(preview);
					//EditorGUILayout.LabelField (preview);
				}
				EditorGUILayout.EndHorizontal ();
			GUILayout.FlexibleSpace ();
			EditorGUILayout.EndVertical ();
			GUILayout.FlexibleSpace ();
			//Right GUI box which holds the Options
			Rect rV2 = EditorGUILayout.BeginVertical ();
				mapName = EditorGUILayout.TextField ("Heightmap Name", mapName);
				mapSize = EditorGUILayout.IntField ("Map-Size", mapSize);
				//gridNum = EditorGUILayout.IntSlider ("Grid Size", gridNum, 2, 8);
				depth = EditorGUILayout.IntField ("Map-Height",depth);
				//GUILayout.FlexibleSpace ();
		        seed = EditorGUILayout.FloatField("Seed", seed);
		        //EditorGUILayout.LabelField ("Map Scale: 0.01f - 0.001f");
				scale = (float)EditorGUILayout.Slider ("Map-Scale", scale,0.03f,0.001f);
				octaves = EditorGUILayout.IntField ("Octaves (Noise Layers)", octaves);
				frequency = EditorGUILayout.IntField ("Octaves Frequency", frequency);
				amplitude = (float)EditorGUILayout.Slider ("Octaves Amplitude", amplitude,0,1);

				autoUpdate = EditorGUILayout.Toggle ("Auto Update", autoUpdate);
					//Button box	
					Rect rB = EditorGUILayout.BeginHorizontal ();
						Rect rbV = EditorGUILayout.BeginVertical ();
							if(GUI.Button(new Rect(rbV.x,rbV.y,80,50),"Generate")){
								Debug.Log ("A: "+selectionA+" B: "+selectionB+" C: "+selectionC+" D: "+selectionD);
								sectors [0] = selectionA;
								sectors [1] = selectionB;
								sectors [2] = selectionC;
								sectors [3] = selectionD;
								map = algo.Generate (sectors, mapSize, mapName, seed, scale, octaves, frequency, amplitude,gridNum);
								Debug.Log ("Map controll value: "+ map[mapSize/2,mapSize/2]);
								createTexture (map);
						}
					EditorGUILayout.EndVertical ();
					//GUILayout.FlexibleSpace ();
					Rect rbV2 = EditorGUILayout.BeginVertical ();
						if(GUI.Button(new Rect(rbV2.x,rbV2.y,80,50),"Terrain")){
							if (map != null) {
								Debug.Log ("Generate Terrain!");
								tGen.generateTerrain (map, depth, mapName);
							}
						}
					EditorGUILayout.EndVertical ();
					EditorGUILayout.EndHorizontal ();
			EditorGUILayout.EndVertical ();
			GUILayout.FlexibleSpace ();
		EditorGUILayout.EndHorizontal ();

		if (autoUpdate) {
			if (EditorGUI.EndChangeCheck ()) {
				if (GUI.changed) {
					sectors [0] = selectionA;
					sectors [1] = selectionB;
					sectors [2] = selectionC;
					sectors [3] = selectionD;
					map = algo.Generate (sectors, mapSize, mapName, seed, scale, octaves, frequency, amplitude, gridNum);
					createTexture (map);
					tGen.updateTerrain (map, depth);
				}
			}
		}
	}

	void createGrid(int N){
		//Check if the grid num has changed
		if (n != N)
			n = N;

	}

	static void createFolder(){
		if (!AssetDatabase.IsValidFolder ("Asset/Terrain")) {
			string guid = AssetDatabase.CreateFolder ("Asset", "Terrain");
			string newFolderPath = AssetDatabase.GUIDToAssetPath (guid);
			Debug.Log ("New Folder Created");
		} else
			Debug.Log ("Folder already exists");

	}

	void createTexture(float[,] mapValues){
		Debug.Log ("Creating Texture");
		float white = 1f;
		float value;
		Color color;
		int size = mapValues.GetLength (0);

		heightmap = new Texture2D (size, size);

		for(int i = 0; i < size; i++){
			for(int j = 0; j < size; j++){
				value = map [j, i];
				color = Color.Lerp (Color.black, Color.white, value);
				heightmap.SetPixel (j, i, color);
			}
		}
		heightmap.Apply ();

		System.IO.File.WriteAllBytes ("Assets/Terrain/"+mapName+".raw",heightmap.EncodeToPNG ());
		AssetDatabase.CreateAsset(heightmap, "Assets/Terrain/"+mapName+".asset");

	}

	void OnValidate(){

	}
}
