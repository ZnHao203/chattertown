using Godot;
using System;

public partial class ToHotel : Area2D
{
	[Export]
	private string HotelScenePath = "res://scenes/hotel/hotel.tscn";
	
	// Optional: Store the position where the player should spawn outside
	[Export]
	private Vector2 ExitPosition = new Vector2(0, 0);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		InputPickable = true;
	}
	
	private void _on_to_hotel_input_event(Node viewport, InputEvent @event, long shapeIdx)
	{
		if (@event is InputEventMouseButton mouseEvent)
		{
			if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
			{
				GD.Print("Going to Hotel...");
				GoToHotel();
			}
		}
	}
	
	private void GoToHotel()
	{
		// Optional: Save the exit position for the player
		// You could use an autoload/singleton to store this data
		// or save it to a file if needed
		
		GetTree().ChangeSceneToFile(HotelScenePath);
		ChatBox.Instance.ToggleVisibility();
	}
}