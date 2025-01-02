using Godot;
using System;

public partial class ToTemple : Area2D
{
	[Export]
	private string TempleScenePath = "res://scenes/temple/temple.tscn";
	
	// Optional: Store the position where the player should spawn outside
	[Export]
	private Vector2 ExitPosition = new Vector2(0, 0);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InputPickable = true;
	}
	
	private void _on_to_temple_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				GD.Print("Going to Hill Park...");
				GoToTemple();
			}
		}
	}
	
	private void GoToTemple()
	{
		// Optional: Save the exit position for the player
		// You could use an autoload/singleton to store this data
		// or save it to a file if needed
		
		GetTree().ChangeSceneToFile(TempleScenePath);
		ChatBox.Instance.ToggleVisibility();
	}
}
