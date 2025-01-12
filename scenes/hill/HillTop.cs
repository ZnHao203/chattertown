using Godot;
using System;

public partial class HillTop : Node
{
	private string MidHillPath = "res://scenes/hill/hill_middle.tscn";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private void _on_to_hill_middle_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				GD.Print("Exiting to Mid Hill...");
				GetTree().ChangeSceneToFile(MidHillPath);
			}
		}
	}
}
