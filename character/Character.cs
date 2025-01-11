using Godot;
using System;
using System.Collections.Generic;
using System.Linq;

// public class DialogueLine
// {
//     public string Text { get; private set; }
//     public Func<bool> Condition { get; private set; }

//     public DialogueLine(string text, Func<bool> condition = null)
//     {
//         Text = text;
//         Condition = condition ?? (() => true); // If no condition provided, always available
//     }
// }

public class DialogueChoice
{
	public string Text { get; private set; }
	public string NextDialogueId { get; private set; }
	public Action OnSelect { get; private set; }

	public DialogueChoice(string text, string nextDialogueId = "", Action onSelect = null)
	{
		Text = text;
		NextDialogueId = nextDialogueId;
		OnSelect = onSelect;
	}
}

public class DialogueLine
{
	public string Id { get; private set; }
	public string Text { get; private set; }
	public Func<bool> Condition { get; private set; }
	public List<DialogueChoice> Choices { get; private set; }

	public DialogueLine(string text, Func<bool> condition = null)
	{
		Text = text;
		Condition = condition ?? (() => true); // If no condition provided, always available
		Choices = new List<DialogueChoice>();
	}

	public DialogueLine(string id, string text, Func<bool> condition = null)
	{
		Id = id;
		Text = text;
		Condition = condition ?? (() => true);
		Choices = new List<DialogueChoice>();
	}

	public DialogueLine AddChoice(string text, string nextDialogueId = "", Action onSelect = null)
	{
		if (Choices == null)  // Extra safety check
		{
			Choices = new List<DialogueChoice>();
		}
		Choices.Add(new DialogueChoice(text, nextDialogueId, onSelect));
		return this;
	}
}

public partial class Character : Node2D
{
	[Export]
	public string CharacterName { get; protected set; } = "Character";
	
	[Export]
	private int _energyCost = 5;

	private Area2D _interactionArea;

	protected List<DialogueLine> _dialogueLines = new List<DialogueLine>();
	
	// This is a virtual method that each character will override
	protected virtual void InitializeDialogues()
	{
		// Basic dialogues with no conditions
		_dialogueLines.Add(new DialogueLine("Nice weather today!"));

		// Dialogue that only appears after day 3
		_dialogueLines.Add(new DialogueLine(
			"Did you hear about the festival coming up?",
			() => GameManager.Instance.CurrentDay >= 3
		));

		// // Dialogue that only appears if player hasn't talked to Paul
		// _dialogueLines.Add(new DialogueLine(
		//     "You should really go talk to Paul at the bakery.",
		//     () => !GameManager.Instance.HasTalkedTo("Paul")
		// ));

		// Dialogue that appears only after talking to Paul AND after day 2
		_dialogueLines.Add(new DialogueLine(
			"Paul's bread recipe is amazing, isn't it?",
			() => GameManager.Instance.HasTalkedTo("Paul") && GameManager.Instance.CurrentDay >= 2
		));
	}

	public string GetRandomDialogue()
	{
		// Filter available dialogues based on conditions
		var availableDialogues = _dialogueLines
			.Where(d => d.Condition())
			.ToList();

		if (availableDialogues.Count() == 0)
			return "Hello!"; // Default fallback dialogue

		// Pick a random dialogue from available ones
		Random random = new Random();
		int index = random.Next(availableDialogues.Count());
		return availableDialogues[index].Text;
	}
	public override void _Ready()
	{
		_interactionArea = GetNode<Area2D>("Area2D");
		_interactionArea.InputEvent += OnAreaInputEvent;
	}

	private void OnAreaInputEvent(Node viewport, InputEvent @event, long shapeIdx)
	{
		// Check for left mouse click
		if (@event is InputEventMouseButton mouseButton)
		{
			if (mouseButton.ButtonIndex == MouseButton.Left && mouseButton.Pressed)
			{
				OnInteract();
			}
		}
	}

	public void OnInteract()
	{
		if (GameManager.Instance.CurrentEnergy >= _energyCost)
		{
			if (GameManager.Instance.UseEnergy(_energyCost))
			{
				StartDialog();
			}
		}
		else
		{
			ShowNoEnergyMessage();
		}
	}

	protected virtual void StartDialog()
	{
		// Filter available dialogues based on conditions and pick a random one
		var availableDialogues = _dialogueLines
			.Where(d => d.Condition())
			.ToList();

		if (availableDialogues.Count == 0)
		{
			// Fallback dialogue if no conditions are met
			GameManager.Instance.DisplayDialogue(CharacterName, "Hi!");
			return;
		}

		var randomLine = availableDialogues[new Random().Next(availableDialogues.Count)];
		GD.Print($"{CharacterName}: {randomLine.Text}");
		GameManager.Instance.DisplayDialogue(CharacterName, randomLine.Text);
	}

	private void ShowNoEnergyMessage()
	{
		GD.Print("Not enough energy to talk!");
	}

}
