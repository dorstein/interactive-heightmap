using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(IHM2_MapGenerator))]
public class IHM2_MapGeneratorEditor : Editor {

	public override void OnInspectorGUI(){
		IHM2_MapGenerator mapGen = (IHM2_MapGenerator)target;


		if (DrawDefaultInspector ()) {
			if (mapGen.autoUpdate) {
				mapGen.GenerateMap ();
				mapGen.UpdateTerrain ();
			}

		}


		if (GUILayout.Button ("Generate")) {
			mapGen.GenerateMap ();
		}

		if (GUILayout.Button ("Terrain")) {
			mapGen.GenerateTerrain ();
		}

		if (GUILayout.Button ("Testcase")) {
			mapGen.runTestCase ();
		}
	}
}
