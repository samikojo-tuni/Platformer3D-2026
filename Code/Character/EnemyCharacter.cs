using GA.Platformer3D;
using GA.Platformer3D.UI;
using Godot;
using System;

public partial class EnemyCharacter : Character
{
	[Export] private HealthBar _healthBar = null;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
