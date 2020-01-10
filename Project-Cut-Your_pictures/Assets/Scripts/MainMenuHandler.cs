using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
	public GameObject[] imagesToFocus;
	int currentImageIndex = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			bool shouldUseZoom = currentImageIndex == 0 ? true : false;
			CameraController.instance.FocusImage(imagesToFocus[currentImageIndex].transform.position, shouldUseZoom);
			currentImageIndex++;
		}
    }
}
