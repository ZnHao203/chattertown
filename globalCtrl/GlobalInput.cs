using Godot;
using System;

public partial class GlobalInput : Node
{
	[Export]
	private string MapScenePath = "res://map/map.tscn";
	
	// Store the previous scene path to return to
	private string PreviousScenePath;

	public override void _Input(InputEvent @event)
	{
		// Check for Ctrl+M
		if (@event.IsActionPressed("toggle_map"))
		{
			ToggleMap();
		}
	}

	private void ToggleMap()
	{
		var currentScene = GetTree().CurrentScene;
		
		// If we're already in the map, go back to the previous scene
		if (currentScene.SceneFilePath == MapScenePath)
		{
			if (!string.IsNullOrEmpty(PreviousScenePath))
			{
				GetTree().ChangeSceneToFile(PreviousScenePath);
			}
		}
		// Otherwise, store current scene path and go to map
		else
		{
			PreviousScenePath = currentScene.SceneFilePath;
			GetTree().ChangeSceneToFile(MapScenePath);
		}
	}
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
