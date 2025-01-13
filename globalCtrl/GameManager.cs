using Godot;
using System;
using System.Collections.Generic;

public partial class GameManager : Node
{
	public static GameManager Instance { get; private set; }
		
	[Export]
	public int MaxEnergyPerDay { get; private set; } = 100;
	public int CurrentEnergy { get; private set; }
	public int CurrentDay { get; private set; } = 1;
    public bool isNight { get; private set; } = false;

	public override void _Ready()
	{
		// Clear dialogue history
        _config = new ConfigFile();  // Create new empty config
        SaveConfig();  // Save the empty config to clear old data
        
        // Reset current dialogue state
        currentDialogue = null;
        currentChoices = null;
        
        // Reset energy
        CurrentEnergy = MaxEnergyPerDay;
        
        // Reset current day
        CurrentDay = 1;
        
        // Reset Day 1 specific states
        IsHomeDoorUnlocked = false;
        IsCutscenePlayed = false;
        HasTalkedToMeat = false;
        
        GD.Print("Game state reset - Starting new game");

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

	private DialogueLine currentDialogue;
    private List<DialogueChoice> currentChoices;

	public void DisplayDialogue(string characterName, DialogueLine dialogue)
    {
        // Debug print
        GD.Print($"Displaying dialogue for {characterName}: {dialogue.Text}");
        GD.Print($"Number of choices: {dialogue.Choices?.Count ?? 0}");

        // Display the character's dialogue
        DisplayDialogue(characterName, dialogue.Text);
        
        // Store current dialogue and choices
        currentDialogue = dialogue;
        currentChoices = dialogue.Choices;

        // Display numbered choices
        if (dialogue.Choices != null && dialogue.Choices.Count > 0)
        {
            // string choiceText = "\n"; // Start with newline
			string choiceText = "";
            for (int i = 0; i < dialogue.Choices.Count; i++)
            {
                choiceText += $"\n{i + 1}. {dialogue.Choices[i].Text}";
            }
            // Append choices to dialogue text
            DisplayDialogue("[Choices]", choiceText); // Empty character name for choices
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event is InputEventKey eventKey && eventKey.Pressed)
        {
            // Check for number keys 1-9
            if (currentChoices != null && currentChoices.Count > 0)
            {
                int number = (int)eventKey.Keycode - (int)Key.Key1;
                if (number >= 0 && number < currentChoices.Count)
                {
                    // Display the player's choice with "You:" prefix
                    DisplayDialogue("You", currentChoices[number].Text);
					HandleChoice(number);
                }
            }
        }
    }

    private void HandleChoice(int choiceIndex)
    {
        var choice = currentChoices[choiceIndex];
        choice.OnSelect?.Invoke();
        
        // Clear current choices after selection
        currentChoices = null;
        currentDialogue = null;
    }


	public void StartNewDay()
	{
		isNight = false;
        CurrentEnergy = MaxEnergyPerDay;
		ChatBox.Instance.AddMessage("SYSTEM", $"Starting day {CurrentDay}");
		EmitSignal(SignalName.DayStarted, CurrentDay, CurrentEnergy);

		// Switch to home scene
		GetTree().ChangeSceneToFile("res://scenes/home/home.tscn");
		// Add any Day 1 specific dialogue or setup
		DisplayDialogue("SYSTEM", "Good morning, starshine. The Earth says hello!");
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

    private Dictionary<string, bool> previousDayClues = new Dictionary<string, bool> {
			{ "Paul", false },
			{ "Aileen", false }
	};
	private List<string> charactersToTrack = new List<string> { "Paul", "Aileen" }; // Add more as needed

	public void InitializePreviousClues()
	{
		foreach (var character in charactersToTrack)
		{
			previousDayClues[character] = HasTalkedTo(character);
		}
        GD.Print($"Previous day clues: {string.Join(", ", previousDayClues)}");
	}
    public List<string> GetNewClues()
	{
		List<string> newClues = new List<string>();
		
		foreach (var character in charactersToTrack)
		{
			bool talkedToToday = HasTalkedTo(character);
			bool talkedToPreviously = previousDayClues[character];
			GD.Print($"Checking {character}: {talkedToToday} vs {talkedToPreviously}");
			if (!talkedToPreviously && talkedToToday)
			{
				newClues.Add(character);
			}
		}
		
		return newClues;
	}

	public void AdvanceToNextDay()
	{
		CurrentDay++;

        // Check if we've reached day 4
        if (CurrentDay >= 4)
        {
            // Switch to the end scene
            ChatBox.Instance.ToggleVisibility();
            GD.Print("Changed scene to new file.");
            GetTree().ChangeSceneToFile("res://scenes/end/end_scene.tscn");
            return;
        }


		StartNewDay();
	}
    public void StartNight()
	{
		isNight = true;
        HasTalkedToMeat = false; // need to talk to meat again every night
        GetTree().ChangeSceneToFile("res://scenes/home/home.tscn");
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


	// Track collected items
    public bool HasTalkedToMeat { get; private set; } = false;


    public void CollectMeat()
    {
        HasTalkedToMeat = true;
    }



    [Signal]
    public delegate void DoorUnlockedEventHandler();


}
