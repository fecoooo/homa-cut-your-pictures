using TMPro;
using UnityEngine;

public class UIHandler : MonoBehaviour
{
	TextMeshProUGUI freezeCountLbl;

	void Start()
    {
		freezeCountLbl = transform.Find("Freeze/FreezeCountLbl").GetComponent<TextMeshProUGUI>();

		Cutter.instance.FreezeCountChanged += OnFreezeCountChanged;
	}

    void Update()
    {
        
    }

	void OnFreezeCountChanged(int newValue)
	{
		freezeCountLbl.text = newValue + "x";
	}
}
