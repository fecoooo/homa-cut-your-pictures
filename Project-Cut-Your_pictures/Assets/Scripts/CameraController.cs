using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviourSingleton<CameraController>
{
	const float zoomedInCameraSize = 7.5f;
	const float zoomedOutCameraSize = 25f;

	const float movePieceAnimTime = 1f;
	
	public float focusAnimTime = .5f;
	public float zoomAnimTime = .5f;

	public Transform cuttingTable;

	Vector3 mainMenuPosition;
	Transform originalParent;
	Transform currentPiece;

	new Camera camera;

	bool IsZoomedIn
	{
		get => camera.orthographicSize - zoomedInCameraSize < Mathf.Epsilon;
	}

	private void Start()
	{
		camera = GetComponent<Camera>();
	}

	public void MovePieceBackToPicture()
	{
		StartCoroutine(MovePieceBackToPictureRoutine());
	}

	IEnumerator MovePieceBackToPictureRoutine()
	{
		currentPiece.parent = camera.transform;
		float timePassed = 0;

		while (timePassed < movePieceAnimTime)
		{
			float t = timePassed / movePieceAnimTime;

			Vector3 currentPosition = Vector2.Lerp(cuttingTable.position, mainMenuPosition, t);
			currentPosition.z = mainMenuPosition.z;
			transform.position = currentPosition;

			timePassed += Time.deltaTime;
			yield return null;
		}

		transform.position = mainMenuPosition;
		currentPiece.parent = originalParent;
	}

	public void MovePieceToCuttingTable(Transform piece)
	{
		currentPiece = piece;
		StartCoroutine(MovePieceToCuttingTableRoutine());
	}

	IEnumerator MovePieceToCuttingTableRoutine()
	{
		originalParent = currentPiece.parent;
		currentPiece.parent = camera.transform;
		float timePassed = 0;

		mainMenuPosition = transform.position;

		while (timePassed < movePieceAnimTime)
		{
			float t = timePassed / movePieceAnimTime;

			Vector3 currentPosition = Vector2.Lerp(mainMenuPosition, cuttingTable.position, t);
			currentPosition.z = mainMenuPosition.z;
			transform.position = currentPosition;

			timePassed += Time.deltaTime;
			yield return null;
		}

		transform.position = new Vector3(cuttingTable.position.x, cuttingTable.position.y, mainMenuPosition.z);
	}

	public void FocusImage(Vector2 imageCenter, bool withZoomTransition)
	{
		StartCoroutine(FocusImageRoutine(imageCenter, withZoomTransition));
	}

	IEnumerator FocusImageRoutine(Vector2 imageCenter, bool withZoomTransition)
	{
		if (withZoomTransition && IsZoomedIn)
			yield return ZoomRoutine(ZoomType.Out);

		yield return PositionOnImageCenterRoutine(imageCenter);

		if(withZoomTransition)
			yield return ZoomRoutine(ZoomType.In);
	}

	IEnumerator PositionOnImageCenterRoutine(Vector2 imageCenter)
	{
		float timePassed = 0;
		Vector3 originalPosition = transform.position;

		while(timePassed < focusAnimTime)
		{
			float t = timePassed / focusAnimTime;

			Vector3 currentPosition = Vector2.Lerp(originalPosition, imageCenter, t);
			currentPosition.z = originalPosition.z;
			transform.position = currentPosition;

			timePassed += Time.deltaTime;
			yield return null;
		}

		transform.position = new Vector3(imageCenter.x, imageCenter.y, originalPosition.z);
	}

	IEnumerator ZoomRoutine(ZoomType zoomType)
	{
		float timePassed = 0;

		while (timePassed < zoomAnimTime)
		{
			float t = timePassed / zoomAnimTime;

			if (zoomType == ZoomType.Out)
				t = 1 - t;

			camera.orthographicSize = Mathf.Lerp(zoomedOutCameraSize, zoomedInCameraSize, t);

			timePassed += Time.deltaTime;
			yield return null;
		}

		camera.orthographicSize = zoomType == ZoomType.In ? zoomedInCameraSize : zoomedOutCameraSize;
	}

	enum ZoomType
	{
		In,
		Out
	}
}
