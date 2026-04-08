using System.IO;
using System.Text;

namespace GA.Platformer3D.Save
{
	/// <summary>
	/// Saves game data in a compact binary format using BinaryWriter / BinaryReader.
	/// Faster to read/write and smaller on disk than JSON, but not human-readable.
	///
	/// Format layout (fixed order — must match between Serialize and Deserialize):
	///   [int32]  format version
	///   [int32]  score
	///   [float]  player position X
	///   [float]  player position Y
	///   [float]  player position Z
	///   [int32]  player current health
	///   [int32]  enemy count
	///   For each enemy:
	///     [string] enemy id (node path)
	///     [float]  enemy position X
	///     [float]  enemy position Y
	///     [float]  enemy position Z
	///     [int32]  current health
	///
	/// NOTE: Bump <see cref="FormatVersion"/> and add a migration path whenever
	/// the layout changes, so older save files remain loadable.
	/// </summary>
	public class BinarySaveSerializer : ISaveSerializer
	{
		private const int FormatVersion = 2;

		public string FileExtension => "sav";

		public GameData Deserialize(Stream stream)
		{
			using (BinaryReader reader = new BinaryReader(stream, Encoding.UTF8, leaveOpen: true))
			{
				int version = reader.ReadInt32();

				// Handle different format versions
				if (version == 1)
				{
					// Load data using v1 loading method.
					return DeserializeV1(reader);
				}
				else if (version == 2)
				{
					return DeserializeV2(reader);
				}
				else
				{
					throw new InvalidDataException($"Unsupported save format version {version}. Expected {FormatVersion}.");
				}
			}
		}

		public void Serialize(Stream stream, GameData data)
		{
			using (BinaryWriter writer = new BinaryWriter(stream, Encoding.UTF8, leaveOpen: true))
			{
				// Important to write the version first. We need this information before we can deserialize anything else.
				writer.Write(FormatVersion);
				writer.Write(data.Score);

				// PlayerData
				writer.Write(data.PlayerData.PositionX);
				writer.Write(data.PlayerData.PositionY);
				writer.Write(data.PlayerData.PositionZ);
				writer.Write(data.PlayerData.CurrentHealth);

				writer.Write(data.EnemyDataCollection.Count);
				foreach (EnemyData item in data.EnemyDataCollection)
				{
					writer.Write(item.Id);
					writer.Write(item.PositionX);
					writer.Write(item.PositionY);
					writer.Write(item.PositionZ);
					writer.Write(item.CurrentHealth);
				}
			}
		}

		private GameData DeserializeV1(BinaryReader reader)
		{
			return new GameData()
			{
				Score = reader.ReadInt32(),
				PlayerData = new PlayerData()
				{
					PositionX = reader.ReadSingle(),
					PositionY = reader.ReadSingle(),
					PositionZ = reader.ReadSingle(),
					CurrentHealth = reader.ReadInt32()
				}
			};
		}

		private GameData DeserializeV2(BinaryReader reader)
		{
			GameData data = DeserializeV1(reader);

			int enemyCount = reader.ReadInt32();
			for (int i = 0; i < enemyCount; ++i)
			{
				EnemyData enemyData = new EnemyData()
				{
					Id = reader.ReadString(),
					PositionX = reader.ReadSingle(),
					PositionY = reader.ReadSingle(),
					PositionZ = reader.ReadSingle(),
					CurrentHealth = reader.ReadInt32()
				};

				data.EnemyDataCollection.Add(enemyData);
			}

			return data;
		}
	}
}