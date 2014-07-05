using UnityEngine;
using System.Collections;

public class TextureHelper : MonoBehaviour
{
	// overloaded method to create a texture with color
	public static Texture2D CreateTexture(int width, int height)
	{
		return CreateTexture(width, height, Color.black);
	}

	// Create and return a black texture
	public static Texture2D CreateTexture(int width, int height, Color color)
	{
		Texture2D texture = new Texture2D(width, height);
		for (int i = 0; i < width; i++)
		{
			for (int j = 0; j < height; j++)
			{
				texture.SetPixel(i, j, color);
			}
		}
		texture.Apply();
		return texture;
	}
	
	public static Texture2D Create1x1Texture()
	{
		return Create1x1Texture(Color.black);
	}
	
	public static Texture2D Create1x1Texture(Color color)
	{
		Texture2D texture = new Texture2D(1, 1);
		texture.SetPixel(0, 0, color);
		texture.Apply();
		return texture;
	}
}