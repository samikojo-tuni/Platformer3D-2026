using GA.Platformer3D.Save;
using Godot;
using System;

public partial class GameManager : Node
{
	public const string SaveConfigPath = "res://Config/SaveConfig.tres";
	#region Singleton
	public static GameManager Instance
	{
		get;
		private set;
	}

	public GameManager()
	{
		if (Instance == null)
		{
			Instance = this;
		}
		else if (Instance != this)
		{
			QueueFree();
			return;
		}

		Initialize();
	}
	#endregion

	private int _score = 0;

	public int Score
	{
		get { return _score; }
		private set
		{
			_score = Mathf.Max(value, 0);
			// TODO: Event or message
		}
	}

	public SaveManager SaveManager
	{
		get;
		private set;
	}

	private void Initialize()
	{
		SaveConfig saveConfig = GD.Load<SaveConfig>(SaveConfigPath);
		if (saveConfig != null)
		{
			SaveManager = new SaveManager(saveConfig);
		}
		else
		{
			GD.PushError($"[GameManager] Can't load SaveConfig from the path {SaveConfigPath}");
		}
	}
}
