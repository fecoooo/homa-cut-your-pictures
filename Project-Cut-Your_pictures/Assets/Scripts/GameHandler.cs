using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameHandler : MonoBehaviour
{
	public Transform[] imagesToFocus;
	int currentImageIndex = -1;

	public Transform cuttingTable;
	Transform currentPiece;
	Transform currentPicture;


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

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			StartCoroutine(MovePieceToCuttingTable());
		}

		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			StartCoroutine(MovePieceToPicture());
		}
	}

	IEnumerator MovePieceToCuttingTable()
	{
		yield return CameraController.instance.MovePieceRoutine(currentPiece.position, cuttingTable.position, currentPiece);
		yield return currentPiece.GetComponent<Piece>().ScaleUp();
		currentPiece.parent = cuttingTable;
	}

	IEnumerator MovePieceToPicture()
	{
		yield return currentPiece.GetComponent<Piece>().ScaleDown();
		yield return CameraController.instance.MovePieceRoutine(currentPiece.position, currentPicture.position, currentPiece);
		currentPiece.parent = currentPicture;
	}
	
}
