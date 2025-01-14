using Godot;
using System;

public partial class Hotel : Node
{
	private Node2D _ed;

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        // Get reference to Gus node
        _ed = GetNode<Node2D>("YSort/Ed");
        
        // Set Gus's visibility based on current day
        if (_ed != null)
        {
            _ed.Visible = GameManager.Instance.CurrentDay >= 2;
        }
    }

}
