using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
	const float ScaleUpAnimTime = 0.3f;
	const int SelectedSortingOrder = 100;

	Vector3 menuScale;
	Transform parentPicture;
	int menuSortingOrder;
	public Vector2 MenuLocalPosition { get; private set; }

	readonly Vector3 scaledUpScale = new Vector3(1,1,1);
	Outline outline;
	bool isSelected;
	SpriteRenderer spriteRenderer;

    void Start()
    {
		spriteRenderer = GetComponent<SpriteRenderer>();
		menuScale = transform.localScale;
		MenuLocalPosition = transform.localPosition;
		parentPicture = transform.parent;
		menuSortingOrder = spriteRenderer.sortingOrder;
		outline = GetComponent<Outline>();
	}

	public void PrepareForMove()
	{
		spriteRenderer.sortingOrder = SelectedSortingOrder;
		SetSelected(false);
	}

	public IEnumerator ScaleUp()
	{
		float timePassed = 0;

		while (timePassed < ScaleUpAnimTime)
		{
			float t = timePassed / ScaleUpAnimTime;

			transform.localScale = Vector3.Lerp(menuScale, scaledUpScale, t);

			timePassed += Time.deltaTime;
			yield return null;
		}

		transform.localScale = scaledUpScale;
	}

	public IEnumerator ScaleDown()
	{
		float timePassed = 0;

		while (timePassed < ScaleUpAnimTime)
		{
			float t = timePassed / ScaleUpAnimTime;

			transform.localScale = Vector3.Lerp(scaledUpScale, menuScale, t);

			timePassed += Time.deltaTime;
			yield return null;
		}

		transform.localScale = menuScale;
	}

	public void SetCompleted(bool completed)
	{
		if (completed)
			spriteRenderer.material = GamePrefs.instance.originalMat;
		else
			spriteRenderer.material = GamePrefs.instance.greyScaleMat;
	}

	public void SetSelected(bool isSelected)
	{
		if(this.isSelected == isSelected)
			return;

		this.isSelected = isSelected;

		if (isSelected)
			OutlineEffect.Instance?.AddOutline(outline);
		else
			OutlineEffect.Instance?.RemoveOutline(outline);
	}

	public void ResetOnMenu()
	{
		transform.parent = parentPicture;
		spriteRenderer.sortingOrder = menuSortingOrder;
	}
}
