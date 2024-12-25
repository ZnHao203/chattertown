using Godot;
using System;
using System.Threading.Tasks;

public partial class ChatBox : Control
{
	public static ChatBox Instance { get; private set; }
	
	private RichTextLabel _messageDisplay;
	private ScrollContainer _scrollContainer;
	private const int MAX_MESSAGES = 50;
		
	public override void _EnterTree()
	{
		base._EnterTree();
		Instance = this;
		GD.Print("ChatBox singleton initialized"); // Debug print
	}

	public override void _ExitTree()
	{
		base._ExitTree();
		if (Instance == this)
			Instance = null;
	}
	
	public override void _Ready()
	{
		_messageDisplay = GetNode<RichTextLabel>("Panel/ScrollContainer/VBoxContainer/RichTextLabel");
		_scrollContainer = GetNode<ScrollContainer>("Panel/ScrollContainer");
		
		
		// Set up RichTextLabel properties
		_messageDisplay.BbcodeEnabled = true;
		
		// Option 1: Load a custom font
		var fontResource = GD.Load<FontFile>("res://assets/07558_CenturyGothic.ttf");
		_messageDisplay.AddThemeFontOverride("normal_font", fontResource);
		
		// Set text color if needed
		_messageDisplay.AddThemeColorOverride("default_color", Colors.White);
		
		// Make sure the background panel is visible
		var panel = GetNode<Panel>("Panel");
		panel.AddThemeStyleboxOverride("panel", new StyleBoxFlat 
		{ 
			BgColor = new Color(0, 0, 0, 0.8f) // Semi-transparent black
		});
		// Set initial properties
		//CustomMinimumSize = new Vector2(300, 600);
		//AnchorRight = 1;
		//AnchorBottom = 1;
		
		// Set initial size and position
		//CustomMinimumSize = new Vector2(300, 200); // Make it smaller
		//Position = new Vector2(10, 10); // Position it away from the top-left
		//Size = new Vector2(300, 200);   // Set a specific size
		
		// Make sure it's not blocking input to other nodes
		MouseFilter = MouseFilterEnum.Ignore;
		
		GD.Print("ChatBox Ready"); // Debug print
		
		//// Optional: Start hidden
		//Visible = false;
	}

	public void AddMessage(string speaker, string message)
	{
		// Make visible when receiving a message
		//Visible = true;
		GD.Print($"in chatbox.cs, Attempting to display dialogue: {speaker}: {message}"); // Debug print
		string formattedMessage = $"[b]{speaker}:[/b] {message}\n";
		//_messageDisplay.Text += formattedMessage;
		// Use AppendText instead of += for better performance
		_messageDisplay.AppendText(formattedMessage);
		
		
		// Auto-scroll to bottom
		_scrollContainer.ScrollVertical = (int)_scrollContainer.GetVScrollBar().MaxValue;
	}
	
	// Add method to toggle visibility
	public void ToggleVisibility()
	{
		Visible = !Visible;
	}
	
	//private RichTextLabel _messageDisplay;
	//private ScrollContainer _scrollContainer;
	//private const int MAX_MESSAGES = 50; // Limit number of messages
	//
	//public override void _Ready()
	//{
		//_messageDisplay = GetNode<RichTextLabel>("Panel/ScrollContainer/VBoxContainer/RichTextLabel");
		//_scrollContainer = GetNode<ScrollContainer>("Panel/ScrollContainer");
		//
		//// Set initial properties
		//CustomMinimumSize = new Vector2(300, 600); // Adjust size as needed
		//AnchorRight = 1;
		//AnchorBottom = 1;
		//
		//// Position it on the right
		//Position = new Vector2(GetViewportRect().Size.X - CustomMinimumSize.X, 0);
		//
		////// Style the panel
		////var panel = GetNode<Panel>("Panel");
		////var styleBox = new StyleBoxFlat();
		////styleBox.BgColor = new Color(0, 0, 0, 0.7f); // Semi-transparent black
		////styleBox.CornerRadius = new Vector4I(10, 10, 10, 10); // Rounded corners
		////panel.AddThemeStyleboxOverride("panel", styleBox);
	//}
//
	//public void AddMessage(string speaker, string message)
	//{
		//string formattedMessage = $"[b]{speaker}:[/b] {message}\n";
		//_messageDisplay.Text += formattedMessage;
		//
		//// Auto-scroll to bottom
		//_scrollContainer.ScrollVertical = (int)_scrollContainer.GetVScrollBar().MaxValue;
		//
		//// Limit message history
		//if (_messageDisplay.GetLineCount() > MAX_MESSAGES)
		//{
			//RemoveOldestMessage();
		//}
	//}
//
	//private void RemoveOldestMessage()
	//{
		//int firstNewline = _messageDisplay.Text.IndexOf('\n');
		//if (firstNewline != -1)
		//{
			//_messageDisplay.Text = _messageDisplay.Text.Substring(firstNewline + 1);
		//}
	//}

	//// Call this to clear the chat
	//public void ClearChat()
	//{
		//_messageDisplay.Text = "";
	//}
	
	//// Add fade effect for new messages
	//public async void AddMessageWithFade(string speaker, string message)
	//{
		//var originalColor = _messageDisplay.ModulateColor;
		//_messageDisplay.ModulateColor = new Color(1, 1, 1, 0);
		//
		//AddMessage(speaker, message);
		//
		//// Fade in
		//var tween = CreateTween();
		//tween.TweenProperty(_messageDisplay, "modulate:a", 1.0, 0.3);
		//await ToSignal(tween, "finished");
	//}
	//
	//// Add typing effect
	//public async Task ShowTypingEffect(string speaker, string message, float charDelay = 0.05f)
	//{
		//string currentText = "";
		//_messageDisplay.Text += $"[b]{speaker}:[/b] ";
		//
		//foreach (char c in message)
		//{
			//currentText += c;
			//_messageDisplay.Text += c;
			//await ToSignal(GetTree().CreateTimer(charDelay), "timeout");
		//}
		//
		//_messageDisplay.Text += "\n";
	//}

	//// Called every frame. 'delta' is the elapsed time since the previous frame.
	//public override void _Process(double delta)
	//{
	//}
}
