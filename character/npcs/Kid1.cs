using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;
using Amazon.BedrockRuntime;
using Amazon.BedrockRuntime.Model;
using System.Linq;
using dotenv.net;

public partial class Kid1 : Character
{
    private AmazonBedrockRuntimeClient _bedrockClient;

    public override void _Ready()
    {
        CharacterName = "Kid"; 
        base._Ready();
        InitializeDialogues();

		DotEnv.Load();  // Will load from .env file in project root

		// Then you can create your client without credentials
		var config = new AmazonBedrockRuntimeConfig
		{
			RegionEndpoint = Amazon.RegionEndpoint.USEast2
		};

		_bedrockClient = new AmazonBedrockRuntimeClient(config);
    }

    protected override void InitializeDialogues()
    {
        _dialogueLines.Clear();

		// First meeting dialogue
		_dialogueLines.Add(new DialogueLine(
			"Ugh, another grown-up! Can't you see I'm busy?",
			() => !GameManager.Instance.HasTalkedTo("Kid1")
		)
		.AddChoice("Hello there!", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"I don't want to play with YOU. You're probably boring like all the others.");
			GameManager.Instance.RecordCharacterInteraction("Kid1");
		})
		.AddChoice("Have you seen anything strange?", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Maybe I have, maybe I haven't. Not telling YOU anything!");
			GameManager.Instance.RecordCharacterInteraction("Kid1");
		}));

		// Return visit dialogue with AI generation
		_dialogueLines.Add(new DialogueLine(
			"Oh great, you're back. *rolls eyes*"
		)
		.AddChoice("What's new?", "", async () => 
		{
			string aiResponse = await GenerateDialogue();
			GameManager.Instance.DisplayDialogue(CharacterName, aiResponse);
		})
		.AddChoice("Just passing by!", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Good! Keep walking! This is MY spot anyway!");
		}));

		// Debug prints
		GD.Print($"Kid1 has {_dialogueLines.Count} dialogues");
		foreach (var dialogue in _dialogueLines)
		{
			GD.Print($"Dialogue: {dialogue.Text}");
			GD.Print($"Number of choices: {dialogue.Choices.Count}");
		}
    }

    private async Task<string> GenerateDialogue()
	{
		try
		{
			var promptJson = JsonSerializer.Serialize(new
			{
				max_tokens = 100,
				messages = new[]
				{
					new
					{
						role = "user",
						content = "Generate a random child-like dialogue line about playing in a small town. Keep it short, angry, and unsatisfying, maximum 30 words."
					}
				},
				anthropic_version = "bedrock-2023-05-31"
			});

			var request = new InvokeModelRequest
			{
				ModelId = "arn:aws:bedrock:us-east-2:897729107368:inference-profile/us.anthropic.claude-3-5-haiku-20241022-v1:0",
				
				Body = new MemoryStream(
					System.Text.Encoding.UTF8.GetBytes(promptJson)
				)
			};

			try
			{
				var response = await _bedrockClient.InvokeModelAsync(request);
				using var reader = new StreamReader(response.Body);
				var jsonResponse = await reader.ReadToEndAsync();
				using JsonDocument document = JsonDocument.Parse(jsonResponse);
				var root = document.RootElement;
				return root.GetProperty("content")[0].GetProperty("text").GetString();
			}
			catch (AccessDeniedException)
			{
				GD.PrintErr("Access denied to Claude 3.5 Haiku. Please enable model access in AWS Bedrock console.");
				return "I KNOW something, but I'm not telling you! Go away!";
			}
			catch (Exception e)
			{
				GD.PrintErr($"Error calling Bedrock: {e.Message}");
				return "Stop bothering me! I don't want to talk right now!";
			}
		}
		catch (Exception e)
		{
			GD.PrintErr($"Error generating dialogue: {e.Message}");
			return "Whatever happened at the playground is none of your business! *crosses arms*";
		}
	}
    

    protected override void StartDialog()
    {
        GD.Print("Starting Kid1's dialogue");
        
        var availableDialogues = _dialogueLines
            .Where(d => d.Condition())
            .ToList();

        GD.Print($"Found {availableDialogues.Count} available dialogues");

        if (availableDialogues.Count == 0)
        {
            GD.Print("No available dialogues, showing fallback");
            GameManager.Instance.DisplayDialogue(CharacterName, "Hi!");
            return;
        }

        var randomLine = availableDialogues[new Random().Next(availableDialogues.Count)];
        GameManager.Instance.DisplayDialogue(CharacterName, randomLine);
    }
}