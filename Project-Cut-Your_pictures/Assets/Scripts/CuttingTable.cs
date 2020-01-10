using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTable : MonoBehaviourSingleton<CuttingTable>
{
	const float ScaleUpAnimTime = 0.3f;
	const float CountDownTime = 1f;
	const float TimeBetweenScaleUpAndUI = .3f;

	public GameObject CuttingUI;

	public bool InGameCutting { get; private set; }

	Template template;
	SpriteRenderer piece;
	int currentLevelIndex;

	LevelData currentLevelData;

	void Start()
    {
		template = transform.Find("Template").GetComponent<Template>();
		piece = transform.Find("Piece").GetComponent<SpriteRenderer>();

		currentLevelIndex = PlayerPrefs.GetInt("currentLevelIndex", 0);

		//StartGame();
    }

	public void Restart()
	{
		StartGame();
	}

	void StartGame()
	{
		InGameCutting = false;
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

	public void InitTable()
	{
		StartCoroutine(InitTableRoutine());
	}

	IEnumerator InitTableRoutine()
	{
		yield return ScaleUp();
		yield return new WaitForSeconds(TimeBetweenScaleUpAndUI);
		CuttingUI.SetActive(true);
	}

	public IEnumerator ScaleUp()
	{
		float timePassed = 0;

		while (timePassed < ScaleUpAnimTime)
		{
			float t = timePassed / ScaleUpAnimTime;

			transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

			timePassed += Time.deltaTime;
			yield return null;
		}

		transform.localScale = Vector3.one;
	}
}
