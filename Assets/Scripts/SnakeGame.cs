using UnityEngine;
using System.Collections;

public class SnakeGame : MonoBehaviour {

	private static SnakeGame	instance = null;

	private GUIText	displayScore;
	private GUIText displayLives;

	public int maxLives = 3;
	public int gameScore = 0;
	public int gameLives = 3;
	public int scoreMultiplier = 100;

	public float moveDelay = 0.1f;

	public static SnakeGame Instance {
		get {
			if(instance == null) {
				instance = new GameObject("SnakeGame").AddComponent<SnakeGame>();
			}
			return instance;
		}
	}

	public void OnApplicationQuit()
	{
		DestroyInstance();
	}

	public void DestroyInstance()
	{
		instance = null;
	}
	

	public void UpdateScore(int add) {
		gameScore += add * scoreMultiplier;
		displayScore.text = "Score: " + gameScore;
	}

	public void UpdateLives(int add) {
		gameLives += add;
		gameLives = Mathf.Clamp(gameLives, 0, maxLives);
		displayLives.text = "Lives: " + gameLives;
	}

	
	// load new resources
	void LoadResources()
	{
		GUIHelper.CreateGUITexture(new Rect(0,0, Globals.ScreenWidth, Globals.GameFieldY), Color.gray, "ScreenBorder", 0);
		GUIHelper.CreateGUITexture(new Rect(0,0, 96, 96), "snake", 1);

		//GUIHelper.CreateGUITexture(new Rect(Globals.GameFieldX, Globals.GameFieldY, 
		//                                    Globals.GameFieldWidth, Globals.GameFieldHeight), Color.black, "ScreenGameField", 1);

		GreenBoard();
		//GUIHelper.CreateGUITexture("Gfx/grass_01");
	}

	void GreenBoard() 
	{
		// 20x15 tiles
		int htiles = Globals.ScreenWidth / Globals.TileSize;
		int vtiles = Globals.ScreenHeight / Globals.TileSize;
		for(int i=0; i < htiles*(vtiles+1); ++i)
		{ 
			int posY = i / htiles;
			int posX = i % htiles;
			int piX = posX * Globals.TileSize;
			int piY = posY * Globals.TileSize;

			GUIHelper.CreateGUITexture(new Rect( piX, piY, Globals.TileSize, Globals.TileSize), 
			                           "grass_04d", Globals.LayerBackround);
			Debug.Log (string.Format( "grass[{0}][{1},{2}] = {3} x {4}", i, posX, posY, piX, piY));
		}
		//GUIHelper.CreateGUITexture(new Rect(0,0, 32, 32), "grass_04", -99);
	}

	public void Initialize()
	{
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;

		gameScore = 0;
		gameLives = maxLives;
		scoreMultiplier = 100;

		LoadResources();

		displayScore = GUIHelper.CreateGetGUIText(new Vector2(96, 64), "Game Score", "Score", 1);   
		UpdateScore(0);
		displayLives = GUIHelper.CreateGetGUIText(new Vector2(96, 96), "Game Lives", "Lives", 1);
		UpdateLives(0);
	}

}
