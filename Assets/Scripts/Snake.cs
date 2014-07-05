using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class Snake : Singleton<Snake> 
{
	private List<Rect>		snakePos = new List<Rect>();
	private List<Texture2D> snakeIcon = new List<Texture2D>();
	private int 			snakeLength = 2;
	private Direction		currentDirection = Direction.NONE;
	private bool 			isAlive = false;

	private AudioClip move1;
	private AudioClip move2;
	private AudioClip death;

	public Rect HeadPos { get { return snakePos[0]; } }
	public Rect TilePos { get { return snakePos[0]; } }
	public bool IsAlive	{ get { return isAlive; } }

	public enum Direction {
		UP,
		DOWN,
		LEFT,
		RIGHT,
		NONE
	}

	private readonly Color headColor = Color.yellow;
	private readonly Color middleColor = Color.green;
	private readonly Color tileColor = Color.green;


	// ---------------------------------------------------------------------------------------------------
	public void Initialize()
	{
		snakePos.Clear();
		snakeIcon.Clear();
		snakeLength = 2;

		// add our AudioSource component
		if (!gameObject.GetComponent<AudioSource>())
		{
			move1 = Resources.Load("Sounds/Move1Blip") as AudioClip;
			move2 = Resources.Load("Sounds/Move2Blip") as AudioClip;
			death = Resources.Load("Sounds/Death") as AudioClip;

			if(!move1 || !move2 || !death) {
				//Debug.Log("Failed to load audio resources!");
			}
			else {
				gameObject.AddComponent<AudioSource>();
				audio.playOnAwake = false;
				audio.loop = false;
				audio.clip = move1;
			}
		}
		// make sure our localScale is correct for a GUItexture
		transform.position = Vector3.zero;
		transform.rotation = Quaternion.identity;
		transform.localScale = Vector3.one;

		// create textures
		snakeIcon.Add(TextureHelper.CreateTexture(Globals.TileSize, Globals.TileSize, headColor));
		snakeIcon.Add(TextureHelper.CreateTexture(Globals.TileSize, Globals.TileSize, middleColor));

		// define our snake head and tail GUI Rect
		float ch = (Globals.GameFieldWidth / Globals.TileSize) / 2f * Globals.TileSize + (Globals.TileSize/2f); // 20
		float cv = (Globals.GameFieldHeight / Globals.TileSize) / 2f * Globals.TileSize + (Globals.TileSize/2f); // 20

		snakePos.Add(new Rect(ch, cv , Globals.TileSize, Globals.TileSize));
		snakePos.Add(new Rect(ch, cv + Globals.TileSize, Globals.TileSize, Globals.TileSize));
	}


	void Start () 
	{
		// start moving behaviour
		StartCoroutine(UpdateSnake());
	}

	void HandleInput()
	{
		if (InputHelper.GetStandardMoveUpDirection())
		{
			currentDirection = Direction.UP;
		}
		else if (InputHelper.GetStandardMoveLeftDirection())
		{
			currentDirection = Direction.LEFT;
		}
		else if (InputHelper.GetStandardMoveDownDirection())
		{
			currentDirection = Direction.DOWN;
		}
		else if (InputHelper.GetStandardMoveRightDirection())
		{
			currentDirection = Direction.RIGHT;
		}
	}

	IEnumerator UpdateSnake()
	{
		while(true)
		{
			HandleInput();

			if(currentDirection != Direction.NONE)
			{
				yield return StartCoroutine(MoveSnake(currentDirection));
			}
			if(SnakeCollidedWithSelf() == true)
			{
				break;
			}

			yield return new WaitForSeconds(SnakeGame.Instance.moveDelay);
		}

		// Death
		if(audio) {
			audio.clip = death;
			audio.Play();
		}
		yield return StartCoroutine(ScreenHelper.FlashScreen(6, 0.1f, new Color(1f,0,0,0.5f)));

		SnakeGame.Instance.UpdateLives(-1);
		if(SnakeGame.Instance.gameLives == 0) {
			GameOver();
		}
		else {
			Initialize();
			Start ();
		}
	}

	IEnumerator GameOver()
	{
		Debug.Log("GameOver, reload level...");
		yield return new WaitForSeconds(1f);
		Application.LoadLevel(Application.loadedLevelName);
	}

	void BuildSnakeSegment(Rect rectPos)
	{
		snakeIcon.Add (TextureHelper.CreateTexture(Globals.TileSize, Globals.TileSize, Color.green));
		snakePos.Add(rectPos);
	}
	
	bool SnakeCollidedWithSelf()
	{
		bool didCollide = false;
		
		if(snakePos.Count <= 4)
			return false;
		
		for(int i=0; i < snakePos.Count; i++)
		{
			if(i > 0) 
			{
				if(snakePos[0].x == snakePos[snakePos.Count - i].x && 
				   snakePos[0].y == snakePos[snakePos.Count - i].y) 
				{
					didCollide = true;
					break;
				}
			}
		}
		
		return didCollide;
	}
	
	void OnGUI()
	{
		//Debug.Log ("OnGUI.snakeLength " + snakeLength + "snakePos " + snakePos.Count + "snakeIcon " + snakeIcon.Count);
		for (int i = 0; i < snakeLength; i++)
		{
			GUI.DrawTexture(snakePos[i], snakeIcon[i]);
		}
	}

	// ---------------------------------------------------------------------------------------------------
	#region Movement

	// MoveSnake() Moves the snake texture (pixel movement / update snake texture Rect)
	public IEnumerator MoveSnake(Direction moveDirection)
	{
		// define a temp List of Rects to our current snakes List of Rects
		List<Rect> tempRects = new List<Rect>();
		Rect segmentRect = new Rect(0,0,0,0);

		// initialize
		for (int i = 0; i < snakePos.Count; i++)
		{
			tempRects.Add(snakePos[i]);
		}

		switch(moveDirection)
		{
		case Direction.UP:
			if (snakePos[0].y > 0)
			{
				// we can move up, first move head
				snakePos[0] = new Rect(snakePos[0].x, snakePos[0].y - Globals.TileSize, snakePos[0].width, snakePos[0].height);
				// now update the rest of our body
				UpdateMovePosition(tempRects);
				// check for food
				if (CheckForFood() == true)
				{
					// check for valid build segment position and add a segment
					// create a temporary check position (this one is below the last segment in snakePos[])
					segmentRect = CheckForValidDownPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						snakeLength++;
						// decrement our moveDelay
						SnakeGame.Instance.moveDelay = Mathf.Max(0.05f, SnakeGame.Instance.moveDelay - 0.01f);
						// give control back to our calling method
						yield break;
					}
					// create a temporary check position (this one is to the left the last segment in snakePos[]) 
					segmentRect = CheckForValidLeftPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						snakeLength++;
						// decrement our moveDelay
						SnakeGame.Instance.moveDelay = Mathf.Max(0.05f, SnakeGame.Instance.moveDelay - 0.01f);
						// give control back to our calling method
						yield break;
					}
					// create a temporary check position (this one is to the right the lastsegment in snakePos[])
					segmentRect = CheckForValidRightPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						snakeLength++;
						// decrement our moveDelay
						SnakeGame.Instance.moveDelay = Mathf.Max(0.05f, SnakeGame.Instance.moveDelay - 0.01f);
					}
					// no need to check Up, because we are pressing the Up key, we do not wanta segment above us
				}
				if(audio)  {
					audio.clip = (audio.clip == move1) ? move2 : move1;
					audio.Play();
				}
			}
			break;
		case Direction.LEFT:
			if (snakePos[0].x > Globals.GameFieldX)
			{
				// we can move left
				snakePos[0] = new Rect(snakePos[0].x - Globals.TileSize, snakePos[0].y, 
				                       snakePos[0].width, snakePos[0].height);
				// now update the rest of our body
				UpdateMovePosition(tempRects);
				// check for food
				if (CheckForFood() == true)
				{
					// check for valid build segment position and add a segment
					// create a temporary check position (this one is to the right the last segment in snakePos[])
					segmentRect = CheckForValidRightPosition();
					if (segmentRect.x != Globals.GameFieldX)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						// increment our snake length
						snakeLength++;
						// decrement our moveDelay
						SnakeGame.Instance.moveDelay = Mathf.Max(0.05f, SnakeGame.Instance.moveDelay - 0.01f);
						// give control back to our calling method
						yield break;
					}
					// create a temporary check position (this one is above the last segment in snakePos[])
					segmentRect = CheckForValidUpPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						snakeLength++;
						SnakeGame.Instance.moveDelay = Mathf.Max(0.05f, SnakeGame.Instance.moveDelay - 0.01f);
						// give control back to our calling method
						yield break;
					}
					// create a temporary check position (this one is below the last segment in snakePos[])
					segmentRect = CheckForValidDownPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						snakeLength++;
						SnakeGame.Instance.moveDelay = Mathf.Max(0.05f, SnakeGame.Instance.moveDelay - 0.01f);
						// give control back to our calling method
						yield break;
					}
					// no need to check Left, because we are pressing the Left key, we do not want a segment ahead of us
				}
				if(audio)  {
					audio.clip = (audio.clip == move1) ? move2 : move1;
					audio.Play();
				}
			}
			break;
		case Direction.DOWN:
			if (snakePos[0].y < Globals.GameFieldHeight - Globals.TileSize)
			{
				// we can move down
				snakePos[0] = new Rect(snakePos[0].x, snakePos[0].y + Globals.TileSize, snakePos[0].width,
				                       snakePos[0].height);
				// now update the rest of our body
				UpdateMovePosition(tempRects);
				// check for food
				if (CheckForFood() == true)
				{
					// check for valid build segment position and add a segment
					// create a temporary check position (this one is above the last segment insnakePos[])
						segmentRect = CheckForValidUpPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						snakeLength++;
						SnakeGame.Instance.moveDelay = Mathf.Max(0.05f, SnakeGame.Instance.moveDelay - 0.01f);
						// give control back to our calling method
						yield break;
					}
					// create a temporary check position (this one is to the left the lastsegment in snakePos[])
					segmentRect = CheckForValidLeftPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						// increment our snake length
						snakeLength++;
						SnakeGame.Instance.moveDelay = Mathf.Max(0.05f, SnakeGame.Instance.moveDelay - 0.01f);
						// give control back to our calling method
						yield break;
					}
					// create a temporary check position (this one is to the right the lastsegment in snakePos[])
					segmentRect = CheckForValidRightPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						snakeLength++;
						SnakeGame.Instance.moveDelay = Mathf.Max(0.05f, SnakeGame.Instance.moveDelay - 0.01f);
					}
					// no need to check Down, because we are pressing the Down key, we do notwant a segment below us
				}
				// toggle the audio clip and play
				if(audio)  {
					audio.clip = (audio.clip == move1) ? move2 : move1;
					audio.Play();
				}
			}
			break;
		case Direction.RIGHT:
			if (snakePos[0].x < Globals.ScreenWidth - Globals.TileSize)
			{
				// we can move right
				snakePos[0] = new Rect(snakePos[0].x + Globals.TileSize, snakePos[0].y, snakePos[0].width,
				                       snakePos[0].height);
				// now update the rest of our body
				UpdateMovePosition(tempRects);
				// check for food
				if (CheckForFood() == true)
				{
					// check for valid build segment position and add a segment
					// create a temporary check position (this one is left of the last segment in snakePos[])
					segmentRect = CheckForValidLeftPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						snakeLength++;
						SnakeGame.Instance.moveDelay = Mathf.Max(0.05f, SnakeGame.Instance.moveDelay - 0.01f);
						// give control back to our calling method
						yield break;
					}
					// create a temporary check position (this one is to the left the last segment in snakePos[])
					segmentRect = CheckForValidUpPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						snakeLength++;
						SnakeGame.Instance.moveDelay = Mathf.Max(0.05f, SnakeGame.Instance.moveDelay - 0.01f);
						// give control back to our calling method
						yield break;
					}
					// create a temporary check position (this one is below the last segment in snakePos[])
						segmentRect = CheckForValidDownPosition();
					if (segmentRect.x != 0)
					{
						// we build another segment passing the Rect as an argument
						BuildSnakeSegment(segmentRect);
						snakeLength++;
						SnakeGame.Instance.moveDelay = Mathf.Max(0.05f, SnakeGame.Instance.moveDelay - 0.01f);
					}
					// no need to check Right, because we are pressing the Right key, we do notwant a segment ahead of us
				}
				// toggle the audio clip and play
				if(audio)  {
					audio.clip = (audio.clip == move1) ? move2 : move1;
					audio.Play();
				}
			}
			break;
		}
		yield return null;
	}


	private void UpdateMovePosition(List<Rect> tmpRects)
	{
		for(int i=0; i < snakePos.Count - 1; i++)
		{
			snakePos[i+1] = tmpRects[i];
		}
	}

	private bool CheckForFood()
	{
		if(Food.Instance != null)
		{
			Rect foodRect = Food.Instance.foodPos;
			if(snakePos[0].Contains(new Vector2(foodRect.x, foodRect.y)))
			{
				Debug.Log ("Yummy!");

				Food.Instance.UpdateFood();
				SnakeGame.Instance.UpdateScore(1);
				return true;
			}
		}
		return false;
	}


	Rect CheckForValidUpPosition()
	{
		if(snakePos[snakePos.Count-1].y != 0)
		{
			return new Rect(snakePos[snakePos.Count-1].x, snakePos[snakePos.Count-1].y - Globals.TileSize, Globals.TileSize, Globals.TileSize);
		}
		return new Rect(0, 0, 0, 0);
	}

	Rect CheckForValidDownPosition()
	{
		if(snakePos[snakePos.Count-1].y != Globals.GameFieldHeight)
		{
			return new Rect(snakePos[snakePos.Count-1].x, snakePos[snakePos.Count-1].y + Globals.TileSize, Globals.TileSize, Globals.TileSize);
		}
		return new Rect(0, 0, 0, 0);
	}
	// lewo, gora
	Rect CheckForValidLeftPosition()
	{
		if(snakePos[snakePos.Count-1].x != 0)
		{
			return new Rect(snakePos[snakePos.Count-1].x - Globals.TileSize, snakePos[snakePos.Count-1].y, Globals.TileSize, Globals.TileSize);
		}
		return new Rect(0, 0, 0, 0);
	}

	Rect CheckForValidRightPosition()
	{
		if(snakePos[snakePos.Count-1].x != Globals.GameFieldWidth)
		{
			return new Rect(snakePos[snakePos.Count-1].x + Globals.TileSize, snakePos[snakePos.Count-1].y, Globals.TileSize, Globals.TileSize);
		}
		return new Rect(0, 0, 0, 0);
	}

	#endregion	
}
