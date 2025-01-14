using Godot;
using System;

public partial class OpenScene : Node
{
	private Label messageLabel;
    private PanelContainer textBox;

    public override void _Ready()
    {
        messageLabel = GetNode<Label>("TextBox/MarginContainer/MessageLabel");
        textBox = GetNode<PanelContainer>("TextBox");

        // Style the text box
        StyleTextBox();

        // Set the EA message
        messageLabel.Text = "Welcome to Chattertown!\n\n" +
							"Nestled at the foot of a small hill lies the town you call home. Familiar yet full of mysteries, this quaint town invites you to explore its corners and converse with the locals.\n\n" +
							"How to Play:\n" +
							"- Use the left and right arrow keys to move around.\n" +
							"- Click on characters to interact or continue conversations.\n" +
							"- During dialogues, if you see choices numbered, click on the corresponding number to make your selection.\n\n" +
							"Embark on your adventure and uncover the stories hidden within the town!\n\n" +
							"Click on the white area to continue.";

		// start with no chatbox
		ChatBox.Instance.ToggleVisibility();
    }

    private void StyleTextBox()
    {
        // Style the label
        messageLabel.HorizontalAlignment = HorizontalAlignment.Left;
        messageLabel.VerticalAlignment = VerticalAlignment.Center;
        messageLabel.AutowrapMode = TextServer.AutowrapMode.WordSmart;
        
        // Optional: Set custom font size
        // messageLabel.AddThemeConstantOverride("font_size", 36);

        // Optional: Style the panel container
        textBox.AddThemeStyleboxOverride("panel", new StyleBoxFlat
        {
            BgColor = new Color(0, 0, 0, 0.8f), // Semi-transparent black
            BorderWidthBottom = 2,
            BorderWidthTop = 2,
            BorderWidthLeft = 2,
            BorderWidthRight = 2,
            BorderColor = new Color(1, 1, 1, 1), // White border
            CornerRadiusBottomLeft = 10,
            CornerRadiusBottomRight = 10,
            CornerRadiusTopLeft = 10,
            CornerRadiusTopRight = 10,
            ContentMarginLeft = 20,
            ContentMarginRight = 20,
            ContentMarginTop = 20,
            ContentMarginBottom = 20
        });

		// Optionally, you can also set the text color explicitly
    	messageLabel.AddThemeColorOverride("font_color", Colors.White);
    }

	// on click, starts new day
	private void _on_area_2d_input_event(Node viewport, InputEvent @event, long shapeIdx)
    {
        if (@event is InputEventMouseButton mouseEvent)
        {
            if (mouseEvent.ButtonIndex == MouseButton.Left && mouseEvent.Pressed)
            {
                // make sure chatbox is restored
				ChatBox.Instance.ToggleVisibility();

				// Call StartNewDay when left mouse button is clicked
                GameManager.Instance.StartNewDay();
            }
        }
    }

}
