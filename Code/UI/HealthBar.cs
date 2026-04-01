using Godot;

namespace GA.Platformer3D.UI
{
	public partial class HealthBar : CanvasLayer
	{
		[Export] private float _offsetHeight = -0.5f; // Height offset above the character
		[Export] private float _heartSize = 16f; // Size of each heart texture
		[Export] private float _heartSpacing = 2f; // Space between hearts
		[Export] private float _distanceFade = 50f; // Distance at which the hearts start to fade
		[Export] private Texture2D _heartTexture = null; // Texture for the heart icon

		private IHealth _health;
		private Node3D _parentNode;
		private Control _heartsContainer;
		private TextureRect[] _heartTextures;
		private Camera3D _camera;

		public override void _Ready()
		{
			base._Ready();

			// Get the parent character (3D node)
			_parentNode = GetParent<Node3D>();
			if (_parentNode == null)
			{
				GD.PrintErr("HealthBar must be a child of a Node3D");
				QueueFree();
				return;
			}

			_health = _parentNode.GetNode<Health>("Health");
			if (_health == null)
			{
				GD.PrintErr("HealthBar parent must have a Health child node");
				QueueFree();
				return;
			}

			if (_heartTexture == null)
			{
				GD.PrintErr($"Failed to load heart texture.");
				QueueFree();
				return;
			}

			// Subscribe to health changes
			_health.HealthChanged += OnHealthChanged;

			// Get the camera
			_camera = GetViewport().GetCamera3D();

			// Create the UI hierarchy
			SetupUI();
		}

		public override void _ExitTree()
		{
			base._ExitTree();

			if (_health != null)
			{
				_health.HealthChanged -= OnHealthChanged;
			}
		}

		public override void _Process(double delta)
		{
			base._Process(delta);

			if (_parentNode == null || _camera == null || _heartsContainer == null)
			{
				return;
			}

			// Calculate the world position for the hearts (above the character)
			Vector3 characterPos = _parentNode.GlobalPosition;
			Vector3 heartsWorldPos = characterPos + Vector3.Up * _offsetHeight;

			// Convert 3D world position to 2D screen position
			Vector2 screenPos = _camera.UnprojectPosition(heartsWorldPos);

			// Center the container on the screen position
			_heartsContainer.GlobalPosition = screenPos - (_heartsContainer.Size / 2);

			// Update visibility based on distance and field of view
			float distanceToCamera = _parentNode.GlobalPosition.DistanceTo(_camera.GlobalPosition);
			bool isBehindCamera = _camera.IsPositionBehind(heartsWorldPos);

			_heartsContainer.Visible = !isBehindCamera && distanceToCamera < _distanceFade;

			// Fade out with distance
			if (_heartsContainer.Visible)
			{
				float fadeStart = _distanceFade * 0.7f;
				float alpha = 1.0f;
				if (distanceToCamera > fadeStart)
				{
					alpha = 1.0f - ((distanceToCamera - fadeStart) / (_distanceFade - fadeStart));
				}
				_heartsContainer.Modulate = new Color(1, 1, 1, alpha);
			}
		}

		private void SetupUI()
		{
			// Create container for hearts
			_heartsContainer = new Control();
			_heartsContainer.MouseFilter = Control.MouseFilterEnum.Ignore;
			AddChild(_heartsContainer);

			// Create heart textures based on max health
			int maxHealth = _health.MaxHP;
			_heartTextures = new TextureRect[maxHealth];

			float containerWidth = (maxHealth * _heartSize) + ((maxHealth - 1) * _heartSpacing);
			_heartsContainer.CustomMinimumSize = new Vector2(containerWidth, _heartSize);

			for (int i = 0; i < maxHealth; i++)
			{
				TextureRect heart = new TextureRect();
				heart.Texture = _heartTexture;
				heart.CustomMinimumSize = new Vector2(_heartSize, _heartSize);
				heart.MouseFilter = Control.MouseFilterEnum.Ignore;
				heart.ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize;
				heart.StretchMode = TextureRect.StretchModeEnum.Scale;
				_heartsContainer.AddChild(heart);

				// Position horizontally
				heart.Position = new Vector2(i * (_heartSize + _heartSpacing), 0);
				_heartTextures[i] = heart;
			}

			UpdateHealthDisplay();
		}

		private void UpdateHealthDisplay()
		{
			if (_health == null || _heartTextures == null)
			{
				return;
			}

			int currentHealth = _health.CurrentHP;

			// Update heart visibility based on current health
			for (int i = 0; i < _heartTextures.Length; i++)
			{
				// Heart is visible if it's within current health
				_heartTextures[i].Visible = (i < currentHealth);
			}
		}

		private void OnHealthChanged(int previous, int current)
		{
			UpdateHealthDisplay();
		}
	}
}