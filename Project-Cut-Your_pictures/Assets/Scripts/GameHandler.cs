using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
	const float WaitBetweenScaleUp = .2f;

	public Transform[] imagesToFocus;
	int currentImageIndex = -1;

	public Transform cuttingTableScene;

	CuttingTable cuttingTable;
	Transform currentPiece;
	Transform currentPicture;

	void Start()
	{
		cuttingTable = cuttingTableScene.Find("CuttingTable").GetComponent<CuttingTable>();
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
		yield return new WaitForSeconds(WaitBetweenScaleUp);
		cuttingTable.InitTable();
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
	
}
