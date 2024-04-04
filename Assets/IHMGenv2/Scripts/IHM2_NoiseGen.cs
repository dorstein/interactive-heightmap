using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class IHM2_NoiseGen  {

	//Current White value.
	static public float white{
		get;
		set;
	}
	//Current grey value
	static public float grey{
		get;
		set;
	}

	//Controlls decrease in amplitude of octaves
	static public float persistance {
		set;
		get;
	}
	// How many noise layers should applied
	static public int octaves {
		set;
		get;
	}
	// Controls increse in freqncy of octaves
	static public int lacunarity {
		set;
		get;
	}
	//Scale of the noise Value	
	static public float noiseScale {
		set;
		get;
	}
	//Max noise height later used for normalization
	static public float maxNoiseHeight {
		set;
		get;
	}
	//Min noise height later user for normalization
	static public float minNoiseHeight {
		set;
		get;
	}
	static public Vector2[] octaveOffsets{
		set;
		get;
	}

	/**
	 * Noise Value method which returns a noise value calculated
	 * with different noise layers for more details
	 */
	public static float noiseValue(int x, int y){
		float amp = 1;
		float freq = 1;
		float nVal = 0;


		for(int k = 0; k < octaves; k++){
			float pVal = 0.5f;
			float sampleX = (float)(x / noiseScale) * freq + octaveOffsets[k].x;
			float sampleY = (float)(y / noiseScale) * freq + octaveOffsets[k].y;
			pVal = (grey - Mathf.PerlinNoise (sampleX, sampleY)) * 2 - 1;

			nVal += pVal * amp;

			amp *= persistance;
			freq *= lacunarity; 
		}

		if (nVal > maxNoiseHeight) {
			maxNoiseHeight = nVal;
		} else if (nVal < minNoiseHeight) {
			minNoiseHeight = nVal;
		}

		return nVal;
	}

	 /** 
	 * Noise Values for the Diamond-Mountain algorithm which returns 
	 * noise values calculated with different layers for more details also 
	 * brighter values for the Diamon-Mountain
	 */
	public static float noiseValueDiamond(int x, int y){
		float amp = 1;
		float freq = 1;
		float nVal = 0;


		for (int k = 0; k < octaves; k++)
		{
			float pVal = 0.5f;
			float sampleX = x / noiseScale * freq + octaveOffsets[k].x;
			float sampleY = y / noiseScale * freq + octaveOffsets[k].y;

			white -= 0.000005f;
			pVal = (white - Mathf.PerlinNoise(sampleX, sampleY)) * 2 - 1;
			nVal += pVal * amp;

			amp *= persistance;
			freq *= lacunarity;
		}

		if (nVal > maxNoiseHeight)
		{
			maxNoiseHeight = nVal;
		}
		else if (nVal < minNoiseHeight)
		{
			minNoiseHeight = nVal;
		}

		return nVal;
	}
		
		
}
