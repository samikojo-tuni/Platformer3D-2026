using Godot;

namespace GA.Platformer3D.Save
{
	[GlobalClass]
	public partial class SaveConfig : Resource
	{
		[Export] public SaveBackend DefaultSaveBackend = SaveBackend.None;
		[Export] public string SaveLocation = "user://save";
		[Export] public string EnemyGroupName = "enemies";
		[Export] public string QuickSaveSlot = "QuickSave";
	}
}