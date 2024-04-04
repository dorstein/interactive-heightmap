using UnityEngine;
using UnityEditor;


[CustomEditor(typeof(IHM2_MapDisplay))]
public class IHM2_MapDisplayEditor : Editor {


	public override void OnInspectorGUI(){
		IHM2_MapDisplay mapDisplay = (IHM2_MapDisplay)target;
		DrawDefaultInspector ();



		if (GUILayout.Button ("Save Texture")) {
			mapDisplay.SaveTexture ();
		}

	}
}
