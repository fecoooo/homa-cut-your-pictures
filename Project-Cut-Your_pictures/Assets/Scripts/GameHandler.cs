using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviourSingleton<GameHandler>
{
	public delegate void GameStateChangeHandler(GameState state);
	public event GameStateChangeHandler GameStateChanged;

	const float TimeInBeforeGameState = .5f;

	public Transform[] imagesToFocus;
	int currentImageIndex = -1;

	public Transform cuttingTableScene;

	CuttingTable cuttingTable;
	Transform currentPiece;
	Transform currentPicture;

	protected override void OnAwake()
	{
		base.OnAwake();
		GameStateChanged += OnGameStateChanged;
	}
	
	void Start()
	{
		cuttingTable = cuttingTableScene.Find("CuttingTable").GetComponent<CuttingTable>();
		GameStateChanged(GameState.Start);
	}

	void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			currentImageIndex++;
			bool shouldUseZoom = currentImageIndex == 0 ? true : false;

			currentPicture = imagesToFocus[0];
			currentPiece = imagesToFocus[currentImageIndex];
			CameraController.instance.FocusImage(currentPiece.position, shouldUseZoom);
		}

		if (Input.GetKeyDown(KeyCode.A))
			StartCoroutine(StartGameOnPiece());

		if (Input.GetKeyDown(KeyCode.Y))
			StartCoroutine(MovePieceToPicture());
	}

	IEnumerator StartGameOnPiece()
	{
		yield return CameraController.instance.MovePieceRoutine(currentPiece.position, cuttingTableScene.position, currentPiece, cuttingTable.transform.localPosition);
		yield return currentPiece.GetComponent<Piece>().ScaleUp();

		GameStateChanged(GameState.BeforeGame);
		ChangeGameStateDelayed(TimeInBeforeGameState, GameState.InGame);
	}

	void ChangeGameStateDelayed(float delay, GameState state)
	{
		StartCoroutine(ChangeGameStateDelayedRoutine(delay, state));
	}

	IEnumerator ChangeGameStateDelayedRoutine(float delay, GameState state)
	{
		yield return new WaitForSeconds(delay);
		GameStateChanged(state);
	}

	IEnumerator MovePieceToPicture()
	{
		yield return currentPiece.GetComponent<Piece>().ScaleDown();
		yield return CameraController.instance.MovePieceRoutine(currentPiece.position, currentPicture.position, currentPiece, Vector2.zero);
		currentPiece.parent = currentPicture;
	}

	bool clickedOnce = false;
	public void ButtonClick()
	{
		if (!clickedOnce)
		{
			currentImageIndex = 1;

			currentPicture = imagesToFocus[0];
			currentPiece = imagesToFocus[currentImageIndex];
			CameraController.instance.FocusImage(currentPiece.position, true);

			clickedOnce = true;
		}
		else
		{
			StartCoroutine(StartGameOnPiece());
		}
	}

	private void OnGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.Start:
				break;
			case GameState.MainMenu:
				break;
			case GameState.InGame:
				break;
			default:
				break;
		}
	}

	public enum GameState
	{
		Start,
		MainMenu,
		BeforeGame,
		InGame,
	}
	
}
