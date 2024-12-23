using Godot;
using System;

public partial class House : Area2D
{
	[Export]
	private string HomeInteriorPath = "res://home/home.tscn";
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// Make the area pickable (clickable)
		InputPickable = true;
	}
	
	// Signal callback with the [Signal] attribute
	private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				GD.Print("Switching to house interior...");
				SwitchToHouseInterior();
			}
		}
	}
	
	private async void SwitchToHouseInterior()
	{
		// Get the SceneTree
		var tree = GetTree();
		
		// Change to the house interior scene
		Error error = tree.ChangeSceneToFile(HomeInteriorPath);
		
		if (error != Error.Ok)
		{
			GD.PrintErr("Failed to switch to house interior scene!");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
