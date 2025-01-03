using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Betty : Character
{
	// private string[] _dialogLines = new string[] 
	// { 
	// 	"Nice weather today!!",
	// 	"How are you doing??",
	// 	"Just another day in town......"
	// };
	// protected override void StartDialog()
	// {
	// 	// Pick random dialogue line
	// 	var randomLine = _dialogLines[new Random().Next(_dialogLines.Length)];
	// 	GD.Print($"{CharacterName}: {randomLine}");
	// 	GameManager.Instance.DisplayDialogue(CharacterName, randomLine);
	// }
	private List<DialogueLine> _dialogueLines = new List<DialogueLine>();

	public override void _Ready()
    {
        base._Ready();
        InitializeDialogues();
    }
	
	protected override void InitializeDialogues()
    {
        base.InitializeDialogues(); // Call base if you want to keep common dialogues
		
		// Basic dialogues with no conditions (always available)
        _dialogueLines.Add(new DialogueLine("Nice weather today!!"));
        _dialogueLines.Add(new DialogueLine("How are you doing??"));
        _dialogueLines.Add(new DialogueLine("Just another day in town......"));

        // Add conditional dialogues
        _dialogueLines.Add(new DialogueLine(
            "Have you heard about the festival preparations?",
            () => GameManager.Instance.CurrentDay >= 2
        ));

        _dialogueLines.Add(new DialogueLine(
            "You should really talk to Paul at the bakery!",
            () => !GameManager.Instance.HasTalkedTo("Paul")
        ));

        _dialogueLines.Add(new DialogueLine(
            "Paul's new bread recipe is amazing, isn't it?",
            () => GameManager.Instance.HasTalkedTo("Paul")
        ));
    }

    
}
