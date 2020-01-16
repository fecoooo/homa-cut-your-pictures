using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Quad : MonoBehaviour
{
	Texture2D currentTexture;
	readonly Color Alpha = new Color(0, 0, 0, 0);

	void Start()
    {
		Texture2D loadedTexture = Resources.Load<Texture2D>("texture");
		currentTexture = new Texture2D(loadedTexture.width, loadedTexture.height);
		currentTexture.SetPixels(loadedTexture.GetPixels());
		GetComponent<Renderer>().material.mainTexture = currentTexture;
	}

    void Update()
    {
		for (int i = 0; i < 1000; ++i)
		{
			Vector2Int randomPos = new Vector2Int(Random.Range(0, currentTexture.width), Random.Range(0, currentTexture.height));
			Cut(randomPos);
		}
		currentTexture.Apply();
	}

	void Cut(Vector2Int cutPosition)
	{
		currentTexture.SetPixel(cutPosition.x, cutPosition.y, Alpha);
	}
}
