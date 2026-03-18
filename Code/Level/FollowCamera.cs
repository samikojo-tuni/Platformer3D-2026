using Godot;
using System;

namespace GA.Platformer3D
{
	public partial class FollowCamera : Camera3D
	{
		// The node to follow
		[Export] private Node3D _target = null;
		// How far the camera should be from the followed node.
		[Export] private Vector3 _offset = new Vector3(0, 5, -5);
		// Move speed for smoothing the movement.
		[Export] private float _smoothSpeed = 5f;
		[Export] private float _shakeIntensity = 0f;

		private float _shakeTimer = 0f;
		private IHealth _playerHealth = null;

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			if (_target == null)
			{
				GD.PushError("FollowCamera: Target is not assigned.");
			}
		}

		public override void _EnterTree()
		{
			Callable.From(SubscribeToEvents).CallDeferred();
		}

		public override void _ExitTree()
		{
			if (_playerHealth != null)
			{
				_playerHealth.HealthChanged -= OnPlayerHealthChanged;
			}
		}

		private void SubscribeToEvents()
		{
			_playerHealth = LevelManager.Active.PlayerCharacter.Health;
			_playerHealth.HealthChanged += OnPlayerHealthChanged;
		}

		private void OnPlayerHealthChanged(int previousHealth, int currentHealth)
		{
			if (currentHealth < previousHealth)
			{
				// TODO: Loose the magic number!
				Shake(0.2f);
			}
		}

		// Called every frame. 'delta' is the elapsed time since the previous frame.
		public override void _Process(double delta)
		{
			if (_target == null)
			{
				return;
			}

			Vector3 desiredPosition = _target.GlobalPosition + _offset;
			GlobalPosition = GlobalPosition.Lerp(desiredPosition, _smoothSpeed * (float)delta);

			if (_shakeTimer > 0)
			{
				_shakeTimer -= (float)delta;

				HOffset = (float)GD.RandRange(-_shakeIntensity, _shakeIntensity);
				VOffset = (float)GD.RandRange(-_shakeIntensity, _shakeIntensity);

				if (_shakeTimer <= 0)
				{
					HOffset = 0;
					VOffset = 0;
				}
			}
		}

		public void Shake(float duration)
		{
			_shakeTimer = duration;
		}
	}
}