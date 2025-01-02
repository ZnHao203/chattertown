using Godot;
using System;

public partial class Plunger : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	private string defaultDialogue = "Ah, yes. My trustworthy plunger, can't live without you.";

	private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				GD.Print("CLICK ON: PLUNGER");
				GameManager.Instance.DisplayDialogue("Plunger", defaultDialogue);
			}
		}
	}
}
