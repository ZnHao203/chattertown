using Godot;
using System;

public partial class TestGameManager : Node
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		// This should print the current energy (100) and day (1)
		GD.Print($"Current Energy: {GameManager.Instance.CurrentEnergy}");
		GD.Print($"Current Day: {GameManager.Instance.CurrentDay}");

		// Wait a second, then use some energy
		var timer = GetTree().CreateTimer(1.0);
		timer.Timeout += () =>
		{
			// GameManager.Instance.UseEnergy(20);
		};
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
