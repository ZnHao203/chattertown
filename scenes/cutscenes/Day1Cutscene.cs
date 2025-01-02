using Godot;
using System;
using System.Collections.Generic;

public partial class Day1Cutscene : Node
{
	private string OutsideScenePath = "res://scenes/main/main.tscn";
	private Queue<(string speaker, string text)> _dialogues = new Queue<(string, string)>();
	
	public override void _Ready()
	{
		// Set up the cutscene dialogues
		_dialogues.Enqueue(("Mr. Green", "And have you seen the prices at the corner store lately?"));
		_dialogues.Enqueue(("Mrs. Green", "Don't get me started! But between you and me..."));
		_dialogues.Enqueue(("Mr. Green", "I saw the owner's son sneaking out with Sarah from number 38!"));
		_dialogues.Enqueue(("Mrs. Green", "No! But isn't she dating that boy from the bakery?"));

		// Start first dialogue
		ShowNextDialogue();
	}

	public override void _Input(InputEvent @event)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				AdvanceDialogue();
			}
		}
	}

	private void ShowNextDialogue()
	{
		if (_dialogues.Count > 0)
		{
			var (speaker, text) = _dialogues.Dequeue();
			GameManager.Instance.DisplayDialogue(speaker, text);
		}
		else
		{
			EndCutscene();
		}
	}

	private void AdvanceDialogue()
	{
		ShowNextDialogue();
	}

	private void EndCutscene()
	{
		// Return to previous scene
		GetTree().ChangeSceneToFile(OutsideScenePath);
		GameManager.Instance.DisplayDialogue("SYSTEM", "Feel free to explore the town.");
	}

}
