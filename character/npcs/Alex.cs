using Godot;
using System;

public partial class Alex : Character
{
	[Export]
	private string[] _dialogLines = new string[] 
	{ 
		"Nice weather today!",
		"How are you doing?",
		"Just another day in town..."
	};

	protected override void StartDialog()
	{
		// Pick random dialogue line
		var randomLine = _dialogLines[new Random().Next(_dialogLines.Length)];
		GD.Print($"{CharacterName}: {randomLine}");
		GameManager.Instance.DisplayDialogue(CharacterName, randomLine);
		//var chatBox = GameManager.Instance.GetChatBox();
		//await chatBox.ShowTypingEffect(CharacterName, "Hello there! How can I help you today?");
	}
}