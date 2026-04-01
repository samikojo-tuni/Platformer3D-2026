using System.Collections.Generic;

namespace GA.Platformer3D
{
	public sealed class PlayerData
	{
		// TODO: Maybe add ID for the player as well.
		// TODO: Add saved player related data here!
		public float PositionX { get; set; }
		public float PositionY { get; set; }
		public float PositionZ { get; set; }
		// TODO: Store rotation
		public int CurrentHealth { get; set; }
	}

	public sealed class EnemyData
	{
		public string Id { get; set; }
		public float PositionX { get; set; }
		public float PositionY { get; set; }
		public float PositionZ { get; set; }
		// TODO: Store rotation
		public int CurrentHealth { get; set; }
	}

	public sealed class GameData
	{
		public int Score { get; set; }
		public PlayerData PlayerData { get; set; } = new();
		public List<EnemyData> EnemyDataCollection { get; set; } = new();
	}
}