using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTable : MonoBehaviourSingleton<CuttingTable>
{
	public bool InGameCutting { get; private set; }
	const float CountDownTime = 1f;

	Template template;
	SpriteRenderer piece;
	int currentLevelIndex;

	LevelData currentLevelData;

	void Start()
    {
		template = transform.Find("Template").GetComponent<Template>();
		piece = transform.Find("Piece").GetComponent<SpriteRenderer>();

		currentLevelIndex = PlayerPrefs.GetInt("currentLevelIndex", 0);

		StartGame();
    }

	void StartGame()
	{
		LoadLevel(currentLevelIndex);
		StartCoroutine(CountDownThanStart());
	}

	private IEnumerator CountDownThanStart()
	{
		yield return new WaitForSeconds(CountDownTime);
		InGameCutting = true;
	}

	void LoadLevel(int levelIndex)
	{
		currentLevelData = GamePrefs.instance.LevelDatas[levelIndex];

		Cutter.instance.Init(currentLevelData);
		template.Init(currentLevelData.templatePath, currentLevelData.minimumPixelToCut);
		piece.sprite = Resources.Load<Sprite>(currentLevelData.piecePath);
	}

	public void Fail()
	{
		InGameCutting = false;
		Debug.Log("You cut into the Piece :(((((((");
	}

	public void EndedCircle(int pixelsCut)
	{
		if (pixelsCut >= currentLevelData.minimumPixelToCut)
			Debug.Log("You won!");
		else
			Debug.Log("You lose!");

		InGameCutting = false;
	}
}
