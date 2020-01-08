using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UIHandler : MonoBehaviour
{
	TextMeshProUGUI freezeCountLbl;
	Image progressBar;
	TextMeshProUGUI progressLbl;

	void Start()
    {
		freezeCountLbl = transform.Find("Freeze/FreezeCountLbl").GetComponent<TextMeshProUGUI>();
		progressBar = transform.Find("Progress/ProgressBar").GetComponent<Image>();
		progressLbl = transform.Find("Progress/ProgressLbl").GetComponent<TextMeshProUGUI>();

		Cutter.instance.FreezeCountChanged += OnFreezeCountChanged;
	}

    void Update()
    {
		if(CuttingTable.instance.InGameCutting)
			UpdateProgress();
    }

	void UpdateProgress()
	{
		progressBar.transform.localScale = new Vector3(Template.instance.Progress, 1, 1);
		progressLbl.text = Mathf.CeilToInt(Template.instance.Progress * 100) + "%";
	}

	void OnFreezeCountChanged(int newValue)
	{
		freezeCountLbl.text = newValue + "x";
	}
}
