using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviourSingleton<CameraController>
{
	const float zoomedInCameraSize = 1.5f;
	const float zoomedOutCameraSize = 5f;
	
	public float focusAnimTime = .5f;
	public float zoomAnimTime = .5f;

	new Camera camera;

	bool IsZoomedIn
	{
		get => camera.orthographicSize - zoomedInCameraSize < Mathf.Epsilon;
	}

	private void Start()
	{
		camera = GetComponent<Camera>();
	}

	public void FocusImage(Vector2 imageCenter)
	{
		StartCoroutine(FocusImageRoutine(imageCenter));
	}

	IEnumerator FocusImageRoutine(Vector2 imageCenter)
	{
		if (IsZoomedIn)
			yield return ZoomRoutine(ZoomType.Out);

		yield return PositionOnImageCenterRoutine(imageCenter);
		yield return ZoomRoutine(ZoomType.In);
	}

	IEnumerator PositionOnImageCenterRoutine(Vector2 imageCenter)
	{
		float timePassed = 0;
		Vector3 orignalPositon = transform.position;

		while(timePassed < focusAnimTime)
		{
			float t = timePassed / focusAnimTime;

			Vector3 currentPosition = Vector2.Lerp(orignalPositon, imageCenter, t);
			currentPosition.z = orignalPositon.z;
			transform.position = currentPosition;

			timePassed += Time.deltaTime;
			yield return null;
		}

		transform.position = new Vector3(imageCenter.x, imageCenter.y, orignalPositon.z);
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

		switch (zoomType)
		{
			case ZoomType.In:
				camera.orthographicSize = zoomedInCameraSize;
				break;
			case ZoomType.Out:
				camera.orthographicSize = zoomedOutCameraSize;
				break;
			default:
				break;
		}
	}

	enum ZoomType
	{
		In,
		Out
	}
}
