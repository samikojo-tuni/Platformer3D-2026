using Godot;
using System;
using System.Text.RegularExpressions;

namespace GA.Platformer3D.UI
{
	public partial class SaveWindow : Window
	{
		[Export] private LineEdit _fileNameInput;
		[Export] private SaveSlotList _slotList;
		[Export] private Button _saveButton;
		[Export] private Button _cancelButton;

		private static readonly Regex _validSlotName = new(@"^[a-zA-Z0-9_\-]+$");

		// Called when the node enters the scene tree for the first time.
		public override void _Ready()
		{
			_slotList.SlotSelected += OnSlotSelected;
			_fileNameInput.TextChanged += _ => UpdateSaveButtonState();

			_saveButton.Pressed += OnSavePressed;
			_cancelButton.Pressed += Hide;
			CloseRequested += Hide;

			UpdateSaveButtonState();
		}

		public void Open()
		{
			_fileNameInput.Text = string.Empty;
			_slotList.Refresh();
			UpdateSaveButtonState();
			Show();
		}

		private void OnSavePressed()
		{
			string slotName = _fileNameInput.Text.Trim();
			if (!_validSlotName.IsMatch(slotName))
			{
				GD.PrintErr("[SaveWindow] Invalid slot name. Use only letters, numbers, _ or -.");
				return;
			}

			GameManager.Instance.SaveManager.Save(slotName);
			Hide();
		}


		private void UpdateSaveButtonState()
		{
			_saveButton.Disabled = !_validSlotName.IsMatch(_fileNameInput.Text.Trim());
		}


		private void OnSlotSelected(string slotName)
		{
			_fileNameInput.Text = slotName;
			UpdateSaveButtonState();
		}
	}
}