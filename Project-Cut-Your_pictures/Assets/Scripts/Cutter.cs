using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviourSingleton<Cutter>
{
	public delegate void FreezeCountChangeHandler(int newValue);
	public event FreezeCountChangeHandler FreezeCountChanged;

	public float speed = 0.01f;
	public float rotationAngle = 1f;
	public float FreezeDuration = 2f;

	Vector2 direction = Vector2.right;
	int rotateDirection;
	bool rotating;

	bool isFreezed;
	public int FreezeCount { get; private set; } = 10;

	Bounds cuttingArea;


	private void Start()
	{
		cuttingArea = GameObject.Find("CuttingTable").GetComponent<BoxCollider2D>().bounds;
		FreezeCountChanged += OnFreezeCountChanged;
	}

	void Update()
    {
		if(!isFreezed)
			Move();

		if (rotating)
			Rotate();
	}

	private void Move()
	{
		transform.Translate(Vector3.up * speed);

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
		rotating = false;
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
		yield return new WaitForSeconds(FreezeDuration);
		isFreezed = false;
	}

	private void OnFreezeCountChanged(int newValue)
	{
		//dummy
	}
}
