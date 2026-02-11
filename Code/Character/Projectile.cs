using Godot;
using System;

namespace GA.Platformer3D
{
	public partial class Projectile : Area3D
	{
		[Signal]
		public delegate void DisposeEventHandler(Projectile projectile);

		[Export] private float _speed = 10f;
		[Export] private float _damage = 10f;
		[Export] private float _aliveTime = 5f;

		private float _age = 0f;

		private Vector3 _direction = Vector3.Zero;

		public bool IsLaunched
		{
			get { return !_direction.IsZeroApprox(); }
		}

		public override void _EnterTree()
		{
			BodyEntered += OnBodyEntered;
		}

		public override void _ExitTree()
		{
			BodyEntered -= OnBodyEntered;
		}

		private void OnBodyEntered(Node3D body)
		{
			// TODO: Apply damage to the target if it was hit.
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if (IsLaunched)
			{
				_age += (float)delta;

				if (_age > _aliveTime)
				{
					Reset();
				}
			}
		}

		public override void _PhysicsProcess(double delta)
		{
			float deltaTime = (float)delta;
			if (IsLaunched)
			{
				GlobalPosition += _direction * _speed * deltaTime;
			}
		}

		public void Launch(Vector3 direction)
		{
			_direction = direction.Normalized();
		}

		public void Reset()
		{
			_direction = Vector3.Zero;
			_age = 0f;

			EmitSignal(SignalName.Dispose, this);
		}
	}
}