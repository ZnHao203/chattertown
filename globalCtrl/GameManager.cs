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
		ChatBox.Instance.AddMessage("SYSTEM", $"Starting day {CurrentDay}");
		EmitSignal(SignalName.DayStarted, CurrentDay, CurrentEnergy);

		// Handle different days
		switch (CurrentDay)
		{
			case 1:
				HandleDay1();
				break;
			// Add more cases for other days
		}
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

	private ConfigFile _config = new ConfigFile();
    private const string SAVE_PATH = "user://game_data.cfg";
    private const string TALKED_TO_SECTION = "talked_to_npcs";
	public bool HasTalkedTo(string npcName)
    {
        LoadConfig();
        return _config.HasSectionKey(TALKED_TO_SECTION, npcName);
    }

    public void RecordCharacterInteraction(string npcName)
    {
        LoadConfig();
        _config.SetValue(TALKED_TO_SECTION, npcName, true);
        SaveConfig();
    }

    private void LoadConfig()
    {
        Error error = _config.Load(SAVE_PATH);
        if (error != Error.Ok)
        {
            GD.Print("No saved data found. Creating new save file.");
            SaveConfig(); // Create a new save file if none exists
        }
    }

    private void SaveConfig()
    {
        Error error = _config.Save(SAVE_PATH);
        if (error != Error.Ok)
        {
            GD.PrintErr("Failed to save game data!");
        }
    }
	// DAY 1 specific
	public bool IsHomeDoorUnlocked { get; private set; } = false;
	public bool IsCutscenePlayed { get; set; } = false;
	private void HandleDay1()
	{
		// Switch to home scene
		GetTree().ChangeSceneToFile("res://scenes/home/home.tscn");
		
		// Add any Day 1 specific dialogue or setup
		DisplayDialogue("SYSTEM", "Good morning, starshine. The Earth says hello!");
	}

	// Track collected items
    public bool HasMeat { get; private set; } = false;


    public void CollectMeat()
    {
        HasMeat = true;
        CheckDoorUnlock();
    }

    private void CheckDoorUnlock()
    {
        if (HasMeat && !IsHomeDoorUnlocked)
        {
            IsHomeDoorUnlocked = true;
			EmitSignal(SignalName.DoorUnlocked);
        }
    }

    [Signal]
    public delegate void DoorUnlockedEventHandler();

}
