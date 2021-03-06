using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static GameHandler;

public class Cutter : MonoBehaviourSingleton<Cutter>
{
	public delegate void FreezeCountChangeHandler(int newValue);
	public event FreezeCountChangeHandler FreezeCountChanged;
	const float VibrateCycleTime = 1f;

	[SerializeField]
	float speed = 0.01f;
	[SerializeField]
	float rotationAngle = 1f;
	[SerializeField]
	float freezeDuration = 2f;

	float speedModifier = 1f;

	int rotateDirection;
	bool rotating;

	bool isFreezed;
	public int FreezeCount { get; private set; } = 10;

	float lastVibrate = VibrateCycleTime;

	Bounds cuttingArea;
	Transform directionChild;

	void Start()
	{
		FreezeCountChanged += OnFreezeCountChanged;
		cuttingArea = GameObject.Find("CuttingTable").GetComponent<BoxCollider2D>().bounds;
		directionChild = transform.Find("Direction");

		GameHandler.instance.GameStateChanged += OnGameStateChanged;
	}

	private void OnGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.Start:
				SetVisualsEnabled(false);
				break;
			case GameState.MainMenuZoomOut:
				break;
			case GameState.InGame:
				break;
			default:
				break;
		}
	}

	private void Update()
	{
#if UNITY_EDITOR
		if (Input.GetKeyDown(KeyCode.LeftArrow))
			StartRotation(1);
		if (Input.GetKeyDown(KeyCode.RightArrow))
			StartRotation(-1);

		if (Input.GetKeyUp(KeyCode.LeftArrow) || Input.GetKeyUp(KeyCode.RightArrow))
			EndRotation();

		if (Input.GetKeyDown(KeyCode.UpArrow))
			speedModifier = 5f;

		if (Input.GetKeyUp(KeyCode.UpArrow))
			speedModifier = 1f;

		if (Input.GetKeyDown(KeyCode.Space))
			Freeze();
#endif
	}

	void FixedUpdate()
    {
		if (!CuttingTable.instance.InGameCutting)
			return;

		if (!isFreezed)
			Move();

		if (rotating)
			Rotate();
	}

	public void Init(LevelData currentLevelData)
	{
		FreezeCount = currentLevelData.freezeCount;
		transform.localPosition = currentLevelData.startingPosition;
		transform.eulerAngles = new Vector3(0, 0, currentLevelData.startingRotation);

		SetVisualsEnabled(true);

		FreezeCountChanged(FreezeCount);
	}

	void SetVisualsEnabled(bool enabled)
	{
		GetComponent<SpriteRenderer>().enabled = enabled;
		directionChild.GetComponent<SpriteRenderer>().enabled = enabled;		
	}

	private void Move()
	{
		lastVibrate += Time.deltaTime;

		if(lastVibrate >= VibrateCycleTime)
		{
			//Handheld.Vibrate();
			lastVibrate = 0;
		}

		transform.Translate(Vector3.up * speed * speedModifier);

		LimitInCuttingArea();
	}

	private void LimitInCuttingArea()
	{
		float x = Mathf.Clamp(transform.position.x, cuttingArea.center.x - cuttingArea.extents.x, cuttingArea.center.x + cuttingArea.extents.x);
		float y = Mathf.Clamp(transform.position.y, cuttingArea.center.y - cuttingArea.extents.y, cuttingArea.center.y + cuttingArea.extents.y);

		transform.position = new Vector2(x, y);
	}

	void Rotate()
	{
		transform.Rotate(0, 0, transform.rotation.eulerAngles.x + rotationAngle * rotateDirection);
	}

	public void StartRotation(int rotateDirection)
	{
		this.rotateDirection = rotateDirection;
		rotating = true;
	}

	public void EndRotation()
	{
		CorrectRotation();
		rotating = false;
	}

	private void CorrectRotation()
	{
		float remainder = transform.eulerAngles.z % 2;
		int newRotationZ = remainder < 1f ? 
			Mathf.CeilToInt(transform.eulerAngles.z - remainder) : 
			Mathf.CeilToInt(transform.eulerAngles.z - remainder) + 2;

		transform.eulerAngles = new Vector3(0, 0, newRotationZ);
	}

	public void Freeze()
	{
		if (isFreezed || FreezeCount == 0)
			return;

		isFreezed = true;

		FreezeCount--;
		FreezeCountChanged(FreezeCount);

		StartCoroutine(UnFreezeAfterTimeOutRoutine());
	}

	private IEnumerator UnFreezeAfterTimeOutRoutine()
	{
		yield return new WaitForSeconds(freezeDuration);
		isFreezed = false;
	}

	private void OnFreezeCountChanged(int newValue)
	{
		//dummy
	}
}
