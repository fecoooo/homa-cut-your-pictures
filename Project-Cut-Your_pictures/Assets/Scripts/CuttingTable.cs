using System.Collections;
using UnityEngine;
using static GameHandler;

public class CuttingTable : MonoBehaviourSingleton<CuttingTable>
{
	public const float CountDownTime = 4f;
	const float ScaleUpAnimTime = .3f;

	public GameObject CuttingUI;

	public bool InGameCutting { get; private set; }
	public bool WonLast { get; private set; }

	Template template;
	SpriteRenderer piece;
	SpriteRenderer outline;

	LevelData currentLevelData;

	void Start()
    {
		template = transform.Find("Template").GetComponent<Template>();
		piece = transform.Find("Piece").GetComponent<SpriteRenderer>();
		outline = transform.Find("Outline").GetComponent<SpriteRenderer>();

		GameHandler.instance.GameStateChanged += OnGameStateChanged;
    }

	private void OnGameStateChanged(GameState state)
	{
		switch (state)
		{
			case GameState.TransferringPiece:
				transform.localScale = Vector3.zero;
				outline.enabled = false;
				LoadLevel();
				break;
			case GameState.BeforeGame:
				outline.enabled = true;
				InitTable();
				break;
			case GameState.InGame:
				StartGame();
				break;
			default:
				break;
		}
	}

	public void Restart()
	{
		StartGame();
	}

	void StartGame()
	{
		WonLast = false;
		InGameCutting = false;
		
		Cutter.instance.Init(currentLevelData);
		StartCoroutine(CountDownThanStart());
	}

	private IEnumerator CountDownThanStart()
	{
		yield return new WaitForSeconds(CountDownTime);
		InGameCutting = true;
	}

	void LoadLevel()
	{
		currentLevelData = GamePrefs.instance.LevelDatas[GameHandler.instance.CurrentSelectedPiece];

		Cutter.instance.Init(currentLevelData);
		
		template.Init(currentLevelData.templatePath, currentLevelData.minimumPixelToCut);
		piece.sprite = Resources.Load<Sprite>(currentLevelData.piecePath);
		outline.sprite = Resources.Load<Sprite>(currentLevelData.outlinePath);
	}

	public void Fail()
	{
		InGameCutting = false;
		Debug.Log("You cut into the Piece :(((((((");
	}

	public void EndedCircle(int pixelsCut)
	{
		if (pixelsCut >= currentLevelData.minimumPixelToCut)
		{
			WonLast = true;
			GameHandler.instance.GameWon();
		}
		else
			GameHandler.instance.GameLost();

		InGameCutting = false;
	}

	public void InitTable()
	{
		StartCoroutine(InitTableRoutine());
	}

	IEnumerator InitTableRoutine()
	{
		yield return ScaleUp();
	}

	public IEnumerator ScaleUp()
	{
		float timePassed = 0;

		while (timePassed < ScaleUpAnimTime)
		{
			float t = timePassed / ScaleUpAnimTime;

			transform.localScale = Vector3.Lerp(Vector3.zero, Vector3.one, t);

			timePassed += Time.deltaTime;
			yield return null;
		}

		transform.localScale = Vector3.one;
	}
}
