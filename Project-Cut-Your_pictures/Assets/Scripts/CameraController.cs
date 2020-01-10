using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviourSingleton<CameraController>
{
	const float CameraZ = -10f;

	const float ZoomedInCameraSize = 7.5f;
	const float ZoomedOutCameraSize = 25f;

	const float MovePieceAnimTime = 1f;

	const float FocusAnimTime = 0.5f;
	const float ZoomAnimTime = 0.5f;

	new Camera camera;

	bool IsZoomedIn
	{
		get => camera.orthographicSize - ZoomedInCameraSize < Mathf.Epsilon;
	}

	private void Start()
	{
		camera = GetComponent<Camera>();
	}

	public void MovePiece(Vector2 from, Vector2 to, Transform piece)
	{
		StartCoroutine(MovePieceRoutine(from, to, piece));
	}

	public IEnumerator MovePieceRoutine(Vector2 from, Vector2 to, Transform piece)
	{
		piece.parent = camera.transform;
		float timePassed = 0;

		while (timePassed < MovePieceAnimTime)
		{
			float t = timePassed / MovePieceAnimTime;

			Vector3 currentPosition = Vector2.Lerp(from, to, t);
			currentPosition.z = CameraZ;
			transform.position = currentPosition;

			timePassed += Time.deltaTime;
			yield return null;
		}

		transform.position = new Vector3(to.x, to.y, CameraZ);
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

		while(timePassed < FocusAnimTime)
		{
			float t = timePassed / FocusAnimTime;

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

		while (timePassed < ZoomAnimTime)
		{
			float t = timePassed / ZoomAnimTime;

			if (zoomType == ZoomType.Out)
				t = 1 - t;

			camera.orthographicSize = Mathf.Lerp(ZoomedOutCameraSize, ZoomedInCameraSize, t);

			timePassed += Time.deltaTime;
			yield return null;
		}

		camera.orthographicSize = zoomType == ZoomType.In ? ZoomedInCameraSize : ZoomedOutCameraSize;
	}

	enum ZoomType
	{
		In,
		Out
	}
}
