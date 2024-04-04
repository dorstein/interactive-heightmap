using UnityEditor;
using UnityEngine;


public class IHM2_Menu {
	

	[MenuItem("Tools/Start Interactive Heightmap")]
	private static void Create(){
		GameObject mapGen = AssetDatabase.LoadAssetAtPath ("Assets/IHMGenV2/Prefab/Map Generator.prefab", typeof(GameObject)) as GameObject;
		PrefabUtility.InstantiatePrefab (mapGen);

	}

}
