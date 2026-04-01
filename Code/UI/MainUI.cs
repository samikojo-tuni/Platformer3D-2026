using GA.Common;
using GA.Common.Messaging;
using GA.Platformer3D.Messages;
using Godot;
using System;

namespace GA.Platformer3D.UI
{
	public partial class MainUI : CanvasLayer
	{
		[Export] private Container _heartsContainer = null;
		[Export] private Texture2D _heartTexture = null;

		private IHealth _playerHealth = null;

		public override void _EnterTree()
		{
			base._EnterTree();

			Callable.From(Initialize).CallDeferred();
		}

		public override void _ExitTree()
		{
			if (_playerHealth != null)
			{
				_playerHealth.HealthChanged -= UpdateHearts;
			}
		}

		private void Initialize()
		{
			_playerHealth = LevelManager.Active.PlayerCharacter.Health;
			InitializeHearts(_playerHealth);
			_playerHealth.HealthChanged += UpdateHearts;
		}

		public void InitializeHearts(IHealth health)
		{
			if (_heartsContainer == null || _heartTexture == null)
			{
				return;
			}

			// Delete existing children.
			foreach (Node child in _heartsContainer.GetChildren())
			{
				// TODO: Pool these?
				child.QueueFree();
			}

			for (int i = 0; i < health.MaxHP; ++i)
			{
				TextureRect heartRect = new TextureRect
				{
					Texture = _heartTexture
				};
				_heartsContainer.AddChild(heartRect);
			}

			UpdateHearts(health.CurrentHP, health.CurrentHP);
		}

		private void UpdateHearts(int previousHP, int currentHP)
		{
			if (_heartsContainer == null)
			{
				GD.PushError("MainUI: _heartsContainer is null!");
				return;
			}

			int index = 0;
			foreach (TextureRect heartRect in _heartsContainer.GetChildren<TextureRect>())
			{
				heartRect.Modulate = index < currentHP
					? Colors.White
					: new Color(1, 1, 1, 0.3f);

				index++;
			}
		}
	}
}