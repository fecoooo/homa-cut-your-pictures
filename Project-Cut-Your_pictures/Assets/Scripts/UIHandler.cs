using TMPro;
using UnityEngine;
using UnityEngine.UI;
using static GameHandler;

public class UIHandler : MonoBehaviour
{
	TextMeshProUGUI freezeCountLbl;
	Image progressBar;
	TextMeshProUGUI progressLbl;
	TextMeshProUGUI countDownLbl;
	Canvas canvas;

	void Start()
    {
		canvas = GetComponent<Canvas>();

		freezeCountLbl = transform.Find("Freeze/FreezeCountLbl").GetComponent<TextMeshProUGUI>();
		progressBar = transform.Find("Progress/ProgressBar").GetComponent<Image>();
		progressLbl = transform.Find("Progress/ProgressLbl").GetComponent<TextMeshProUGUI>();
		countDownLbl = transform.Find("CountDownLbl").GetComponent<TextMeshProUGUI>();

		Cutter.instance.FreezeCountChanged += OnFreezeCountChanged;
		GameHandler.instance.GameStateChanged += OnGameStateChanged;
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

	void OnGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.Start:
				canvas.enabled = false;
				break;
			case GameState.MainMenu:
				canvas.enabled = false;
				break;
			case GameState.InGame:
				canvas.enabled = true;
				break;
			default:
				break;
		}
	}
}
