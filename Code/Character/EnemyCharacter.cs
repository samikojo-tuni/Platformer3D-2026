using GA.Platformer3D;
using GA.Platformer3D.Save;
using GA.Platformer3D.UI;
using Godot;
using System;

public partial class EnemyCharacter : Character, ISaveable<EnemyData>
{
	[Export] private HealthBar _healthBar = null;

	public void LoadState(EnemyData state)
	{
		throw new NotImplementedException();
	}

	public EnemyData SaveState()
	{
		throw new NotImplementedException();
	}

}
