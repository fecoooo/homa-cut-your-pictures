﻿using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameHandler;

public class GameUIHandler:MonoBehaviour
{
	TextMeshProUGUI freezeCountLbl;
	Image progressBar;
	TextMeshProUGUI progressLbl;
	TextMeshProUGUI countDownLbl;
	TextMeshProUGUI middleMsgLbl;
	Canvas canvas;
	string[] countDownTexts = { "3", "2", "1", "Start"};

	void Start()
    {
		canvas = GetComponent<Canvas>();

		freezeCountLbl = transform.Find("Freeze/FreezeCountLbl").GetComponent<TextMeshProUGUI>();
		progressBar = transform.Find("Progress/ProgressBar").GetComponent<Image>();
		progressLbl = transform.Find("Progress/ProgressLbl").GetComponent<TextMeshProUGUI>();
		countDownLbl = transform.Find("CountDownLbl").GetComponent<TextMeshProUGUI>();
		middleMsgLbl = transform.Find("MiddleMsgLbl").GetComponent<TextMeshProUGUI>();

		Cutter.instance.FreezeCountChanged += OnFreezeCountChanged;
		GameHandler.instance.GameStateChanged += OnGameStateChanged;
	}

	void Update()
    {
		if(CuttingTable.instance.InGameCutting)
			UpdateProgress();
    }

	void UpdateProgress()
	{
		progressBar.transform.localScale = new Vector3(Template.instance.Progress, 1, 1);
		progressLbl.text = Mathf.CeilToInt(Template.instance.Progress * 100) + "%";
	}

	void OnFreezeCountChanged(int newValue)
	{
		freezeCountLbl.text = newValue + "x";
	}

	void OnGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.Start:
				canvas.enabled = false;
				break;
			case GameState.MainMenuZoomOut:
				canvas.enabled = false;
				break;
			case GameState.BeforeGame:
				UpdateProgress();
				middleMsgLbl.gameObject.SetActive(false);
				canvas.enabled = true;
				CountDown();
				break;
			case GameState.TransferringPiece:
				middleMsgLbl.gameObject.SetActive(false);
				canvas.enabled = false;
				break;
			case GameState.GameWon:
				middleMsgLbl.gameObject.SetActive(true);
				middleMsgLbl.text = "You won!";
				break;
			case GameState.GameLost:
				middleMsgLbl.gameObject.SetActive(true);
				middleMsgLbl.text = "You lose! :(";
				break;
			default:
				break;
		}
	}

	void CountDown()
	{
		StartCoroutine(CountDownRoutine());
	}

	IEnumerator CountDownRoutine()
	{
		countDownLbl.gameObject.SetActive(true);

		float timePassed = 0;
		while (timePassed < CuttingTable.CountDownTime)
		{
			float t = timePassed / CuttingTable.CountDownTime;
			int textIndex = Mathf.FloorToInt(t * countDownTexts.Length);
			countDownLbl.text = countDownTexts[textIndex];

			timePassed += Time.deltaTime;
			yield return null;
		}

		countDownLbl.gameObject.SetActive(false);
	}
}
