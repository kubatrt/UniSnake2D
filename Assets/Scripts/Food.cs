using UnityEngine;
using System.Collections;

public class Food : Singleton<Food> 
{
	// ---------------------------------------------------------------------------------------------------
	public Rect foodPos = new Rect(0,0, Globals.TileSize, Globals.TileSize);
	
	private static int[] 	initXPos; 
	private static int[] 	initYPos; 
	private Texture2D 		foodTexture;
	private AudioClip		foodPickup;
	
	private void GenerateInitPositions()
	{
		int tilesHorizontal = Globals.GameFieldWidth / Globals.TileSize;
		int tilesVertical = Globals.GameFieldHeight / Globals.TileSize;
 
		initXPos = new int[tilesHorizontal];
		for(int x=0; x < initXPos.Length; ++x)
			initXPos[x] = Globals.TileSize * x;

		initYPos = new int[tilesVertical];
		for(int y=0; y < initYPos.Length; ++y)
			initYPos[y] = Globals.TileSize * y;
	}

	// ---------------------------------------------------------------------------------------------------
	public void UpdateFood()
	{
		if(audio) audio.Play();

		int randX = Random.Range(0, initXPos.Length);
		int randY = Random.Range(0, initYPos.Length);

		foodPos = new Rect(initXPos[randX], initYPos[randY], Globals.TileSize, Globals.TileSize);
		Debug.Log ("UpdateFood :" + foodPos);
	}

	public void OnGUI()
	{
		// drawing
		if(Food.Instance != null)
		{
			GUI.DrawTexture(foodPos, foodTexture);
		}
	}

	public void Initialize()
	{
		if(gameObject.GetComponent<AudioSource>())
		{
			foodPickup = Resources.Load ("Sounds/FoodPickup") as AudioClip;

			gameObject.AddComponent<AudioSource>();
			audio.playOnAwake = false;
			audio.loop = false;
			audio.clip = foodPickup;
		}

		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;

		foodTexture = TextureHelper.CreateTexture("Gfx/apple_01");

		GenerateInitPositions();

		int randX = Random.Range(0, initXPos.Length);
		int randY = Random.Range(0, initYPos.Length);

		foodPos = new Rect(initXPos[randX], initYPos[randY], Globals.TileSize, Globals.TileSize);
		Debug.Log ("Food initialized!:" + foodPos);
	}
}
