using System.IO;
using Godot;

namespace GA.Platformer3D.Save
{
	public class MockSerializer : ISaveSerializer
	{
		public string FileExtension => "mock";

		public GameData Deserialize(Stream stream)
		{
			return new GameData();
		}

		public void Serialize(Stream stream, GameData data)
		{
			GD.Print($"[MockSerializer] Saving data {data}");
		}
	}
}