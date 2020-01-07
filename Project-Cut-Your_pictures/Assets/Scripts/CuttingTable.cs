using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTable : MonoBehaviour
{
	Template template;
	SpriteRenderer piece;
	int currentLevelIndex;

    void Start()
    {
		template = transform.Find("Template").GetComponent<Template>();
		piece = transform.Find("Piece").GetComponent<SpriteRenderer>();

		currentLevelIndex = PlayerPrefs.GetInt("currentLevelIndex", 0);
		LoadLevel(currentLevelIndex);
    }

    void LoadLevel(int levelIndex)
	{
		LevelData currentLevelData = GamePrefs.instance.LevelDatas[levelIndex];

		Cutter.instance.Init(currentLevelData);
		template.LoadImage(currentLevelData.templatePath);
		piece.sprite = Resources.Load<Sprite>(currentLevelData.piecePath);

	}

	private void Update()
	{
		if (Input.GetKeyDown(KeyCode.Space))
		{
			LoadLevel(++currentLevelIndex);
		}
	}
}
