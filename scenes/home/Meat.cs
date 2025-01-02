using Godot;
using System;

public partial class Meat : Area2D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	
	private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				GD.Print("CLICK ON: MEAT");
				HandleMeatDialogue();
			}
		}
	}
	
	private void HandleMeatDialogue()
	{
		var result = GameManager.Instance.CurrentDay switch
		{
			1 => HandleDay1(),
			_ => HandleDefaultDay()  // _ is the default case
		};
	}
	
	private int HandleDay1()
	{
		// assume it's morning
		GameManager.Instance.CollectMeat();
		GameManager.Instance.DisplayDialogue("???", "Still asleep.");
		return 0;
	}
	
	private int HandleDefaultDay()
	{
		return 0;
	}
	
}
