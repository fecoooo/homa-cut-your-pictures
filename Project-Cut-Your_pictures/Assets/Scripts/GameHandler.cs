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

	int lastFinishedPiece;

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
		currentPiece = pieces[lastFinishedPiece + 1].GetComponent<Piece>();
		currentPicture = currentPiece.transform.parent;
		yield return CameraController.instance.FocusImageRoutine(currentPicture.position, true);
		MenuUIHandler.instance.SetDisabled(lastFinishedPiece + 2);
		MenuUIHandler.instance.ClickToggle(lastFinishedPiece + 1);
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

	void ChangeGameStateDelayed(float delay, GameState state)
	{
		StartCoroutine(ChangeGameStateDelayedRoutine(delay, state));
	}

	IEnumerator ChangeGameStateDelayedRoutine(float delay, GameState state)
	{
		yield return new WaitForSeconds(delay);
		GameStateChanged(state);
	}

	public void MovePieceToPicture()
	{
		StartCoroutine(MovePieceToPictureRoutine());
	}

	IEnumerator MovePieceToPictureRoutine()
	{
		GameStateChanged(GameState.TransferringPiece);
		yield return currentPiece.GetComponent<Piece>().ScaleDown();
		yield return CameraController.instance.MovePieceRoutine(currentPiece.transform.position, currentPicture.position, 
			currentPiece.transform, currentPiece.MenuLocalPosition);

		currentPiece.ResetOnMenu();

		GameStateChanged(GameState.MainMenuZoomIn);
	}

	private void OnGameStateChanged(GameState state)
	{
		//dummy
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
	}
}
