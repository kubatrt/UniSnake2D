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

	public void Initialize()
	{
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;

		gameScore = 0;
		gameLives = maxLives;
		scoreMultiplier = 100;

		GUIHelper.CreateGUITexture(new Rect(0,0, Globals.ScreenWidth, Globals.ScreenHeight), Color.gray, "ScreenBorder", 0);
		GUIHelper.CreateGUITexture(new Rect(Globals.GameFieldX, Globals.GameFieldY, Globals.GameFieldWidth, Globals.GameFieldHeight), Color.black, "ScreenGameField", 1);

		displayScore = GUIHelper.CreateGetGUIText(new Vector2(16, 32), "Game Score", "Score", 1);   
		UpdateScore(0);
		displayLives = GUIHelper.CreateGetGUIText(new Vector2(16, 64), "Game Lives", "Lives", 1);
		UpdateLives(0);
	}

}
