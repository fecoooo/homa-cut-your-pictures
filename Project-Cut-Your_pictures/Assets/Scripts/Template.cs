﻿using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Template : MonoBehaviourSingleton<Template>
{
	public bool visualize;
	readonly Color Alpha = new Color(0, 0, 0, 0);

	public float Progress
	{
		get => Mathf.Clamp01(pixelsCut / (float)minimumPixelToCut);
	}

	//int initialBlackPixels;
	//int currentBlackPixels;

	int pixelsCut;
	int minimumPixelToCut;

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

		spriteRenderer.enabled = visualize;
	}

	void Update()
    {
		if (!CuttingTable.instance.InGameCutting)
			return;

		DoCurrentCut();
	}

	private void DoCurrentCut()
	{
		if (currentTexture.GetPixel(CutterTexturePosition.x, CutterTexturePosition.y) == Color.black)
			pixelsCut++;
		else if (currentTexture.GetPixel(CutterTexturePosition.x, CutterTexturePosition.y) == Color.magenta)
			CuttingTable.instance.Fail();
		else if (currentTexture.GetPixel(CutterTexturePosition.x, CutterTexturePosition.y) == Color.green)
		{
			Debug.Log(pixelsCut + "/" + minimumPixelToCut);
			//Debug.Log(currentBlackPixels + "/" + initialBlackPixels);
			CuttingTable.instance.EndedCircle(pixelsCut);
		}
			

		currentTexture.SetPixel(CutterTexturePosition.x, CutterTexturePosition.y, Alpha);

		if (visualize)
			Visualize();
	}

	public void Init(string path, int minimumPixelToCut)
	{
		Texture2D loadedTexture2D = Resources.Load<Texture2D>(path);
		currentTexture = new Texture2D(loadedTexture2D.width, loadedTexture2D.height);
		currentTexture.SetPixels(loadedTexture2D.GetPixels());
		currentTexture.Apply();

		//foreach(Color c in currentTexture.GetPixels())
		//{
		//	if(c == Color.black)
		//		initialBlackPixels++;
		//}

		//currentBlackPixels = initialBlackPixels;
		this.minimumPixelToCut = minimumPixelToCut;
		pixelsCut = 0;
	}

	void Visualize()
	{
		currentTexture.Apply();
		spriteRenderer.sprite = Sprite.Create(currentTexture, spriteRenderer.sprite.rect, new Vector2(.5f, .5f));
	}
}
