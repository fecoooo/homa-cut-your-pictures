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

	public int CurrentExcercise
	{
		get => lastFinishedPiece + 1;
	}

	GameState currentState;

	CuttingTable cuttingTable;
	Transform currentPicture;
	Piece currentPiece;

	int lastFinishedPiece;

	const float TimeOnWinScreen = 2f;

	protected override void OnAwake()
	{
		base.OnAwake();
		GameStateChanged += OnGameStateChanged;
	}
	
	void Start()
	{
		lastFinishedPiece = PlayerPrefs.GetInt("LastFinishedPiece", -1);
		SetPiecesCompleteState();

		cuttingTable = cuttingTableScene.Find("CuttingTable").GetComponent<CuttingTable>();
		GameStateChanged(GameState.Start);
	}

	public void SelectPiece(int index)
	{
		currentPiece = pieces[index];
		for(int i = 0; i < pieces.Length; ++i)
			pieces[i].SetSelected(i == index);
	}

	public void FocusCurrentPiece() 
	{
		GameStateChanged(GameState.MainMenuZoomIn);
		StartCoroutine(FocusCurrentPieceRoutine());
	}

	IEnumerator FocusCurrentPieceRoutine()
	{
		if (CurrentExcercise <= 4)
		{
			currentPiece = pieces[CurrentExcercise].GetComponent<Piece>();
			currentPicture = currentPiece.transform.parent;
			yield return CameraController.instance.FocusImageRoutine(currentPicture.position, true);
			MenuUIHandler.instance.SetDisabled(CurrentExcercise + 1);
			MenuUIHandler.instance.ClickToggle(CurrentExcercise);
		}
	}

	public void StartGameOnPiece()
	{
		StartCoroutine(StartGameOnPieceRoutine());
	}

	IEnumerator StartGameOnPieceRoutine()
	{
		GameStateChanged(GameState.TransferringPiece);
		yield return CameraController.instance.FocusImageRoutine(currentPiece.transform.position, false);

		currentPiece.PrepareForMove();
		
		yield return CameraController.instance.MovePieceRoutine(currentPiece.transform.position, cuttingTableScene.position, 
			currentPiece.transform, cuttingTable.transform.localPosition);
		yield return currentPiece.GetComponent<Piece>().ScaleUp();

		GameStateChanged(GameState.BeforeGame);
		ChangeGameStateDelayed(TimeInBeforeGameState, GameState.InGame);
	}

	internal void GameLost()
	{
		GameStateChanged(GameState.GameLost);
	}

	internal void GameWon()
	{
		GameStateChanged(GameState.GameWon);
		MovePieceToPicture(TimeOnWinScreen);
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

	public void MovePieceToPicture(float delay = 0)
	{
		StartCoroutine(MovePieceToPictureRoutine(delay));
	}

	IEnumerator MovePieceToPictureRoutine(float delay)
	{
		yield return new WaitForSeconds(delay);
		GameStateChanged(GameState.TransferringPiece);
		yield return currentPiece.GetComponent<Piece>().ScaleDown();
		yield return CameraController.instance.MovePieceRoutine(currentPiece.transform.position, currentPicture.position, 
			currentPiece.transform, currentPiece.MenuLocalPosition);

		currentPiece.ResetOnMenu();

		if (CuttingTable.instance.WonLast)
			StartWinAnimation();
		else
			GameStateChanged(GameState.MainMenuZoomIn);
	}

	private void StartWinAnimation()
	{
		GameStateChanged(GameState.WinAnimation);
		StartCoroutine(WinAnimationRoutine());
	}

	IEnumerator WinAnimationRoutine()
	{
		yield return new WaitForSeconds(1f);
		currentPiece.SetCompleted(true);

		yield return new WaitForSeconds(1f);

		lastFinishedPiece++;
		PlayerPrefs.SetInt("LastFinishedPiece", lastFinishedPiece);
		MenuUIHandler.instance.SetDisabled(lastFinishedPiece + 2);
		MenuUIHandler.instance.ClickToggle(lastFinishedPiece + 1);

		GameStateChanged(GameState.MainMenuZoomIn);
	}

	private void OnGameStateChanged(GameState state)
	{
		currentState = state;
	}

	void SetPiecesCompleteState()
	{
		for(int i = 0; i < pieces.Length; ++i)
			pieces[i].SetCompleted(i <= lastFinishedPiece);
	}

	public enum GameState
	{
		Start,
		MainMenuZoomOut,
		MainMenuZoomIn,
		TransferringPiece,
		BeforeGame,
		InGame,
		GameWon,
		GameLost,
		WinAnimation
	}
}
