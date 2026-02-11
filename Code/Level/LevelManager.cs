using Godot;
using System;

namespace GA.Platformer3D
{
	public partial class LevelManager : Node3D
	{
		public static LevelManager Active { get; private set; }

		[Export] private bool _useProjectilePool = false;
		[Export] private PackedScene _projectileScene = null;

		public override void _EnterTree()
		{
			// Sets itself as the active LevelManager
			Active = this;
		}

		public override void _ExitTree()
		{
			// Removes itself from the active LevelManager's role.
			Active = null;
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{

		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
		}
	}
}