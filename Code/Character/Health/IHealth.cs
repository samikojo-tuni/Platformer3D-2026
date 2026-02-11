namespace GA.Platformer3D
{

	public interface IHealth
	{
		// If you define the get accessor only, it leaves it up to the implementor 
		// if they want to add a public set accessor as well. Only get is required
		// by the interface.

		/// <summary>
		/// Returns the current HP amount.
		/// </summary>
		int CurrentHP { get; }

		int MaxHP { get; }

		int InitialHP { get; }

		bool IsAlive { get; }

		bool IsImmortal { get; }

		/// <summary>
		/// Increases hit points by ´amount´.
		/// </summary>
		/// <param name="amount">The amount of hit points to add.</param>
		void Heal(int amount);

		/// <summary>
		/// Decreases hit points, if the user is not immortal.
		/// </summary>
		/// <param name="amount">The amount of damage applied.</param>
		/// <returns>Wheter damage was applied or not.</returns>
		bool TakeDamage(int amount);
	}
}