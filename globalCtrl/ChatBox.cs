using Godot;
using System;
using System.Threading.Tasks;

public partial class ChatBox : Control
{
	public static ChatBox Instance { get; private set; }
	
	private RichTextLabel _messageDisplay;
	private ScrollContainer _scrollContainer;
	private VBoxContainer _vBoxContainer;
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
		_vBoxContainer = GetNode<VBoxContainer>("Panel/ScrollContainer/VBoxContainer");
        
		
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
		
		ZIndex = 100;

		
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
		
		
		// // Auto-scroll to bottom
		// _scrollContainer.ScrollVertical = (int)_scrollContainer.GetVScrollBar().MaxValue;

		// // Force layout update and scroll
        // CallDeferred(nameof(EnsureScroll));

		// Immediate scroll attempt
        EnsureScroll();
        
        // Deferred scroll attempt
        CallDeferred(nameof(EnsureScroll));
        
        // Multiple delayed scroll attempts
        CreateScrollTimer(0.05);
        CreateScrollTimer(0.1);
	}

	 private void CreateScrollTimer(double delay)
    {
        var timer = GetTree().CreateTimer(delay);
        timer.Timeout += EnsureScroll;
    }

	private void EnsureScroll()
    {
        // Force layout update
        _messageDisplay.ForceUpdateTransform();
        _vBoxContainer.ForceUpdateTransform();
        _scrollContainer.ForceUpdateTransform();

        // Get the scroll bar and ensure it's at the bottom
        var vscroll = _scrollContainer.GetVScrollBar();
        if (vscroll != null)
        {
            _scrollContainer.ScrollVertical = (int)vscroll.MaxValue;
        }

        // Double-check scroll following is enabled
        _messageDisplay.ScrollFollowing = true;

		// Force the scroll container to update
        _scrollContainer.QueueRedraw();
    }
	
	// Add method to toggle visibility
	public void ToggleVisibility()
	{
		Visible = !Visible;
	}
	

}
