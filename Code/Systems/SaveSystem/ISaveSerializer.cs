using System.IO;

namespace GA.Platformer3D.Save
{
	/// <summary>
	/// Strategy interface for serializing and deserializing game save data.
	/// Swap implementations at runtime to switch between different formats.
	/// </summary>
	public interface ISaveSerializer
	{
		/// <summary>
		/// File extension used for save files produced by this serializer (without the dot).
		/// </summary>
		string FileExtension { get; }

		/// <summary>
		/// Write <paramref name="data"/> to <paramref name="stream"/>.
		/// </summary>
		void Serialize(Stream stream, GameData data);

		/// <summary>
		/// Read and return a <see cref="GameData"/> from <paramref name="stream"/>.
		/// </summary>
		GameData Deserialize(Stream stream);
	}
}