using Godot;
using System;

public partial class Main : Node
{
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	private void _on_area_2d_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				GetTree().ChangeSceneToFile("res://scenes/town2/town_2.tscn");
				
			}
		}
	}

}
