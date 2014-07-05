using UnityEngine;
using System.Collections;

public class ScreenHelper : MonoBehaviour {

	public static IEnumerator FlashScreen(int flashTimes, float flashDelay, Color flashColor)
	{
		GUITexture flashScreenTexture = GUIHelper.CreateGetGUITexture( new Rect(
			Globals.GameFieldX,Globals.GameFieldY,
			Globals.GameFieldWidth, Globals.GameFieldHeight), 
		    flashColor, 20);

		for(int i=0; i < flashTimes; i++)
		{
			flashScreenTexture.color = flashColor;
			yield return new WaitForSeconds(flashDelay);
			flashScreenTexture.color = Color.clear;
			yield return new WaitForSeconds(flashDelay);
		}
		Destroy(flashScreenTexture.gameObject);
	}
}
