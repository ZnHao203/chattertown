using Godot;
using System;

public partial class EndScene : Node
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
        messageLabel.Text = "Thank you for playing!\n\n" +
							"You've reached the current end of our story, but the journey doesn't stop here. " + 
							"Our game is still in its Early Access phase, and we are committed to enhancing and expanding the narrative. " + 
							"Stay tuned for deeper plot developments and the unraveling of the murderer's elusive secrets.\n\n" + 
							"For updates on new content and features, keep an eye on our GitHub posts. " + 
							"We're excited to share what comes next and truly appreciate your feedback.\n\n" + 
							"We hope you've enjoyed the story so far and had some fun along the way! " + 
							"Thank you for your time and for being a part of our game's community.";
    }

    private void StyleTextBox()
    {
        // Style the label
        messageLabel.HorizontalAlignment = HorizontalAlignment.Center;
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
}
