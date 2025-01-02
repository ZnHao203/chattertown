using Godot;
using System;

public partial class Player : Area2D
{
	[Export]
	public int Speed { get; set; } = 400; // How fast the player will move (pixels/sec).

	//public Vector2 ScreenSize; // Size of the game window.
	private Vector2 _minBoundary;
	private Vector2 _maxBoundary;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		//ScreenSize = GetViewportRect().Size; // THIS IS THE BUG
		//ScreenSize = new Vector2(10000, 1000);
		
		// Try to find the background node
		var background = GetParent().GetNode<Sprite2D>("../Background");
		
		// Get the root node of the current scene
		//var root = GetTree().Root.GetChild(0); // First child of root is the current scene
		//var background = root.GetNode<Sprite2D>("Background");
		
		if (background != null && background.Texture != null)
		{
			// Calculate boundary considering background position and size
			var textureSize = background.Texture.GetSize();
			var scale = background.Scale;
			var totalSize = textureSize * scale;
			
			// Calculate boundaries based on background position and size
			//_minBoundary = background.Position - (totalSize / 2); // If background is centered
			//_maxBoundary = _minBoundary + totalSize;

			_minBoundary = Vector2.Zero;
			_maxBoundary = totalSize;
			_maxBoundary.X -= 1200;  // temporary values
			GD.Print($"Background position: {background.Position}");
			GD.Print($"Min boundary: {_minBoundary}");
			GD.Print($"Max boundary: {_maxBoundary}");
		}
		else
		{
			// Fallback boundaries if background is not found
			_minBoundary = Vector2.Zero;
			_maxBoundary = new Vector2(10000, 1000);
			GD.PrintErr("Background node not found, using fallback boundaries");
		}
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		var velocity = Vector2.Zero; // The player's movement vector.

		if (Input.IsActionPressed("move_right"))
		{
			velocity.X += 1;
		}

		if (Input.IsActionPressed("move_left"))
		{
			velocity.X -= 1;
		}
		
		 var animatedSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");

		if (velocity.Length() > 0)
		{
			velocity = velocity.Normalized() * Speed;
			animatedSprite2D.Play();
		}
		else
		{
			animatedSprite2D.Stop();
		}
		
		// Debug position
		//GD.Print($"Player position: {Position}");
		
		Position += velocity * (float)delta;
		//Position = new Vector2(
			//x: Mathf.Clamp(Position.X, 0, ScreenSize.X),
			//y: Mathf.Clamp(Position.Y, 0, ScreenSize.Y)
		//);
		Position = new Vector2(
			x: Mathf.Clamp(Position.X, _minBoundary.X, _maxBoundary.X),
			y: Mathf.Clamp(Position.Y, _minBoundary.Y, _maxBoundary.Y)
		);
		
		if (velocity.X < 0)
		{
			animatedSprite2D.FlipH = true;
		}
		else
		{
			animatedSprite2D.FlipH = false;
		}
	}

}
