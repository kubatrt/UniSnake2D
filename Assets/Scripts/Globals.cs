﻿using UnityEngine;
using System.Collections;

public class Globals 
{
	public static readonly int 	TileSize = 32;
	public static readonly int 	ScreenWidth = 800;
	public static readonly int 	ScreenHeight = 600;
	
	public static readonly int 	GameFieldX = 0;
	public static readonly int	GameFieldY = 96;
	public static readonly int 	GameFieldWidth = ScreenWidth - GameFieldX;
	public static readonly int 	GameFieldHeight = ScreenHeight - GameFieldY;

	public static readonly int 	LayerBackround = -100;
	public static readonly int 	LayerForeground = 0;
	public static readonly int 	LayerHUD = 100;
}

//  GUI COORDS (0,0)
//  +---------
//  |
//  |
//           (1,1)