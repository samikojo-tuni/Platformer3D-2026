using Godot;
using System;

namespace GA.Platformer3D.UI
{
	public partial class SaveSlotList : VBoxContainer
	{
		[Signal] public delegate void SlotSelectedEventHandler(string slotName);

		public string SelectedSlot { get; private set; }

		public void Refresh()
		{
			foreach (Node child in GetChildren())
			{
				child.QueueFree();
			}

			SelectedSlot = null;

			ButtonGroup slotButtons = new ButtonGroup();

			string[] slots = GameManager.Instance.SaveManager.ListSaves();
			foreach (string slot in slots)
			{
				string capturedSlot = slot;
				Button button = new Button()
				{
					Text = capturedSlot,
					ToggleMode = true,
					ButtonGroup = slotButtons
				};
				button.Pressed += () => OnSlotButtonPressed(capturedSlot);
				AddChild(button);
			}
		}

		private void OnSlotButtonPressed(string slotName)
		{
			SelectedSlot = slotName;
			EmitSignal(SignalName.SlotSelected, slotName);
		}
	}
}