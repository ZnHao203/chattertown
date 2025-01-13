using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Cameron : Character
{

    public override void _Ready()
    {
        CharacterName = "Cameron"; 
		base._Ready();
        InitializeDialogues();
    }

	protected override void InitializeDialogues()
    {
        _dialogueLines.Clear();

		_dialogueLines.Add(new DialogueLine(
			"Oh, it's you. Let me guess, you want to know about that dead tourist?",
			() => !GameManager.Instance.HasTalkedTo("Cameron") &&
					!GameManager.Instance.HasTalkedTo("Cameron0")
		)
		.AddChoice("Exactly, how did you know?", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"What else do you care about besides gossip?");
			GameManager.Instance.RecordCharacterInteraction("Cameron0");
		}));

		// Follow-up question about tourist
		_dialogueLines.Add(new DialogueLine(
			"...",
			() => GameManager.Instance.HasTalkedTo("Cameron0") && 
				!GameManager.Instance.HasTalkedTo("Cameron1")
		)
		.AddChoice("So he really came by here?", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Yeah, I was shocked when I heard he died.");
			GameManager.Instance.RecordCharacterInteraction("Cameron1");
			GameManager.Instance.RecordCharacterInteraction("Cameron");
		}));

		_dialogueLines.Add(new DialogueLine(
			"What else do you want to know?",
			() => GameManager.Instance.HasTalkedTo("Cameron1") && 
				!(GameManager.Instance.HasTalkedTo("Cameron21") && 
					GameManager.Instance.HasTalkedTo("Cameron22") && 
					GameManager.Instance.HasTalkedTo("Cameron23"))
		)
		.AddChoice("Did you know the victim?", "", () =>
		{
			GameManager.Instance.DisplayDialogue(CharacterName,
				"Oh, I actually know him. He was a well-known photographer for the magazine 'Seasons and Scenery'. " +
				"Us photography enthusiasts have all seen his work.");
			GameManager.Instance.RecordCharacterInteraction("Cameron21");
		})
		.AddChoice("What did he come here for?", "", () =>
		{
			GameManager.Instance.DisplayDialogue(CharacterName,
				"He came by twice. The first time was two days before he died to get some photos developed. " +
				"Then the next day he came back to buy some film.");
			GameManager.Instance.RecordCharacterInteraction("Cameron22");
		})
		.AddChoice("Did you see any of the photos he took?", "", () =>
		{
			GameManager.Instance.DisplayDialogue(CharacterName,
				"No, he said he wanted to develop them himself. After all, he was a pro, so I let him. " +
				"By the way, he spent quite a while in the darkroom... I went to check on him once, but he ignored me. " +
				"Then he rushed out after paying, and I meant to ask if I could see the photos, but I didn't get the chance.");
			GameManager.Instance.RecordCharacterInteraction("Cameron23");
		}));

		// Follow-up dialogue about second visit
		_dialogueLines.Add(new DialogueLine(
			"About his second visit...",
			() => GameManager.Instance.HasTalkedTo("Cameron22") && 
				!GameManager.Instance.HasTalkedTo("Cameron221")
		)
		.AddChoice("So what about the second time he came?", "", () =>
		{
			GameManager.Instance.DisplayDialogue(CharacterName,
				"He said he didn't brought enough film this time, so he bought some more from me.");
			GameManager.Instance.RecordCharacterInteraction("Cameron221");
		}));

		// Follow-up dialogue about photos
		_dialogueLines.Add(new DialogueLine(
			"About those photos...",
			() => GameManager.Instance.HasTalkedTo("Cameron23") && 
				!GameManager.Instance.HasTalkedTo("Cameron231")
		)
		.AddChoice("Did he mention what he was filming?", "", () =>
		{
			GameManager.Instance.DisplayDialogue(CharacterName,
				"No, but what else can it be aside from sceneries? There's not much special around our town. " +
				"I forgot to ask him for the photos. He left in a hurry, you see. " +
				"Sign, what a shame, we'll never see them now.");
			GameManager.Instance.RecordCharacterInteraction("Cameron231");
		}));

		// All done
		_dialogueLines.Add(new DialogueLine(
			"That's all I know... I need to get back to work.",
			() => GameManager.Instance.HasTalkedTo("Cameron231") && 
				GameManager.Instance.HasTalkedTo("Cameron221") &&
				GameManager.Instance.HasTalkedTo("Cameron21") &&
				!GameManager.Instance.HasTalkedTo("CameronDone")
		)
		.AddChoice("I see, thanks, Cameron.", "", () =>
		{
			GameManager.Instance.DisplayDialogue(CharacterName,
				"See you around");
			GameManager.Instance.RecordCharacterInteraction("CameronDone");
		}));

		// Return visit dialogue
		_dialogueLines.Add(new DialogueLine(
			"Back again? Still investigating?",
			() => GameManager.Instance.HasTalkedTo("CameronDone")
		)
		.AddChoice("Just checking in.", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Well, if you need anything else about the photographer, you know where to find me.");
		}));

		// Debug prints
		GD.Print($"Cameron has {_dialogueLines.Count} dialogues");
		foreach (var dialogue in _dialogueLines)
		{
			GD.Print($"Dialogue: {dialogue.Text}");
			GD.Print($"Number of choices: {dialogue.Choices.Count}");
		}
    }


	protected override void StartDialog()
    {
        // Debug print
        GD.Print("Starting Cameron's dialogue");
        
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