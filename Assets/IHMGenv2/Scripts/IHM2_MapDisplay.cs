using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IHM2_MapDisplay : MonoBehaviour {

	public Renderer textureRender;
	public MeshFilter meshFilter;
	public MeshRenderer meshRenderer;
	public string mapName = "Heightmap";


	/**
	 * Draws the generated Heightmap in the scene view 
	 */ 
	public void DrawTexture(Texture2D texture){
		textureRender.sharedMaterial.mainTexture = texture;
		textureRender.transform.localScale = new Vector3 ((texture.width/10), 1, (texture.height/10));
	}

	public void DrawMesh(IHM2_MeshData meshData, Texture2D texture){
		meshFilter.sharedMesh = meshData.CreateMesh ();
		meshRenderer.sharedMaterial.mainTexture = texture;
	}
		
	public void SaveTexture(){
		IHM2_TextureGenerator.SaveTexture (mapName);
	}

}
