using Godot;
using System;

public partial class Town2 : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private void _on_sign_village_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				GetTree().ChangeSceneToFile("res://scenes/main/main.tscn");
				
			}
		}
	}

	private void _on_sign_hill_park_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				GetTree().ChangeSceneToFile("res://scenes/hill/hill_down.tscn");
				
			}
		}
	}
}
