using Godot;
using System;

public partial class HillExit : Area2D
{
	[Export]
	private string MapScenePath = "res://scenes/map/map.tscn";
	
	// Optional: Store the position where the player should spawn outside
	[Export]
	private Vector2 ExitPosition = new Vector2(0, 0);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InputPickable = true;
	}
	
	private void _on_hill_exit_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				GD.Print("Exiting Hill...");
				ExitToMap();
			}
		}
	}
	
	private void ExitToMap()
	{
		// Optional: Save the exit position for the player
		// You could use an autoload/singleton to store this data
		// or save it to a file if needed
		
		GetTree().ChangeSceneToFile(MapScenePath);
		ChatBox.Instance.ToggleVisibility();
	}

}
