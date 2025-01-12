using Godot;
using System;

public partial class Home : Node
{

    private Sprite2D dayBackground;
    private Sprite2D nightBackground;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		dayBackground = GetNode<Sprite2D>("DayBackground");
        nightBackground = GetNode<Sprite2D>("NightBackground");
        
		if (!GameManager.Instance.isNight) {
			SetDayBackground();
		} else {
			SetNightBackground();
		}

	}

    private void SetDayBackground()
    {
        dayBackground.Visible = true;
        nightBackground.Visible = false;
    }

    private void SetNightBackground()
    {
        dayBackground.Visible = false;
        nightBackground.Visible = true;
    }
}
