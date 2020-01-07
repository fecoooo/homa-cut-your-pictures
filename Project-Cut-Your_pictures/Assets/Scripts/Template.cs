using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template:MonoBehaviour
{
	readonly Color Alpha = new Color(0, 0, 0, 0);

	public float Progress
	{
		get => currentBlackPixels / (float)initialBlackPixels;
	}

	int initialBlackPixels;
	int currentBlackPixels;

	SpriteRenderer spriteRenderer;
	Texture2D currentTexture;

	Vector2 BotLeft_readonly;
	Vector2 Size_readonly;


	Vector2Int CutterTexturePosition
	{
		get
		{ 
			Vector2 cutterPos = Cutter.instance.transform.position;

			float x01 = (cutterPos.x - BotLeft_readonly.x) / Size_readonly.x;
			float y01 = (cutterPos.y - BotLeft_readonly.y) / Size_readonly.y;

			float x = x01 * currentTexture.width;
			float y = y01 * currentTexture.height;

			return new Vector2Int((int)x, (int)y);
		}
	}

	void Start()
	{
		spriteRenderer = GetComponent<SpriteRenderer>();

		Bounds bounds = GetComponent<SpriteRenderer>().bounds;
		BotLeft_readonly = bounds.center - bounds.extents;
		Size_readonly = bounds.size;
	}

	void Update()
    {
		DoCurrentCut();
	}

	private void DoCurrentCut()
	{
		if (currentTexture.GetPixel(CutterTexturePosition.x, CutterTexturePosition.y) == Color.black)
			currentBlackPixels--;

		currentTexture.SetPixel(CutterTexturePosition.x, CutterTexturePosition.y, Alpha);
		Visualize();
	}

	public void LoadImage(string path)
	{
		Texture2D loadedTexture2D = Resources.Load<Texture2D>(path);
		currentTexture = new Texture2D(loadedTexture2D.width, loadedTexture2D.height);
		currentTexture.SetPixels(loadedTexture2D.GetPixels());
		currentTexture.Apply();

		foreach(Color c in currentTexture.GetPixels())
		{
			if(c == Color.black)
				initialBlackPixels++;
		}

		currentBlackPixels = initialBlackPixels;
	}

	void Visualize()
	{
		currentTexture.Apply();
		spriteRenderer.sprite = Sprite.Create(currentTexture, spriteRenderer.sprite.rect, new Vector2(.5f, .5f));
	}
}
