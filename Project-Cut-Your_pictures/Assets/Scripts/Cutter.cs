using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cutter : MonoBehaviour
{
	public float speed = 0.01f;
	public float rotationAngle = 1f;

	Vector2 direction = Vector2.right;
	bool rotating;
	int rotateDirection;

	void Update()
    {
		Vector3 translation = transform.up * speed;

		transform.Translate(Vector3.up * speed);

		if (rotating)
			Rotate();
	}

	public void Rotate()
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
}
