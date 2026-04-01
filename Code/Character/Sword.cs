using Godot;
using System;

using GA.Common;

namespace GA.Platformer3D
{
	public partial class Sword : MeshInstance3D
	{
		[Export] private int _damage = 1;
		private Area3D _damageArea = null;

		public override void _EnterTree()
		{
			base._EnterTree();

			if (_damageArea == null)
			{
				_damageArea = this.GetNode<Area3D>();

				if (_damageArea == null)
				{
					GD.PushWarning("Sword: The dependency Area3D can't be found!");
					return;
				}
			}

			_damageArea.BodyEntered += OnBodyEntered;
		}

		public override void _ExitTree()
		{
			base._ExitTree();

			if (_damageArea != null)
			{
				_damageArea.BodyEntered -= OnBodyEntered;
			}
		}


		private void OnBodyEntered(Node3D body)
		{
			if (body is Character character)
			{
				character.Health.TakeDamage(_damage);
			}
		}
	}
}