using GA.Platformer3D;
using GA.Platformer3D.Save;
using GA.Platformer3D.UI;
using Godot;
using System;

public partial class EnemyCharacter : Character, ISaveable<EnemyData>
{
	[Export] private HealthBar _healthBar = null;

	public override void _Ready()
	{
		// Here, this is a must! If this is left out, the Health would not get initialized!
		base._Ready();

		AddToGroup(GameManager.Instance.SaveManager.EnemyGroupName);
	}


	public void LoadState(EnemyData state)
	{
		// TODO: Add support for restoring dead enemies. Hide them maybe?
		GlobalPosition = new Vector3(state.PositionX, state.PositionY, state.PositionZ);
		Health.Restore(state.CurrentHealth);
	}

	public EnemyData SaveState()
	{
		return new EnemyData()
		{
			Id = GetPath().ToString(),
			PositionX = GlobalPosition.X,
			PositionY = GlobalPosition.Y,
			PositionZ = GlobalPosition.Z,
			CurrentHealth = Health?.CurrentHP ?? 0
		};
	}

}
