using GA.Common.Messaging;
using GA.Platformer3D.UI;
using Godot;
using System;

namespace GA.Platformer3D
{
	public partial class LevelManager : Node3D
	{
		public static LevelManager Active { get; private set; }

		[Export] private bool _useProjectilePool = false;
		[Export] private PackedScene _projectileScene = null;
		[Export] private Node3D _projectileParent = null;
		[Export] private int _projectilePoolCapacity = 10;
		[Export] private bool _canGrow = false;
		[Export] private PlayerCharacter _player = null;
		[Export] private MainUI _mainUI = null;
		[Export] private FollowCamera _followCamera = null;

		private ProjectilePool _projectilePool = null;

		public PlayerCharacter PlayerCharacter => _player;
		public FollowCamera Camera => _followCamera;
		public MainUI MainUI => _mainUI;

		public MessageBus MessageBus
		{
			get;
			private set;
		}

		public bool UseProjectilePool
		{
			get => _useProjectilePool;
			set => _useProjectilePool = value;
		}

		public Node3D ProjectileParent
		{
			get => _projectileParent;
		}

		public override void _EnterTree()
		{
			// Sets itself as the active LevelManager
			Active = this;
		}

		public override void _ExitTree()
		{
			// Removes itself from the active LevelManager's role.
			Active = null;

			// Clean up the memory ProjectilePool has reserved.
			if (_projectilePool != null)
			{
				_projectilePool.Dispose();
				_projectilePool = null;
			}
		}

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			if (_projectileScene == null)
			{
				GD.PushError("Projectile scene is not assigned in the LevelManager.");
			}

			if (_projectileParent == null)
			{
				GD.PushError("The projectile parent is null!");
				_projectileParent = this; // TODO: Consider creating an empty node under LevelManager.
			}

			_projectilePool = new ProjectilePool(_projectileScene, _projectilePoolCapacity, _canGrow);

			MessageBus = new MessageBus();
		}

		public Projectile SpawnProjectile(Vector3 position, Vector3 direction,
			uint collisionLayer, uint collisionMask)
		{
			if (_projectileScene == null)
			{
				throw new InvalidOperationException("Projectile scene is not assigned.");
			}

			Projectile projectile;
			if (_useProjectilePool)
			{
				// TODO: Fetch the projectile from the pool
				projectile = _projectilePool.Get();
			}
			else
			{
				projectile = _projectileScene.Instantiate<Projectile>();
				_projectileParent.AddChild(projectile);
			}

			if (projectile != null)
			{
				projectile.CollisionLayer = collisionLayer;
				projectile.CollisionMask = collisionMask;

				projectile.GlobalPosition = position;
				// TODO: Discuss this later
				projectile.Dispose += OnDisposeProjectile;

				projectile.Launch(direction);
			}

			return projectile;
		}

		private void OnDisposeProjectile(Projectile projectile)
		{
			// Unsubscribe from the Dispose signal.
			projectile.Dispose -= OnDisposeProjectile;

			if (_useProjectilePool)
			{
				if (!_projectilePool.Return(projectile))
				{
					GD.PushError("Failed to return projectile to pool.");
				}
			}
			else
			{
				projectile.QueueFree();
			}
		}
	}
}