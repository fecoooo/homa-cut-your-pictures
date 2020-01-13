using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviourSingleton<GameHandler>
{
	public delegate void GameStateChangeHandler(GameState state);
	public event GameStateChangeHandler GameStateChanged;

	const float TimeInBeforeGameState = .5f;

	public Transform[] pictures;
	public Piece[] pieces;

	public Transform cuttingTableScene;

	CuttingTable cuttingTable;
	Transform currentPicture;
	Piece currentPiece;

	int currentLevelIndex;

	protected override void OnAwake()
	{
		base.OnAwake();
		GameStateChanged += OnGameStateChanged;
	}
	
	void Start()
	{
		currentLevelIndex = PlayerPrefs.GetInt("currentLevelIndex", 0);
		cuttingTable = cuttingTableScene.Find("CuttingTable").GetComponent<CuttingTable>();
		GameStateChanged(GameState.Start);
	}


	void Update()
    {
		if (Input.GetKeyDown(KeyCode.A))
			StartCoroutine(StartGameOnPieceRoutine());

		if (Input.GetKeyDown(KeyCode.Y))
			StartCoroutine(MovePieceToPicture());
	}

	public void SelectPiece(int index)
	{
		currentPiece = pieces[index];
		for(int i = 0; i < pieces.Length; ++i)
			pieces[i].SetHasOutline(i == index);
	}

	public void FocusCurrentPiece() 
	{
		GameStateChanged(GameState.MainMenuZoomIn);
		StartCoroutine(FocusCurrentPieceRoutine());
	}

	IEnumerator FocusCurrentPieceRoutine()
	{
		currentPiece = pieces[currentLevelIndex].GetComponent<Piece>();
		currentPicture = currentPiece.transform.parent;
		yield return CameraController.instance.FocusImageRoutine(currentPicture.position, true);
	}

	public void StartGameOnPiece()
	{
		StartCoroutine(StartGameOnPieceRoutine());
	}

	IEnumerator StartGameOnPieceRoutine()
	{
		yield return CameraController.instance.FocusImageRoutine(currentPiece.transform.position, false);
		yield return CameraController.instance.MovePieceRoutine(currentPiece.transform.position, cuttingTableScene.position, 
			currentPiece.transform, cuttingTable.transform.localPosition);
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
		yield return CameraController.instance.MovePieceRoutine(currentPiece.transform.position, currentPicture.position, 
			currentPiece.transform, Vector2.zero);
		currentPiece.transform.parent = currentPicture;
	}

	private void OnGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.Start:
				break;
			case GameState.MainMenuZoomOut:
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
		MainMenuZoomOut,
		MainMenuZoomIn,
		BeforeGame,
		InGame,
	}
}
