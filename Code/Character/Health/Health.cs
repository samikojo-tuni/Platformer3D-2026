using Godot;

namespace GA.Platformer3D
{
	public partial class Health : Node, IHealth
	{
		[Signal]
		public delegate void HealthChangedEventHandler(int currentHP);

		[Export]
		private int _maxHP = 10;

		[Export]
		private int _initialHP = 10;

		private int _currentHP = 0;

		public int CurrentHP
		{
			get => _currentHP;
			// TODO: Does this really need to be public? Probably not.
			set
			{
				_currentHP = Mathf.Clamp(value, 0, MaxHP);
				EmitSignal(SignalName.HealthChanged, _currentHP);
			}
		}

		public int InitialHP => _initialHP;

		public int MaxHP => _maxHP;


		public bool IsAlive
		{
			get { return CurrentHP > 0; }
		}

		// TODO: Does setting value for this require some validation?
		public bool IsImmortal
		{
			get;
			set;
		}

		public void Heal(int amount)
		{
			if (amount < 0)
			{
				throw new System.ArgumentOutOfRangeException(nameof(amount), amount, "Heal doesn't support negative values.");
			}

			CurrentHP += amount;
		}

		public bool TakeDamage(int amount)
		{
			if (amount < 0)
			{
				throw new System.ArgumentOutOfRangeException(nameof(amount), amount, "TakeDamage doesn't support negative values.");
			}

			if (IsImmortal)
			{
				return false;
			}

			CurrentHP -= amount;
			return true;
		}
	}
}