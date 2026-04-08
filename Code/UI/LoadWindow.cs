using System;
using Godot;

namespace GA.Platformer3D.UI
{
	public partial class LoadWindow : Window
	{
		[Export] private SaveSlotList _slotList;
		[Export] private Button _loadButton;
		[Export] private Button _cancelButton;

		public override void _Ready()
		{
			// Keep Load button disabled until the player picks a slot.
			_slotList.SlotSelected += OnSlotPressed;

			_loadButton.Pressed += OnLoadPressed;
			_cancelButton.Pressed += Hide;
			CloseRequested += Hide;
		}

		private void OnSlotPressed(string slotName)
		{
			_loadButton.Disabled = slotName == null;
		}

		/// <summary>Open the window — refreshes the slot list and resets button state.</summary>
		public void Open()
		{
			_slotList.Refresh();
			_loadButton.Disabled = true;
			Show();
		}

		private void OnLoadPressed()
		{
			string slot = _slotList.SelectedSlot;
			if (slot == null)
			{
				return;
			}

			GameManager.Instance.SaveManager.Load(slot);
			Hide();
		}
	}
}