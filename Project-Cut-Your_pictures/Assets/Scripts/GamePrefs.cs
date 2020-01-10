using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePrefs : MonoBehaviourSingleton<GamePrefs>
{
	public readonly LevelData[] LevelDatas = 
	{
		new LevelData(4, new Vector2(0.012f, -2.606f),	-80, "Levels/template_3", "Levels/piece_3", 1600),
		new LevelData(3, new Vector2(0, 3.4693f),		150, "Levels/template_2", "Levels/piece_2", 0),
	};
}
