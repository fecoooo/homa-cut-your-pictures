using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
	public GameObject[] imagesToFocus;
	int currentImageIndex = -1;

    void Start()
    {
        
    }

    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			currentImageIndex++;
			bool shouldUseZoom = currentImageIndex == 0 ? true : false;
			CameraController.instance.FocusImage(imagesToFocus[currentImageIndex].transform.position, shouldUseZoom);
		}

		if (Input.GetKeyDown(KeyCode.LeftShift))
		{
			CameraController.instance.MovePieceToCuttingTable(imagesToFocus[currentImageIndex].transform);
		}

		if (Input.GetKeyDown(KeyCode.LeftControl))
		{
			CameraController.instance.MovePieceBackToPicture();
		}
	}
}
