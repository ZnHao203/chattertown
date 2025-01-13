using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Gus : Character
{

    public override void _Ready()
    {
        CharacterName = "Gus"; 
		base._Ready();
        InitializeDialogues();
    }

	protected override void InitializeDialogues()
    {
        // First meeting dialogue
		_dialogueLines.Clear();

        _dialogueLines.Add(new DialogueLine(
            "Yo, what's up?",
            () => !GameManager.Instance.HasTalkedTo("Gus")
        )
        .AddChoice("Gus, can I talk to you? About Bill.", "", () => 
        {
            GameManager.Instance.DisplayDialogue(CharacterName, "What do you want? I didn't kill him! Stay away from me!");
            GameManager.Instance.RecordCharacterInteraction("Gus");
        }));

		_dialogueLines.Add(new DialogueLine(
            "...",
            () => GameManager.Instance.HasTalkedTo("Gus") &&
				!GameManager.Instance.HasTalkedTo("Gus1")
        )
        .AddChoice("That's not what I meant! I just want to understand what happened. I heard you two had a history.", "", () => 
        {
            GameManager.Instance.DisplayDialogue(CharacterName, 
				"So you think I'm the killer! Listen, he's dead, and he got what was coming to him. It has nothing to do with me.");
            GameManager.Instance.RecordCharacterInteraction("Gus1");
        }));

		_dialogueLines.Add(new DialogueLine(
            "...",
            () => GameManager.Instance.HasTalkedTo("Gus1") &&
					!(GameManager.Instance.HasTalkedTo("Gus21") &&
					GameManager.Instance.HasTalkedTo("Gus22") &&
					GameManager.Instance.HasTalkedTo("Gus23"))
        )
        .AddChoice("Why do you say he got what was coming to him?", "", () => 
        {
            GameManager.Instance.DisplayDialogue(CharacterName, 
				"People with a little fame like him, always start to think they’re better than everyone else. Falling off a cliff? He had it coming.");
            GameManager.Instance.RecordCharacterInteraction("Gus21");
        })
		.AddChoice("I never said you were the killer. Why are you reacting so strongly?", "", () => 
        {
            GameManager.Instance.DisplayDialogue(CharacterName, 
				"If you don’t think I’m the killer, why are you asking me?! \n" + 
				"I just got dragged in for questioning, and now everyone’s pointing fingers at me. Do you think I’m blind?! (throws a cup)");
            GameManager.Instance.DisplayDialogue("Lady Quack", 
				"That’s another cup you owe me for breaking.");
			
			GameManager.Instance.RecordCharacterInteraction("Gus22");
        })
		.AddChoice("Can you tell me about the other person who was called to the police station with you?", "", () => 
        {
            GameManager.Instance.DisplayDialogue(CharacterName, 
				"That guy is a weirdo, totally out of it. He seems to be the first one who noticed the photographer was missing. If anyone's suspicious, it’s definitely him!");
            GameManager.Instance.RecordCharacterInteraction("Gus23");
        })
		);

		_dialogueLines.Add(new DialogueLine(
			"...",
			() => GameManager.Instance.HasTalkedTo("Gus22") &&
				!GameManager.Instance.HasTalkedTo("Gus2211")
		) 
		.AddChoice("I just want to understand your relationship with Bill. Can you tell me?", "", () => 
		{
			GameManager.Instance.RecordCharacterInteraction("Gus2211");

			_dialogueLines.Add(new DialogueLine(
				"What relationship? I don’t know him! I just saw him hogging the photo spot for too long and went over to tell him off. Other tourists should be thanking me!",
				() => GameManager.Instance.HasTalkedTo("Gus2211") &&
					!GameManager.Instance.HasTalkedTo("Gus2212")
			) 
			.AddChoice("You said you just told him off?", "", () => 
			{
				GameManager.Instance.RecordCharacterInteraction("Gus2212");

				_dialogueLines.Add(new DialogueLine(
					"...Okay, I pushed him a bit, but it was just a moment of anger during the day. He just fell to the ground, maybe scraped his skin. Don’t tell me that push caused him to fall off the cliff at night.",
					() => GameManager.Instance.HasTalkedTo("Gus2212") &&
						!GameManager.Instance.HasTalkedTo("Gus2213")
				) 
				.AddChoice("I see. The police confirmed your alibi, right?", "", () => 
				{
					GameManager.Instance.DisplayDialogue(CharacterName, 
						"What do you think? I was in the hotel all night, my roommate and the front desk can vouch for me. I had no time to commit the crime.");
					GameManager.Instance.RecordCharacterInteraction("Gus2213");
				}));
			}));

		}));

		_dialogueLines.Add(new DialogueLine(
			"That guy is a weirdo, totally out of it. ",
			() => GameManager.Instance.HasTalkedTo("Gus23") &&
				!( GameManager.Instance.HasTalkedTo("Gus231") &&
					GameManager.Instance.HasTalkedTo("Gus232") &&
					GameManager.Instance.HasTalkedTo("Gus233"))
		) 
		.AddChoice("What makes him so out of it?", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Uh! He was rambling nonsense, talking about monsters one minute and aliens the next, acting all paranoid like he just escaped from a mental hospital.");
			GameManager.Instance.RecordCharacterInteraction("Gus231");
		})
		.AddChoice("Where is he now??", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Probably at the hotel. He wanted to stay at the police station, but the cops kicked him out to avoid messing up the investigation.");
			GameManager.Instance.RecordCharacterInteraction("Gus232");
		})
		.AddChoice("Do you know this guy?", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"No idea. I only found out today his name is Ed... He didn’t really stand out in the tour group. I don’t know how he got involved in this.");
			GameManager.Instance.RecordCharacterInteraction("Gus233");
		})
		);

		// All dialogues exhausted
		_dialogueLines.Add(new DialogueLine(
            "What else do you want to know?",
            () => GameManager.Instance.HasTalkedTo("Gus21") &&
					GameManager.Instance.HasTalkedTo("Gus2213") &&
					( GameManager.Instance.HasTalkedTo("Gus231") &&
					GameManager.Instance.HasTalkedTo("Gus232") &&
					GameManager.Instance.HasTalkedTo("Gus233")) &&
					! GameManager.Instance.HasTalkedTo("Gus31")
        )
        .AddChoice("So... last question. Do you think Bill's death was an accident or something else?", "", () => 
        {
            
			GameManager.Instance.RecordCharacterInteraction("Gus31");
			_dialogueLines.Add(new DialogueLine( 
				"Whether it was an accident, suicide, or murder, he left me with a whole mess after he died. Honestly, I wish I could choke him again if he came back.",
				() => GameManager.Instance.HasTalkedTo("Gus31") &&
						!GameManager.Instance.HasTalkedTo("Gus32")
			)
			.AddChoice("...I didn’t offend you earlier... right?", "", () =>
			{
				GameManager.Instance.DisplayDialogue(CharacterName, "Ha, scared now? You’ve already wasted enough of my time. Get lost!");
				GameManager.Instance.RecordCharacterInteraction("Gus32");
			}));
            
        }));

		// next time

		_dialogueLines.Add(new DialogueLine(
            "Get lost!",
            () => GameManager.Instance.HasTalkedTo("Gus32")
        ));

		// _dialogueLines.Add(new DialogueLine(
        //     "...",
        //     () => GameManager.Instance.HasTalkedTo("Gus1") &&
		// 		!GameManager.Instance.HasTalkedTo("Gus2")
        // )

        // // Return visit dialogue
        // _dialogueLines.Add(new DialogueLine(
        //     "You guys are not the first couple of people I met today… \n" + 
        //     "I remember seeing Bill go up to the hills at the very middle of the night, perhaps just a few minutes past midnight. I mean it's dark as hell up there, why bother going up at that time?",
        //     () => GameManager.Instance.HasTalkedTo("Paul") &&
        //         !GameManager.Instance.HasTalkedTo("Paul1")
        // )
        // .AddChoice("You sure that's Bill?", "", () => 
        // {
        //     GameManager.Instance.DisplayDialogue(CharacterName, 
        //         "No, no one would.");
        //     GameManager.Instance.RecordCharacterInteraction("Paul1");
        // }));

        // _dialogueLines.Add(new DialogueLine(
        //     "Yup, absolutely. That poor man waved an ultra-bright flashlight before climbing up. I was sitting beside my window enjoying a midnight snack before he interrupted my sweet snack… It must have been him. He was carrying a large hiking backpack, with what seemed like a camera hanging from it, looking just like a tourist. At the time, I even thought to myself, 'This outsider really has nothing better to do, climbing up the mountain in the middle of the night.'",
        //     () => GameManager.Instance.HasTalkedTo("Paul1") &&
        //             !GameManager.Instance.HasTalkedTo("PaulDone")
        // )
        // .AddChoice("Hmm..", "", () => 
        // {
        //     GameManager.Instance.DisplayDialogue(CharacterName, 
        //         "Weird right? I double-checked my security camera after hearing his news… BUT NO ONE ELSE WENT UP THERE!");
        //     GameManager.Instance.RecordCharacterInteraction("PaulDone");
        // }));


        // // Return visit dialogue
		// _dialogueLines.Add(new DialogueLine(
		// 	"Back again? Still investigating?",
		// 	() => GameManager.Instance.HasTalkedTo("PaulDone")
		// )
		// .AddChoice("Just say hi.", "", () => 
		// {
		// 	GameManager.Instance.DisplayDialogue(CharacterName, 
		// 		"Well, if you want to try some bread, you know where to find me.");
		// }));

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