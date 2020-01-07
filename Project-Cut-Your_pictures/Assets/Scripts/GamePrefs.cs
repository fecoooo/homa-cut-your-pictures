using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePrefs : MonoBehaviourSingleton<GamePrefs>
{
	public readonly LevelData[] LevelDatas = 
	{
		new LevelData(4, new Vector2(0, -0.752f), -90,	"Levels/template_1", "Levels/piece_1"),
		new LevelData(3, new Vector2(0, 3.4693f), 150,	"Levels/template_2", "Levels/piece_2"),
	};
}
