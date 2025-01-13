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
            "Hi! Ya need help?",
            () => !GameManager.Instance.HasTalkedTo("Paul")
        )
        .AddChoice("Nah, just saying hi!", "", () => 
        {
            GameManager.Instance.DisplayDialogue(CharacterName, "Sure, have a nice day!");
        })
        .AddChoice("Well, tell me something about your bakery!", "", () => 
        {
            GameManager.Instance.DisplayDialogue(CharacterName, 
                "So, typically, I start to prepare the dough from the one night before I make them into bread, I mix the dough, let it rise, shape it, and bake it fresh for thirty years like a day.");
			GameManager.Instance.RecordCharacterInteraction("Paul");
        }));

        // Return visit dialogue
        _dialogueLines.Add(new DialogueLine(
            "You guys are not the first couple of people I met today… \n" + 
            "I remember seeing Bill go up to the hills at the very middle of the night, perhaps just a few minutes past midnight. I mean it's dark as hell up there, why bother going up at that time?",
            () => GameManager.Instance.HasTalkedTo("Paul") &&
                !GameManager.Instance.HasTalkedTo("Paul1")
        )
        .AddChoice("You sure that's Bill?", "", () => 
        {
            GameManager.Instance.DisplayDialogue(CharacterName, 
                "No, no one would.");
            GameManager.Instance.RecordCharacterInteraction("Paul1");
        }));

        _dialogueLines.Add(new DialogueLine(
            "Yup, absolutely. That poor man waved an ultra-bright flashlight before climbing up. I was sitting beside my window enjoying a midnight snack before he interrupted my sweet snack… It must have been him. He was carrying a large hiking backpack, with what seemed like a camera hanging from it, looking just like a tourist. At the time, I even thought to myself, 'This outsider really has nothing better to do, climbing up the mountain in the middle of the night.'",
            () => GameManager.Instance.HasTalkedTo("Paul1") &&
                    !GameManager.Instance.HasTalkedTo("PaulDone")
        )
        .AddChoice("Hmm..", "", () => 
        {
            GameManager.Instance.DisplayDialogue(CharacterName, 
                "Weird right? I double-checked my security camera after hearing his news… BUT NO ONE ELSE WENT UP THERE!");
            GameManager.Instance.RecordCharacterInteraction("PaulDone");
        }));


        // Return visit dialogue
		_dialogueLines.Add(new DialogueLine(
			"Back again? Still investigating?",
			() => GameManager.Instance.HasTalkedTo("PaulDone")
		)
		.AddChoice("Just say hi.", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Well, if you want to try some bread, you know where to find me.");
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