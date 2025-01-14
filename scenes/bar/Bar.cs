using Godot;
using System;

public partial class Bar : Node
{
	private Node2D _gus;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Get reference to Gus node
        _gus = GetNode<Node2D>("YSort/Gus");
        
        // Set Gus's visibility based on current day
        if (_gus != null)
        {
            _gus.Visible = GameManager.Instance.CurrentDay >= 2;
        }
    }
}
