using Godot;
using System;
using System.IO;

namespace GA.Platformer3D.Save
{
	public enum SaveBackend
	{
		None = 0,
		Json,
		Binary,
		Mock
	}

	public class SaveManager
	{
		private SaveBackend _backend = SaveBackend.None;
		private string _saveLocation = null;
		private ISaveSerializer _serializer = null;

		public SaveManager(SaveConfig config)
		{
			_saveLocation = config.SaveLocation;
			SetBackend(config.DefaultSaveBackend);
		}

		#region Public API

		public void SetBackend(SaveBackend backend)
		{
			_backend = backend;
			_serializer = CreateSerializer(backend);
			GD.Print($"[SaveManager] Backend switched to {backend}.");
		}

		/// <summary>
		/// Snapshot the current game state and write it to disk.
		/// </summary>
		/// <param name="saveSlotName">Save slot identifier, used as the filename.</param>
		public void Save(string saveSlotName)
		{
			// Collect the save data.
			GameData data = CollectData();

			// Form the save file path.
			string path = BuildPath(saveSlotName);

			EnsureDirectoryExists(path);

			using (FileStream fileStream = File.Open(path, FileMode.Create, System.IO.FileAccess.Write))
			{
				_serializer.Serialize(fileStream, data);
				GD.Print($"[SaveManager] Game saved to {path} using {_backend}.");
			}
		}

		/// <summary>
		/// Load a previously saved slot from disk and apply its state to the game objects.
		/// </summary>
		/// <param name="saveSlotName">Save slot identifier matching the one used in <see cref="Save"/>.</param>
		/// <returns><c>true</c> if the file was found and applied; <c>false</c> if no save exists.</returns>
		public bool Load(string saveSlotName)
		{
			string path = BuildPath(saveSlotName);

			if (!File.Exists(path))
			{
				GD.Print($"[SaveManager] No save file found at {path}.");
				return false;
			}

			using (FileStream fileStream = File.Open(path, FileMode.Open, System.IO.FileAccess.Read))
			{
				GameData gameData = _serializer.Deserialize(fileStream);
				ApplyState(gameData);
				GD.Print($"[SaveManager] Game loaded from {path} using {_backend}.");
			}

			return true;
		}

		#endregion Public API

		#region Internal functionality
		private static GameData CollectData()
		{
			GameData data = new GameData()
			{
				Score = GameManager.Instance.Score,
				PlayerData = LevelManager.Active.PlayerCharacter.SaveState()
			};

			// TODO: Collect enemy data.

			return data;
		}

		private static void ApplyState(GameData data)
		{
			throw new NotImplementedException();
		}

		private string BuildPath(string saveSlotName)
		{
			string directory = ProjectSettings.GlobalizePath(_saveLocation);
			return Path.Combine(directory, $"{saveSlotName}.{_serializer.FileExtension}");
		}

		private static void EnsureDirectoryExists(string path)
		{
			string directory = Path.GetDirectoryName(path);
			if (directory != null && !Directory.Exists(directory))
			{
				Directory.CreateDirectory(directory);
			}
		}

		private static ISaveSerializer CreateSerializer(SaveBackend backend)
		{
			switch (backend)
			{
				case SaveBackend.Mock:
					return new MockSerializer();
				case SaveBackend.Json:
				case SaveBackend.Binary:
				case SaveBackend.None:
				default:
					return null;
			}
		}
		#endregion
	}
}