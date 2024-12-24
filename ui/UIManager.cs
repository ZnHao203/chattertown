using Godot;
using System;

public partial class UIManager : Control
{
	
	private Label _energyLabel;
	private Label _dayLabel;
	private Button _nextDayButton;

	public override void _Ready()
	{
		_energyLabel = GetNode<Label>("EnergyLabel");
		_dayLabel = GetNode<Label>("DayLabel");
		_nextDayButton = GetNode<Button>("NextDayButton");

		// Connect to GameManager signals
		GameManager.Instance.Connect(GameManager.SignalName.DayStarted, 
			Callable.From((int day, int energy) => UpdateUI(day, energy)));
		
		GameManager.Instance.Connect(GameManager.SignalName.EnergyChanged,
			Callable.From((int energy) => UpdateEnergyDisplay(energy)));

		// Connect button click
		_nextDayButton.Pressed += OnNextDayPressed;
		
		// Initial update
		UpdateUI(GameManager.Instance.CurrentDay, GameManager.Instance.CurrentEnergy);
	}
	
	private void OnNextDayPressed()
	{
		GameManager.Instance.AdvanceToNextDay();
	}

	private void UpdateUI(int day, int energy)
	{
		_dayLabel.Text = $"Day: {day}";
		UpdateEnergyDisplay(energy);
	}

	private void UpdateEnergyDisplay(int energy)
	{
		_energyLabel.Text = $"Energy: {energy}/{GameManager.Instance.MaxEnergyPerDay}";
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
