using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePrefs : MonoBehaviourSingleton<GamePrefs>
{
	public Material originalMat;
	public Material greyScaleMat;

	public readonly LevelData[] LevelDatas = 
	{
		new LevelData(4, new Vector2(0.012f, -2.606f),	-80, "Levels/template_11", "Levels/piece_11", "Levels/outline_11", 1600),
		new LevelData(3, new Vector2(0, 3.4693f),		150, "Levels/template_12", "Levels/piece_12", "Levels/outline_12", 0),
	};
}
