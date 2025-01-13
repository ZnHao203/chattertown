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
		// _dialogues.Enqueue(("Mr. Green", "And have you seen the prices at the corner store lately?"));
		// _dialogues.Enqueue(("Mrs. Green", "Don't get me started! But between you and me..."));
		// _dialogues.Enqueue(("Mr. Green", "I saw the owner's son sneaking out with Sarah from number 38!"));
		// _dialogues.Enqueue(("Mrs. Green", "No! But isn't she dating that boy from the bakery?"));

		_dialogues.Enqueue(("Detective Morris", "Did you hear? They found him yesterday afternoon, down by the cliff. He fell to his death."));
		_dialogues.Enqueue(("Officer Chen", "Oh my god, that's terrifying! Was it an accident or suicide?"));
		_dialogues.Enqueue(("You", "Are you talking about the tourist who went missing yesterday? They found the body?"));
		_dialogues.Enqueue(("Detective Morris", "Yeah, aren't you usually in the loop? How come you just found out now?"));
		_dialogues.Enqueue(("You", "Well, I spent all afternoon yesterday playing chess."));
		_dialogues.Enqueue(("Officer Chen", "How did that go?"));
		_dialogues.Enqueue(("You", "Ten games, ten losses."));
		_dialogues.Enqueue(("Detective Morris", "......"));
		_dialogues.Enqueue(("You", "So, back to the case. Any new updates?"));
		_dialogues.Enqueue(("Detective Morris", "Uh... we don't know either. The police haven't shared any more details."));
		_dialogues.Enqueue(("Officer Chen", "Isn't this your area of expertise? Why don't you ask around?"));
		_dialogues.Enqueue(("You", "Good point. Time to get to work!"));

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
