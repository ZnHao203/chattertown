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

public partial class Kid2 : Character
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
			"Oh look, another detective wannabe! *slow clap* This should be entertaining...",
			() => !GameManager.Instance.HasTalkedTo("Kid2")
		)
		.AddChoice("Hello there!", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Wow, such an original greeting! Did you come up with that all by yourself?");
			GameManager.Instance.RecordCharacterInteraction("Kid2");
		})
		.AddChoice("Have you seen anything strange?", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Define 'strange.' The adults running around like headless chickens, or the actual weird stuff?");
			GameManager.Instance.RecordCharacterInteraction("Kid2");
		}));

		// Return visit dialogue with AI generation
		_dialogueLines.Add(new DialogueLine(
			"Well, well, well... if it isn't our town's finest detective! *exaggerated bow*"
		)
		.AddChoice("What's new?", "", async () => 
		{
			string aiResponse = await GenerateDialogue();
			GameManager.Instance.DisplayDialogue(CharacterName, aiResponse);
		})
		.AddChoice("Just passing by!", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Sure you are! Don't let all those mysterious clues trip you on your way out!");
		}));
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
						content = "Generate a sarcastic and witty child-like dialogue line about the recent strange events in the town. Keep it humorous yet slightly mocking, maximum 30 words."
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
				return "Oh, you want to know what I saw? *dramatic pause* Sorry, my gossip subscription expired!";
			}
			catch (Exception e)
			{
				GD.PrintErr($"Error calling Bedrock: {e.Message}");
				return "My detective senses are tingling... or maybe that's just brain freeze from too much ice cream.";
			}
		}
		catch (Exception e)
		{
			GD.PrintErr($"Error generating dialogue: {e.Message}");
			return "You should see what's happening at the playground! *winks* Or maybe you shouldn't... your detective skills might not be up for it.";
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