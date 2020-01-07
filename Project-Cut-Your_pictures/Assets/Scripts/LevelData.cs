using UnityEngine;

public class LevelData
{
	public readonly int freezeCount;
	public readonly Vector2 startingPosition;
	public readonly int startingRotation;
	public readonly string templatePath;
	public readonly string piecePath;

	public LevelData(int freezeCount, Vector2 startingPosition, int startingRotation, string templatePath, string piecePath)
	{
		this.freezeCount		= freezeCount;
		this.startingPosition	= startingPosition;
		this.startingRotation	= startingRotation;
		this.templatePath		= templatePath;
		this.piecePath			= piecePath;
	}
}
