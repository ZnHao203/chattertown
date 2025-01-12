using Godot;
using System;

public partial class HillMiddle : Node
{
	private string TopHillPath = "res://scenes/hill/hill_top.tscn";
	private string DownHillPath = "res://scenes/hill/hill_down.tscn";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}
	
	private void _on_to_hill_down_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				GD.Print("Exiting Hill to Down Hill...");
				GetTree().ChangeSceneToFile(DownHillPath);
			}
		}
	}

	private void _on_to_hill_top_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				GD.Print("Exiting to Top Hill...");
				GetTree().ChangeSceneToFile(TopHillPath);
			}
		}
	}
}
