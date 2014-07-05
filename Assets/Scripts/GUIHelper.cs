using UnityEngine;
using System.Collections;

public class GUIHelper : MonoBehaviour {

	public static GUIText CreateGetGUIText(Vector2 offset, string text, float layer)
	{
		return CreateGetGUIText(offset, "GUITextObject", text, layer);
	}

	public static GUIText CreateGetGUIText(Vector2 offset, string name, string text, float layer)
	{
		GameObject guiTextObject = new GameObject(name);

		guiTextObject.transform.position = new Vector3(0,0, layer);
		guiTextObject.transform.rotation = Quaternion.identity;
		guiTextObject.transform.localScale = Vector3.one;

		GUIText	guiDisplayText = guiTextObject.AddComponent<GUIText>();
		guiDisplayText.pixelOffset = offset;
		guiDisplayText.text = text;

		return guiDisplayText;
	}


	public static void CreateGUITexture(Rect coordinates, Color colTexture, float layer)
	{
		CreateGUITexture(coordinates, colTexture, "GUITextureOBject", layer);
	}

	public static void CreateGUITexture(Rect coordinates, Color colTexture, string name, float layer)
	{
		// we need a new game object to hold the component
		GameObject guiTextureObject = new GameObject(name);
		guiTextureObject.transform.position = new Vector3(0, 0, layer);
		guiTextureObject.transform.rotation = Quaternion.identity;
		guiTextureObject.transform.localScale = new Vector3(0.01f, 0.01f, 1.0f);

		GUITexture guiDisplayTexture = guiTextureObject.AddComponent<GUITexture>();
		Texture2D guiTexture = TextureHelper.Create1x1Texture(colTexture);
		guiDisplayTexture.texture = guiTexture;
		guiDisplayTexture.pixelInset = coordinates;
	}


	public static GUITexture CreateGetGUITexture(Rect coordinates, Color colTexture, float layer)
	{
		return CreateGetGUITexture(coordinates, colTexture, "GUITextureOBject", layer);
	}

	public static GUITexture CreateGetGUITexture(Rect coordinates, Color colTexture, string name, float layer)
	{
		// we need a new game object to hold the component
		GameObject guiTextureObject = new GameObject(name);
		guiTextureObject.transform.position = new Vector3(0, 0, layer);
		guiTextureObject.transform.rotation = Quaternion.identity;
		guiTextureObject.transform.localScale = new Vector3(0.01f, 0.01f, 1.0f);

		GUITexture guiDisplayTexture = guiTextureObject.AddComponent<GUITexture>();
		Texture2D guiTexture = TextureHelper.Create1x1Texture(colTexture);
		guiDisplayTexture.texture = guiTexture;
		guiDisplayTexture.pixelInset = coordinates;

		return guiDisplayTexture;
	}
}
