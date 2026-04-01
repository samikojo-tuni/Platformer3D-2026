using System;
using GA.Platformer3D.Messages;
using Godot;

namespace GA.Platformer3D
{
	public partial class Health : Node, IHealth
	{
		[Export]
		private int _maxHP = 10;

		[Export]
		private int _initialHP = 10;

		private int _currentHP = 0;

		public event Action<int, int> HealthChanged;

		public int CurrentHP
		{
			get => _currentHP;
			// TODO: Does this really need to be public? Probably not.
			set
			{
				int previousHP = _currentHP;
				_currentHP = Mathf.Clamp(value, 0, MaxHP);
				// Invoke the event if it is not null.
				// HealthChanged?.Invoke(previousHP, _currentHP);
				// Combine with the Pool
				HealthChangedMessage message = new HealthChangedMessage(this);
				LevelManager.Active.MessageBus.Send(message);
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

		public override void _Ready()
		{
			base._Ready();

			CurrentHP = InitialHP;
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