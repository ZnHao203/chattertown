using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Ed : Character
{

    public override void _Ready()
    {
        CharacterName = "Ed"; 
		base._Ready();
        InitializeDialogues();
    }

	protected override void InitializeDialogues()
    {
        _dialogueLines.Clear();

		_dialogueLines.Add(new DialogueLine(
			"Who's there!? Don't come any closer!",
			() => !GameManager.Instance.HasTalkedTo("Ed") &&
					!GameManager.Instance.HasTalkedTo("Ed0")
		)
		.AddChoice("Uh...?", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Looks like what everyone said is true... You're a detective, right? " );
			GameManager.Instance.RecordCharacterInteraction("Ed0");
		}));

		// Follow-up question about tourist
		_dialogueLines.Add(new DialogueLine(
			"...",
			() => GameManager.Instance.HasTalkedTo("Ed0") && 
				!GameManager.Instance.HasTalkedTo("Ed1")
		)
		.AddChoice("Haha! Guess who I am!", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"I know! You must be an assassin sent by aliens!\n");
			GameManager.Instance.RecordCharacterInteraction("Ed1");
		}));

		_dialogueLines.Add(new DialogueLine(
			"You want to kill me… you will kill me as well… I won't let you succeed.",
			() => GameManager.Instance.HasTalkedTo("Ed1") && 
				!(GameManager.Instance.HasTalkedTo("Ed21") && 
                    GameManager.Instance.HasTalkedTo("Ed22") && 
					GameManager.Instance.HasTalkedTo("Ed23") && 
					GameManager.Instance.HasTalkedTo("Ed24"))
		)
        .AddChoice("As well? What do you mean?", "", () =>
        {
            GameManager.Instance.DisplayDialogue(CharacterName,
                "Don't play dumb! I know it was you who killed Bill!!");
        })

        //record

        //3*1Layer,yellow,2 lines spacing
		.AddChoice("Why do you say that? Bill's death was obviously an accident.", "", () =>
		{
			GameManager.Instance.DisplayDialogue(CharacterName,
				"Nonsense! You killed him... you killed him to cover your tracks!");
            GameManager.Instance.DisplayDialogue("Me",
                "Whoa, calm down, man. If we're talking motives, yours seems a lot stronger than mine as a bystander!");
            GameManager.Instance.DisplayDialogue(CharacterName,
                "What do you mean by bystander? You all just want to silence him!");
            GameManager.Instance.DisplayDialogue("Me",
                "Silence what? Did you discover something extraordinary?");
            GameManager.Instance.DisplayDialogue(CharacterName,
                "No… I don't know anything. It was Bill, he was the one. You've already killed him, just leave me alone...");
            GameManager.Instance.RecordCharacterInteraction("Ed21");
		})
    

		.AddChoice("You got it! I'm an alien assassin! Now tell me, why would I want to kill you?", "", () =>
		{
			GameManager.Instance.DisplayDialogue(CharacterName,
				"?! You're exposing yourself as an assassin!");
            GameManager.Instance.DisplayDialogue("Me",
                "Because my mission didn't say why I should kill you."
                + " Explain it to me; if you make a good case, I might let you go.");
            GameManager.Instance.DisplayDialogue(CharacterName,
                "...It's all Bill's fault! I really don't know anything. "
                + "Killing him was enough; I won't say a thing, just let me go...");
			GameManager.Instance.RecordCharacterInteraction("Ed22");
		})


		.AddChoice("Dude, I have no idea what you're talking about... Actually, I'm a delivery guy.", "", () =>
		{
            GameManager.Instance.DisplayDialogue(CharacterName,
                "Uh, really? But I didn't order anything.");
            GameManager.Instance.DisplayDialogue("Me",
                "I know, because I'm treating you.");
            GameManager.Instance.DisplayDialogue(CharacterName,
                "...You think that trick would work on me? Get lost!");
            GameManager.Instance.RecordCharacterInteraction("Ed23");
		})

        .AddChoice("Dude, I have no idea what you're talking about... I'm a hotel staff member, here to collect money.", "", () =>
        {
            GameManager.Instance.DisplayDialogue(CharacterName,
                "What money do you want from me? I don't owe you anything.");
			GameManager.Instance.DisplayDialogue("Me",
				"Your room fee. You haven't paid for today.");
            GameManager.Instance.DisplayDialogue(CharacterName,
                "Oh, I'll just slide it under the door.");
            GameManager.Instance.DisplayDialogue("Me",
                "...Do you really have to do it like this?");
            GameManager.Instance.DisplayDialogue(CharacterName,
                "You service staff should mind your own business, go away!");
			GameManager.Instance.RecordCharacterInteraction("Ed24");
        }));

		// all dialogues exhausted
		_dialogueLines.Add(new DialogueLine(
			"Get lost!",
			() => GameManager.Instance.HasTalkedTo("Ed21") && 
                    GameManager.Instance.HasTalkedTo("Ed22") && 
					GameManager.Instance.HasTalkedTo("Ed23") && 
					GameManager.Instance.HasTalkedTo("Ed24")
		)
		.AddChoice("(Leave)", "", () => {
			GameManager.Instance.RecordCharacterInteraction("EdDone");

			GD.Print("EdDone");
		}));
		
        /*
        // Follow-up dialogue about second visit
		_dialogueLines.Add(new DialogueLine(
			"About his second visit...",
			() => GameManager.Instance.HasTalkedTo("Ed22") && 
				!GameManager.Instance.HasTalkedTo("Ed221")
		)
		.AddChoice("So what about the second time he came?", "", () =>
		{
			GameManager.Instance.DisplayDialogue(CharacterName,
				"He said he didn't brought enough film this time, so he bought some more from me.");
			GameManager.Instance.RecordCharacterInteraction("Ed221");
		}));

		// Follow-up dialogue about photos
		_dialogueLines.Add(new DialogueLine(
			"About those photos...",
			() => GameManager.Instance.HasTalkedTo("Ed23") && 
				!GameManager.Instance.HasTalkedTo("Ed231")
		)
		.AddChoice("Did he mention what he was filming?", "", () =>
		{
			GameManager.Instance.DisplayDialogue(CharacterName,
				"No, but what else can it be aside from sceneries? There's not much special around our town. " +
				"I forgot to ask him for the photos. He left in a hurry, you see. " +
				"Sign, what a shame, we'll never see them now.");
			GameManager.Instance.RecordCharacterInteraction("Ed231");
		}));

		// All done
		_dialogueLines.Add(new DialogueLine(
			"That's all I know... I need to get back to work.",
			() => GameManager.Instance.HasTalkedTo("Ed231") && 
				GameManager.Instance.HasTalkedTo("Ed221") &&
				GameManager.Instance.HasTalkedTo("Ed21") &&
				!GameManager.Instance.HasTalkedTo("EdDone")
		)
		.AddChoice("I see, thanks, Ed.", "", () =>
		{
			GameManager.Instance.DisplayDialogue(CharacterName,
				"See you around");
			GameManager.Instance.RecordCharacterInteraction("EdDone");
		}));

		// Return visit dialogue
		_dialogueLines.Add(new DialogueLine(
			"Back again? Still investigating?",
			() => GameManager.Instance.HasTalkedTo("EdDone")
		)
		.AddChoice("Just checking in.", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Well, if you need anything else about the photographer, you know where to find me.");
		}));
        */


		// Debug prints
		GD.Print($"Ed has {_dialogueLines.Count} dialogues");
		foreach (var dialogue in _dialogueLines)
		{
			GD.Print($"Dialogue: {dialogue.Text}");
			GD.Print($"Number of choices: {dialogue.Choices.Count}");
		}
    }


	protected override void StartDialog()
    {
        // Debug print
        GD.Print("Starting Ed's dialogue");
        
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