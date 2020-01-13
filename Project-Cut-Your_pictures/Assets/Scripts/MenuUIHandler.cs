using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameHandler;

public class MenuUIHandler:MonoBehaviour
{
	Canvas canvas;
	Transform playCurrentPiece;
	Transform zoomedButtons;

	void Start()
    {
		canvas = GetComponent<Canvas>();
		playCurrentPiece = transform.Find("PlayCurrentPiece");
		zoomedButtons = transform.Find("ZoomedButtons");

		GameHandler.instance.GameStateChanged += OnGameStateChanged;
	}

	void OnGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.Start:
				canvas.enabled = true;
				playCurrentPiece.gameObject.SetActive(true);
				zoomedButtons.gameObject.SetActive(false);
				break;
			case GameState.MainMenuZoomOut:
				playCurrentPiece.gameObject.SetActive(true);
				zoomedButtons.gameObject.SetActive(false);
				canvas.enabled = true;
				break;
			case GameState.MainMenuZoomIn:
				playCurrentPiece.gameObject.SetActive(false);
				zoomedButtons.gameObject.SetActive(true);
				break;
			case GameState.InGame:
				canvas.enabled = false;
				break;
			default:
				break;
		}
	}
}
