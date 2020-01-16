using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad : MonoBehaviour
{
	public int halfCutSize = 3;

	Texture2D currentTexture;
	readonly Color Alpha = new Color(0, 0, 0, 0);
	Vector2Int textureSize;
	

	void Start()
    {
		QualitySettings.vSyncCount = 0;

		Texture2D loadedTexture = Resources.Load<Texture2D>("texture");
		currentTexture = new Texture2D(loadedTexture.width, loadedTexture.height);
		currentTexture.SetPixels(loadedTexture.GetPixels());
		currentTexture.Apply();

		textureSize = new Vector2Int(currentTexture.width, currentTexture.height);

		GetComponent<Renderer>().material.mainTexture = currentTexture;

	}

	void Update()
    {
		if (Input.GetMouseButton(0))
		{
			RaycastHit hit;
			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			if (Physics.Raycast(ray, out hit, 100.0f))
			{
				Vector2 cutPosition = hit.textureCoord * textureSize;
				Cut(cutPosition.ToVector2Int());
			}
		}
	}


	void Cut(Vector2Int cutPosition)
	{
		for (int i = -halfCutSize; i < halfCutSize; ++i)
		{
			for (int j = -halfCutSize; j < halfCutSize; ++j)
			{
				currentTexture.SetPixel(cutPosition.x + i, cutPosition.y + j, Alpha);
			}
		}
		currentTexture.Apply();
	}
}
