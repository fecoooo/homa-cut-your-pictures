using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils
{
	public static Vector2Int ToVector2Int(this Vector2 v)
	{
		return new Vector2Int((int)v.x, (int)v.y);
	}

}
