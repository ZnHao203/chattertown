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

public partial class Kid3 : Character
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
			"*gazing at clouds* Did you know the clouds look like dancing unicorns today? They're having a tea party up there...",
			() => !GameManager.Instance.HasTalkedTo("Kid3")
		)
		.AddChoice("Hello there!", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"*twirling around* Your voice sounds like rainbow bells! Do you collect magical stories too?");
			GameManager.Instance.RecordCharacterInteraction("Kid3");
		})
		.AddChoice("Have you seen anything strange?", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"Strange? *eyes sparkling* Like the fairy lights dancing in the old house windows at midnight? Or the singing shadows under the moon?");
			GameManager.Instance.RecordCharacterInteraction("Kid3");
		}));

		// Return visit dialogue with AI generation
		_dialogueLines.Add(new DialogueLine(
			"*drawing patterns in the air* Oh! You're back! I was just painting stories with stardust..."
		)
		.AddChoice("What's new?", "", async () => 
		{
			string aiResponse = await GenerateDialogue();
			GameManager.Instance.DisplayDialogue(CharacterName, aiResponse);
		})
		.AddChoice("Just passing by!", "", () => 
		{
			GameManager.Instance.DisplayDialogue(CharacterName, 
				"*dreamily* The wind will carry your footsteps to the secret garden where memories bloom like flowers...");
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
						content = "Generate a dreamy and creative child-like dialogue line about imagining a fantasy version of the town. Keep it whimsical and imaginative, maximum 30 words."
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
				return "*lost in thought* The butterflies whispered secrets to me, but they spoke in rainbow colors...";
			}
			catch (Exception e)
			{
				GD.PrintErr($"Error calling Bedrock: {e.Message}");
				return "My imaginary friend says the town is like a music box at night, full of dancing melodies...";
			}
		}
		catch (Exception e)
		{
			GD.PrintErr($"Error generating dialogue: {e.Message}");
			return "*spinning in circles* Yesterday, I saw the playground transform into a crystal castle under the sunset!";
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