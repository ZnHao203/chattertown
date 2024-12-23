using Godot;
using System;

public partial class Door : Area2D
{
	[Export]
	private string OutsideScenePath = "res://main/main.tscn";
	
	// Optional: Store the position where the player should spawn outside
	[Export]
	private Vector2 ExitPosition = new Vector2(0, 0);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InputPickable = true;
	}
	
	private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				GD.Print("Exiting house...");
				ExitToOutside();
			}
		}
	}
	
	private void ExitToOutside()
	{
		// Optional: Save the exit position for the player
		// You could use an autoload/singleton to store this data
		// or save it to a file if needed
		
		GetTree().ChangeSceneToFile(OutsideScenePath);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
