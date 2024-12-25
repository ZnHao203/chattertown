using Godot;
using System;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }
		
	[Export]
	public int MaxEnergyPerDay { get; private set; } = 100;
	public int CurrentEnergy { get; private set; }
	public int CurrentDay { get; private set; } = 1;

	public override void _Ready()
	{
		
		if (Instance == null)
		{
			Instance = this;
		}
		// Debug check for ChatBox
		if (ChatBox.Instance == null)
		{
			GD.PrintErr("ChatBox not found in _Ready! This might be an autoload order issue.");
		}
		else
		{
			GD.Print("ChatBox found successfully");
		}
		GD.Print("GameManager Ready"); // Debug print
		StartNewDay();
	}
	
	public void DisplayDialogue(string speaker, string message)
	{
		GD.Print($"in gamemanagerAttempting to display dialogue: {speaker}: {message}"); // Debug print
		if (ChatBox.Instance == null)
		{
			GD.PrintErr("ChatBox instance is null!");
			return;
		}
		
		ChatBox.Instance.AddMessage(speaker, message);
	}

	public void StartNewDay()
	{
		CurrentEnergy = MaxEnergyPerDay;
		EmitSignal(SignalName.DayStarted, CurrentDay, CurrentEnergy);
	}

	public bool UseEnergy(int amount)
	{
		if (CurrentEnergy >= amount)
		{
			CurrentEnergy -= amount;
			EmitSignal(SignalName.EnergyChanged, CurrentEnergy);
			return true;
		}
		return false;
	}

	public void AdvanceToNextDay()
	{
		CurrentDay++;
		StartNewDay();
	}

	[Signal]
	public delegate void DayStartedEventHandler(int day, int energy);

	[Signal]
	public delegate void EnergyChangedEventHandler(int newEnergy);
}
