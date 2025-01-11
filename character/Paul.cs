using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Paul : Character
{

    public override void _Ready()
    {
        CharacterName = "Paul"; 
		base._Ready();
        InitializeDialogues();
    }

	 protected override void InitializeDialogues()
    {
        // First meeting dialogue
		_dialogueLines.Clear();

        _dialogueLines.Add(new DialogueLine(
            "Hello there! I'm Paul, the town's baker.",
            () => !GameManager.Instance.HasTalkedTo("Paul")
        )
        .AddChoice("Hi Paul!", "", () => 
        {
            GameManager.Instance.DisplayDialogue(CharacterName, "Nice weather today!");
        })
        .AddChoice("Tell me about your bakery!", "", () => 
        {
            GameManager.Instance.DisplayDialogue(CharacterName, 
                "I make the best bread in town! Would you like to try some?");
			GameManager.Instance.RecordCharacterInteraction("Paul");
        }));

        // Return visit dialogue
        _dialogueLines.Add(new DialogueLine(
            "Welcome back! How can I help you today?",
            () => GameManager.Instance.HasTalkedTo("Paul")
        )
        .AddChoice("I'd like some bread.", "", () => 
        {
            GameManager.Instance.DisplayDialogue(CharacterName, 
                "Here's a fresh loaf, just out of the oven!");
        })
        .AddChoice("Just saying hello!", "", () => 
        {
            GameManager.Instance.DisplayDialogue(CharacterName, 
                "Always nice to see a friendly face!");
        }));

		// For debugging, let's print the number of dialogues and choices
        GD.Print($"Paul has {_dialogueLines.Count} dialogues");
        foreach (var dialogue in _dialogueLines)
        {
            GD.Print($"Dialogue: {dialogue.Text}");
            GD.Print($"Number of choices: {dialogue.Choices.Count}");
        }
    }


	protected override void StartDialog()
    {
        // Debug print
        GD.Print("Starting Paul's dialogue");
        
        // Filter available dialogues based on conditions
        var availableDialogues = _dialogueLines
            .Where(d => d.Condition())
            .ToList();

        // Debug print
        GD.Print($"Found {availableDialogues.Count} available dialogues");

        if (availableDialogues.Count == 0)
        {
            // Fallback dialogue if no conditions are met
            GD.Print("No available dialogues, showing fallback");
            GameManager.Instance.DisplayDialogue(CharacterName, "Hi!");
            return;
        }

        var randomLine = availableDialogues[new Random().Next(availableDialogues.Count)];
        GameManager.Instance.DisplayDialogue(CharacterName, randomLine);
    }

}