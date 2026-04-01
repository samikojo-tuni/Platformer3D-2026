namespace GA.Platformer3D.Save
{
	/// <summary>
	/// Contract for any game object that wants to participate in the save system.
	/// Implement this interface on nodes or components that own persistent state.
	///
	/// TData is a plain C# class (a "data container") containing only the fields that
	/// must survive between sessions — no Godot node references, no signals.
	/// </summary>
	public interface ISaveable<TData>
		where TData : class, new()
	{
		/// <summary>
		/// Snapshot the current runtime state into a serializable data object.
		/// </summary>
		TData SaveState();

		/// <summary>
		/// Apply a previously captured snapshot back to this object.
		/// </summary>
		void LoadState(TData state);
	}
}