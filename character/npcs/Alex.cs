using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class Alex : Character
{
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
        _dialogueLines.Add(new DialogueLine("Hello there! Nice to meet you!"));
        _dialogueLines.Add(new DialogueLine("I love reading books in my free time."));
        _dialogueLines.Add(new DialogueLine("The library is my favorite place in town."));

        // Add conditional dialogues
        _dialogueLines.Add(new DialogueLine(
            "I lost my favorite book somewhere... Have you seen it?",
            () => !GameManager.Instance.HasTalkedTo("Alex_BookQuest")
        ));

        _dialogueLines.Add(new DialogueLine(
            "Thank you so much for finding my book!",
            () => GameManager.Instance.HasTalkedTo("Alex_BookQuest")
        ));

        // Dialogue that only appears after talking to Paul
        _dialogueLines.Add(new DialogueLine(
            "Paul's bakery has the best cookies in town!",
            () => GameManager.Instance.HasTalkedTo("Paul")
        ));

        // Dialogue that appears only after day 2
        _dialogueLines.Add(new DialogueLine(
            "Did you hear about the upcoming book fair?",
            () => GameManager.Instance.CurrentDay >= 2
        ));

        // Dialogue that combines multiple conditions
        _dialogueLines.Add(new DialogueLine(
            "I'm thinking of sharing Paul's cookies at the book fair!",
            () => GameManager.Instance.CurrentDay >= 2 && GameManager.Instance.HasTalkedTo("Paul")
        ));
    }

    
}