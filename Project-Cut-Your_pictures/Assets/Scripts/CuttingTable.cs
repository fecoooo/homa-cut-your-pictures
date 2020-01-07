using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CuttingTable : MonoBehaviour
{
	Template template;

    void Start()
    {
		template = transform.Find("Template").GetComponent<Template>();
		template.LoadImage("Levels/template_1");
    }

    void Update()
    {
        
    }
}
