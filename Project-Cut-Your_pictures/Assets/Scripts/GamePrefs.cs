using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GamePrefs : MonoBehaviourSingleton<GamePrefs>
{
	public Material originalMat;
	public Material greyScaleMat;

	public readonly LevelData[] LevelDatas = 
	{
		new LevelData(4, new Vector2(0.1598f, -0.7364f),	-90,	"Levels/template_11", "Levels/piece_11", "Levels/outline_11", 1070),
		new LevelData(2, new Vector2(1.6782f, -0.5857f),	0,		"Levels/template_12", "Levels/piece_12", "Levels/outline_12", 790),
		new LevelData(3, new Vector2(1.684f, -0.504f),		16,		"Levels/template_13", "Levels/piece_13", "Levels/outline_13", 770),
		new LevelData(3, new Vector2(0.615f, -1.865f),		2,		"Levels/template_14", "Levels/piece_14", "Levels/outline_14", 930),
		new LevelData(0, new Vector2(-0.026f, -1.335f),		-83,	"Levels/template_15", "Levels/piece_15", "Levels/outline_15", 770),
	};
}
