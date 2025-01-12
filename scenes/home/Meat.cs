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
		if (! GameManager.Instance.isNight){
			// meat sleeps at day
			GameManager.Instance.DisplayDialogue("???", "Still asleep.");
		} else {
			// can interact with meat at night
			var meatChar = GetParent<MeatChar>();
			meatChar.StartNightRecap();
			GameManager.Instance.CollectMeat();
		}
	}

	
}
