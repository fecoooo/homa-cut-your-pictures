using cakeslice;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
	const float ScaleUpAnimTime = 0.3f;

	Vector3 originalScale;
	readonly Vector3 scaledUpScale = new Vector3(1,1,1);
	Outline outline;

    void Start()
    {
		originalScale = transform.localScale;
		outline = GetComponent<Outline>();
	}

	public IEnumerator ScaleUp()
	{
		float timePassed = 0;

		while (timePassed < ScaleUpAnimTime)
		{
			float t = timePassed / ScaleUpAnimTime;

			transform.localScale = Vector3.Lerp(originalScale, scaledUpScale, t);

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

			transform.localScale = Vector3.Lerp(scaledUpScale, originalScale, t);

			timePassed += Time.deltaTime;
			yield return null;
		}

		transform.localScale = originalScale;
	}

	public void SetGreyEnabled(bool enabled)
	{
		if (enabled)
			GetComponent<SpriteRenderer>().material = GamePrefs.instance.originalMat;
		else
			GetComponent<SpriteRenderer>().material = GamePrefs.instance.greyScaleMat;
	}

	public void SetOutlineActive(bool isActive)
	{
		if(isActive)
			OutlineEffect.Instance?.AddOutline(outline);
		else
			OutlineEffect.Instance?.RemoveOutline(outline);
	}
}
