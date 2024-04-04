using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

public static class IHM2_TextureGenerator{
	static Texture2D currentTexture;

	public static Texture2D TextureFromColormap(Color[] colormap, int width, int height){
		Texture2D texture = new Texture2D (width, height);
		texture.wrapMode = TextureWrapMode.Clamp;
		texture.SetPixels (colormap);
		texture.Apply ();


		currentTexture = texture;

		return texture;
	}

	public static Texture2D TextureFromHeightmap(float[,] map){
		int width = map.GetLength (0);
		int height = map.GetLength (1);

		Color[] colourMap = new Color[width * height];
		for (int j = 0; j < height; j++) {
			for(int i = 0; i < width; i++){
				colourMap [j * width + i] = Color.Lerp (Color.black, Color.white, map [i, j]);
			}
		}
		return TextureFromColormap(colourMap,width,height);
	}

	public static void SaveTexture(string name){
		System.IO.File.WriteAllBytes ("Assets/IHMGenv2/Textures/"+name+".raw",currentTexture.EncodeToPNG());
		AssetDatabase.CreateAsset(currentTexture, "Assets/IHMGenv2/Textures/"+name+".asset");

	}

}
