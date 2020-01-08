using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuHandler : MonoBehaviour
{
	public GameObject[] pictures;
	int currentPictureIndex = 0;

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		if (Input.GetKeyDown(KeyCode.Space))
		{
			CameraController.instance.FocusImage(pictures[currentPictureIndex].transform.position);
			currentPictureIndex++;
		}
    }
}
