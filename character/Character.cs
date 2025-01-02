using Godot;
using System;

public partial class Character : Node2D
{
	[Export]
	public string CharacterName { get; private set; } = "Character";
	
	[Export]
	private int _energyCost = 5;

	private Area2D _interactionArea;

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
		// For now, just print to output
		GD.Print($"Talking with {CharacterName}");
		GD.Print($"Used {_energyCost} energy");
	}

	private void ShowNoEnergyMessage()
	{
		GD.Print("Not enough energy to talk!");
	}

}
