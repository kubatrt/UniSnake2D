using UnityEngine;
using System.Collections;


public class InputHelper : MonoBehaviour
{
	public static bool GetStandardMoveUpDirection()
	{
		if (Input.GetKey (KeyCode.W) || Input.GetKey(KeyCode.UpArrow)) { return true; }
		return false;
	}

	public static bool GetStandardMoveLeftDirection()
	{
		if (Input.GetKey (KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)) { return true; }
		return false;
	}

	public static bool GetStandardMoveDownDirection()
	{
		if (Input.GetKey (KeyCode.S) || Input.GetKey(KeyCode.DownArrow)) { return true; }
		return false;
	}

	public static bool GetStandardMoveRightDirection()
	{
		if (Input.GetKey (KeyCode.D) || Input.GetKey(KeyCode.RightArrow)) { return true; }
		return false;
	}
}