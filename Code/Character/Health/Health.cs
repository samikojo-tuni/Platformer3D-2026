using Godot;

namespace GA.Platformer3D
{
	public partial class Health : Node, IHealth
	{
		[Signal]
		public delegate void HealthChangedEventHandler(int currentHP);

		private int _currentHP = 0;

		public int CurrentHP
		{
			get => _currentHP;
			set
			{
				_currentHP = Mathf.Clamp(value, 0, MaxHP);
				EmitSignal(SignalName.HealthChanged, _currentHP);
			}
		}

		public int MaxHP => throw new System.NotImplementedException();

		public bool IsAlive => throw new System.NotImplementedException();

		public bool IsImmortal => throw new System.NotImplementedException();

		public void Heal(int amount)
		{
			throw new System.NotImplementedException();
		}

		public bool TakeDamage(int amount)
		{
			throw new System.NotImplementedException();
		}
	}
}