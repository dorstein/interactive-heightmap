using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IHM2_DiamondMountain{
	float white = 1f;
	float[,] heightmap;
	float black = -2f;
	List<Vector2Int> toCompute;
	int width, height;
	int count;
	public int iterations;

	/**
	 * Diamond Mountain algorithm which generates Mountains on the given
	 * locations.
	 */
	public float [,] DiamondMountain(float[,] map, Vector2Int[] startingPoints){
		//Debug.Log ("Diamond Mountain!");
		width = map.GetLength (0);
		height = map.GetLength (1);
		heightmap = map;
		count = 0;
		toCompute = startingPoints.OfType<Vector2Int>().ToList();
		InstatiateMap ();
		Vector2Int tempPixel;
		//Debug.Log ("List size: "+toCompute.Count);
		int xt,yt;
		//As long as pixel are to compute
		while(toCompute.Count != 0){

			tempPixel = toCompute[0];
			xt = tempPixel.x;
			yt = tempPixel.y;

			//Check if inside the map
			if (xt < 0) {
				toCompute.RemoveAt(0);
				continue ;
			} else if (xt > width) {
				toCompute.RemoveAt(0);
				continue ;
			}
			if(yt < 0){
				toCompute.RemoveAt(0);
				continue ;
			}else if(yt > height){
				toCompute.RemoveAt(0);
				continue ;
			}
			//Check if specific value
			if(map[xt,yt] < IHM2_NoiseGen.noiseValue(xt,yt)){
				toCompute.RemoveAt(0);
				continue;
			}
			//SetPixel around the given point
			for (int i = 1; i <= iterations; i++) {
				SetPixel((xt + (int)Random.Range(-i,i)),((yt + (int)Random.Range(-i,i))));
			}

			toCompute.RemoveAt(0);

		}
		//Debug.Log ("Count: " + count);
		return heightmap;
	}


	/**
	 * Instatiate the map, set the starting points to white.
	 */
	void InstatiateMap(){
		Vector2Int temp;
		for (int i = 0; i < toCompute.Count; i++) {
			temp = toCompute [i];
			heightmap [temp.x, temp.y] = white;
		}

	}

	/**
	 * Set pixel (noise value) for the given point
	 */
	void SetPixel(int x, int y){
		//Debug.Log ("setPixel(X: "+x+",Y: "+y+")");
		if (x < 0) {
			return;
		} else if (x >= width) {
			return;
		}
		if(y < 0){
			return;
		}else if(y >= height){
			return;
		}

		if (heightmap [x, y] == black) {
			heightmap [x, y] = IHM2_NoiseGen.noiseValueDiamond(x,y);
			//map[x,y] = white - Mathf.PerlinNoise(((float)(x / width) * noiseScale) + seed, ((float)(y / width) * noiseScale) + seed);
			//map[x,y] = white - sNoise.noise(((x) * noiseScale), ((y) * noiseScale),0.5f);
			toCompute.Add (new Vector2Int (x, y));
			count++;
		}
	}

}
