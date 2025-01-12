using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class MeatChar : Character
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CharacterName = "Meat"; 
		base._Ready();
		// InitializePreviousClues();
        // InitializeDialogues();
	}

	protected override void InitializeDialogues()
	{
		_dialogueLines.Clear();

		// First meeting dialogue
		_dialogueLines.Add(new DialogueLine(
			"Hello there! I'm Meat, your best friend. I heard about Bill's death. I can help you track clues you find each day.",
			() => !GameManager.Instance.HasTalkedTo("Meat")
		)
		.AddChoice("What do you know?", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Nothing yet. Come back when you've talked to people around town.");
			GameManager.Instance.RecordCharacterInteraction("Meat");
		}));

		// This is just for initialization - actual daily dialogues will be handled in StartDialog
		_dialogueLines.Add(new DialogueLine(
			"Let's see what's new today...",
			() => GameManager.Instance.HasTalkedTo("Meat")
		));
	}

	public void StartNightRecap()
	{
		// Start the daily dialogue
		StartDialog();
	}

	protected override void StartDialog()
	{
		var newClues = GetNewClues();
		GD.Print($"new day clues: {string.Join(", ", newClues)}");
		
		// if (!GameManager.Instance.HasTalkedTo("Meat"))
		// {
		// 	// First meeting - use the first dialogue line
		// 	base.StartDialog();
		// }
		string dialogueText = "";
		if (!GameManager.Instance.HasTalkedTo("Meat"))
		{
			dialogueText += "Hello there! I'm Meat, your best friend. \nI heard about Bill's death. \nI can help you track clues you find each day.\n";
			GameManager.Instance.RecordCharacterInteraction("Meat");
		}

		dialogueText += "Let's see what's new today...\n";

		// Daily clue check and dialogue
		
		if (newClues.Count == 0)
		{
			dialogueText += "Nothing new today. Keep investigating!";
		}
		else
		{
			dialogueText = "Here's what I've gathered:\n";
			foreach (var clue in newClues)
			{
				switch (clue)
				{
					case "Paul":
						dialogueText += "- You have talked to Paul, Bravo!\n";
						break;
					case "Aileen":
						dialogueText += "- You have talked to Aileen, Bravo!\n";
						break;
					// Add more cases for other characters
				}
			}
		}

		// Display the dialogue and process the daily interaction
		GameManager.Instance.DisplayDialogue(CharacterName, dialogueText);
		GameManager.Instance.CollectMeat();
		InitializePreviousClues(); // Update for next day
	}

	// private Dictionary<string, bool> previousDayClues = new Dictionary<string, bool> {
	// 		{ "Paul", false },
	// 		{ "Aileen", false }
	// };
	// private List<string> charactersToTrack = new List<string> { "Paul", "Aileen" }; // Add more as needed

	private void InitializePreviousClues()
	{
		GameManager.Instance.InitializePreviousClues();
	}

	private List<string> GetNewClues()
	{
		return GameManager.Instance.GetNewClues();
	}
}
