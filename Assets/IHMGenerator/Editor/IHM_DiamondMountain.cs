using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


public class IHM_DiamondMountain{
	int borderL,borderR,borderT,borderB;
	float[,] map;
	float white = 2f;
	float grey = 0.0f;
	float black = 0;
	float noiseScale;
	float value;
	float seed;
	int width, height;
	int count;
	int cntrlCount;
	bool diamond = false;

	int octaves = 3; // How many noise layers should applied
	int lacunarity = 2; // Controls increse in freqncy of octaves
	float persistance = 0.5f; //Controlls decrease in amplitude of octaves

	float maxNoiseHeight = 1;
	float minNoiseHeight = -1;

	Texture2D heightMap;
	Color color;
	List<Pixel> toCompute;
	SimplexNoiseGenerator sNoise;

	public float[,] Generate (int[] sectors, int mapSize, string mapName, float seed, float scale, int octaves, int frequency, float amplitude, int gridNum){
		width = mapSize;
		height = mapSize;
		count = 0;
		cntrlCount = 0;
		float white = 2f;
		toCompute = new List<Pixel>();

		this.seed = seed;
		noiseScale = scale;
		this.octaves = octaves;
		lacunarity = frequency;
		persistance = amplitude;

		map = new float[width,height];
		sNoise = new SimplexNoiseGenerator (seed.ToString());

		Debug.Log ("Map-Size: "+mapSize);

		//Fill the map with black
		Debug.Log ("Filling the map with black "+ System.DateTime.Now);
		for(int i = 0; i < height; i++){
			for(int j = 0; j < width; j++){
				map[j,i] = black;

			}
		}

		//Adding start pixel for every choosen sector
		Debug.Log ("Adding starting pixels " + System.DateTime.Now);
		for (int i = 0; i < 4; i++) {
			int sec = 0;
			int x = 0;
			int y = 0;
			int mapEighth = mapSize / (gridNum * 2);
			if(sectors[i] != 99){
				sec = sectors[i];
				x = Eighth (sec) * mapEighth;
				y = Eighth (i) * mapEighth;
				Debug.Log ("sec: "+i+" X: " + x + " Y: " + y);
				map [x, y] = white - 0.01f;
				toCompute.Add (new Pixel (x, y));
			}
		}
			
		Debug.Log ("Start computing "+ System.DateTime.Now);
		Debug.Log ("toCompute: " + toCompute.Count);
		diamondMountain ();

		Debug.Log ("Filling remaining map "+ System.DateTime.Now);
		fillMap ();
        //Fill the remaining Heightmap with noise


		Debug.Log ("Map controll value: "+ map[mapSize/2,mapSize/2]+"  "+System.DateTime.Now);
		Debug.Log ("Max: " + maxNoiseHeight + " Min: " + minNoiseHeight + "  " + System.DateTime.Now);
		Debug.Log ("Normalizing map " + System.DateTime.Now);
		for (int i = 0; i < height; i++) {
			for (int j = 0; j < width; j++) {
				//Normalize Heightmap to range 0 - 1 
				map [j, i] = Mathf.InverseLerp (minNoiseHeight,maxNoiseHeight,map[j,i]);
			}
		}
		Debug.Log ("Map controll value: "+ map[mapSize/2,mapSize/2]+"  "+System.DateTime.Now);
		Debug.Log ("Computing done!" + System.DateTime.Now);
		Debug.Log ("Runs: " + count + " Controll Count: " +cntrlCount);



		return map;
	}

    //Give the right index for the sectors starting point
	int Eighth(int value){
		int eighth = 0;
		switch (value) {
		case 0:
			eighth = 1;
			break;
		case 1:
			eighth = 3;
			break;
		case 2:
			eighth = 5;
			break;
		case 3:
			eighth = 7;
			break;
		case 4:
			eighth = 9;
			break;
		case 5:
			eighth = 11;
			break;
		case 6:
			eighth = 13;
			break;
		case 7:
			eighth = 15;
			break;
		}
		return eighth;
	}
		

	bool diamondMountain(){
		Pixel tempPixel;
		int xt,yt;
		//As long as pixel are to compute
		while(toCompute.Count != 0){

			tempPixel = toCompute[0];
			xt = tempPixel.x;
			yt = tempPixel.y;

			//Check if inside the map
			if (xt < 0) {
				return true;
			} else if (xt > width) {
				return true;
			} 
			if(yt < 0){
				return true;
			}else if(yt > height){
				return true;
			}
			//Debug.Log ("Value: "+map[xt,yt]);
			//Check if specific value
			if(map[xt,yt] < generateNoise(xt,yt)){
				toCompute.RemoveAt(0);
				break;
			}

			setPixel(xt,yt+1); 
			setPixel(xt,yt-1);
			setPixel(xt+1,yt);
			setPixel(xt-1,yt);

			setPixel((xt + (int)Random.Range(-2,2)),((yt + (int)Random.Range(-2,2))));
			setPixel((xt + (int)Random.Range(-2,2)),((yt + (int)Random.Range(-2,2))));
			setPixel((xt + (int)Random.Range(-3,3)),((yt + (int)Random.Range(-3,3))));
			setPixel((xt + (int)Random.Range(-5,5)),((yt + (int)Random.Range(-5,5))));
			setPixel((xt + (int)Random.Range(-7,7)),((yt + (int)Random.Range(-7,7))));

			toCompute.RemoveAt(0);

		}
		diamond = true;
		return true;
	}


	bool fillMap(){
		if (diamond) {
			for (int i = 0; i < height; i++) {
				for (int j = 0; j < width; j++) {
					if (map [j, i] == black) {
						map [j, i] = generateNoise (j, i);
					}
				}
			}
		}

		return true;
	}

	void setPixel(int x, int y){
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
			
		//if(map[x,y] < 1){
		//	return;
		//}
		if (map [x, y] == black) {
			map [x, y] = generateNoiseDiamond(x,y);
			//map[x,y] = white - Mathf.PerlinNoise(((float)(x / width) * noiseScale) + seed, ((float)(y / width) * noiseScale) + seed);
			//map[x,y] = white - sNoise.noise(((x) * noiseScale), ((y) * noiseScale),0.5f);
			toCompute.Add (new Pixel (x, y));
			count++;
		}
	}


	private float generateNoise(int x, int y){
		
		float amp = 1;
		float freq = 1;
		float nVal = 0;


		for(int k = 0; k < octaves; k++){
				float pVal = 0.5f;
				float sampleX = (float)(x * noiseScale) * freq;
				float sampleY = (float)(y * noiseScale) * freq;
				 pVal = Mathf.PerlinNoise (sampleX + seed, sampleY + seed) * 2 - 1;
			
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

    private float generateNoiseDiamond(int x, int y){
        float amp = 1;
        float freq = 1;
        float nVal = 0;


        for (int k = 0; k < octaves; k++)
        {
            float pVal = 0.5f;
			float sampleX = (float)(x * noiseScale) * freq;
			float sampleY = (float)(y * noiseScale) * freq;

                cntrlCount++;
                white -= 0.000005f;
                pVal = (white - Mathf.PerlinNoise(sampleX + seed, sampleY + seed)) * 2 - 1;
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



	private class Pixel{
		public int x,y;

		public Pixel(int x, int y){
			this.x = x;
			this.y = y;
		}
	}


			
}
