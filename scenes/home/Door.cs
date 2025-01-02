using Godot;
using System;

public partial class Door : Area2D
{
	[Export]
	private string OutsideScenePath = "res://scenes/main/main.tscn";
	private string CutScenePath = "res://scenes/cutscenes/day_1_cutscene.tscn";
	
	// Optional: Store the position where the player should spawn outside
	[Export]
	private Vector2 ExitPosition = new Vector2(0, 0);
	
	

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InputPickable = true;
		_isCutscenePlayed = GameManager.Instance.IsCutscenePlayed;
		_isUnlocked = GameManager.Instance.IsHomeDoorUnlocked;
		
		// Only connect to signal if door hasn't been unlocked yet
		if (!_isUnlocked)
		{
			GameManager.Instance.DoorUnlocked += OnDoorUnlocked;
		}
	}

	
	
	private void _on_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				if (_isUnlocked){
					GD.Print("Exiting house...");
					ExitToOutside();
				}
				else 
				{
					GameManager.Instance.DisplayDialogue("Me", "I should gather everything I need first...");
				}
				
			}
		}
	}

	// TODO: do i even need this function?
	public override void _ExitTree()
	{
		// Clean up signal connection when node is removed
		GameManager.Instance.DoorUnlocked -= OnDoorUnlocked;
	}
	
	private void ExitToOutside()
	{
		// Optional: Save the exit position for the player
		// You could use an autoload/singleton to store this data
		// or save it to a file if needed
		if (!_isCutscenePlayed)
		{
			_isCutscenePlayed = true;
			GameManager.Instance.IsCutscenePlayed = true;
			GetTree().ChangeSceneToFile(CutScenePath);
		}
		else 
		{
			GetTree().ChangeSceneToFile(OutsideScenePath);
		}
		
		
	}

	// day1 features
	private bool _isUnlocked = false;
	private bool _isCutscenePlayed = false;
	private void OnDoorUnlocked()
	{
		_isUnlocked = true;
		
	}
	



}
