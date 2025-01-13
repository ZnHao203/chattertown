using Godot;
using System;
using System.Collections.Generic;
using System.Linq;


public partial class DefaultNpc2 : Character
{
    private Dictionary<string, (Queue<(string, string)> Dialogue, Func<bool> Condition)> _dialogueTopics;
    private List<string> _availableNames;
    private string _currentSpeaker1;
    private string _currentSpeaker2;
    private Queue<(string, string)> _currentDialogueQueue;
    private bool _isInConversation;

    public override void _Ready()
    {
        CharacterName = "TwoNPCChat";
        base._Ready();
        InitializeDialogues();
    }

    protected override void InitializeDialogues()
    {
        _dialogueTopics = new Dictionary<string, (Queue<(string, string)>, Func<bool>)>();
        _availableNames = new List<string> 
        { 
            "Kenny", 
            "Alex", 
            "Sarah", 
            "Detective Morris",
            "Officer Chen" 
        };

        InitializeCameronHint();
        InitializePaulHint();
		InitializeGusHint();
		InitializeDefaultChat();
    }

	private void InitializeDefaultChat()
	{
		var dialogue = new Queue<(string, string)>();
		dialogue.Enqueue(("Did you try that new coffee place downtown?", 
						"Yeah, their prices are ridiculous! Seven dollars for a latte?"));
		
		dialogue.Enqueue(("The weather's been weird lately.", 
						"I know! One day it's sunny, the next it's pouring. Can't even plan a proper picnic."));
		
		dialogue.Enqueue(("My kid won't stop talking about that new video game.", 
						"The one with the dancing animals? Mine too! It's all they talk about at school."));
		
		dialogue.Enqueue(("Have you seen how many potholes there are on Main Street?", 
						"Tell me about it! I had to get my suspension fixed last month."));
		
		dialogue.Enqueue(("The farmer's market is getting better every weekend.", 
						"Those organic tomatoes from the Johnson farm are amazing!"));
		
		// Add condition for default chat - always available
		_dialogueTopics["Default Chat"] = (dialogue, () => true);
	}

    private void InitializeCameronHint()
    {
        var dialogue = new Queue<(string, string)>();
        dialogue.Enqueue(("Hey, I'm heading to the Photographic Equipment store at the Town Center, you wanna join?",
		 					"No not really, I heard Bill is going there these days, I don't wanna bump into him — sorry mate."));
        
        // Add condition for Cameron hint
        _dialogueTopics["Cameron Hint"] = (dialogue, () => !GameManager.Instance.HasTalkedTo("CameronDone"));
    }


    private void InitializePaulHint()
    {
        var dialogue = new Queue<(string, string)>();
        dialogue.Enqueue(("I heard old Paul just broke his leg last week, I wonder if he is still able to bake his bread every day?", 
						"Probably his old wife is doing the work for him I guess? My wife bought buns from Paul's this morning and told me Paul's son, the local policeman, just found a dead body somewhere close to the mountains in the early morning!"));
        dialogue.Enqueue(("Holy crap! A dead body?", 
						"Yeah, and I was told that it is Bill!"));
        dialogue.Enqueue(("Bill? The influencer, Bill? Oh my, I love his videos! Poor guy.", 
						"I know, right? Maybe it's my wife making up stories, I shall go and visit old Paul later…"));
        
        // Add condition for Paul hint
        _dialogueTopics["Paul Hint"] = (dialogue, () => !GameManager.Instance.HasTalkedTo("PaulDone"));
    }

	private void InitializeGusHint()
    {
        var dialogue = new Queue<(string, string)>();
        dialogue.Enqueue(("Have you guys heard? About that dead guy, the police brought in two people for questioning, and one of them just got released—he's completely out of it.", 
						"I saw that! The guy didn't even want to leave, shouting for the police to protect him. They had to drag him out."));
        dialogue.Enqueue(("Holy crap! A dead body?", 
						"Yeah, and I was told that it is Bill!"));
        dialogue.Enqueue(("That man...he looked really aggressive... I heard he pushed the victim. Could it have been a crime of passion?", 
						"I saw him walk into the bar; I didn't even want to go in after that."));
        
        // Add condition for Paul hint
        _dialogueTopics["Gus Hint"] = (dialogue, () => GameManager.Instance.CurrentDay >= 2 && !GameManager.Instance.HasTalkedTo("GusDone"));
    }


    protected override void StartDialog()
    {
        if (_isInConversation)
        {
            ContinueConversation();
            return;
        }

        // Filter available topics based on conditions
        var availableTopics = _dialogueTopics
            .Where(kvp => kvp.Value.Condition())
            .ToList();

        if (availableTopics.Count == 0)
        {
            GD.Print("No available topics");
            return;
        }

        // Select a random available topic
        var randomTopic = availableTopics[new Random().Next(availableTopics.Count)];
        
        // Select two random different speakers
        SelectRandomSpeakers();
        
        // Start new conversation
        _currentDialogueQueue = new Queue<(string, string)>(randomTopic.Value.Dialogue);
        _isInConversation = true;
        ContinueConversation();
    }

    private void SelectRandomSpeakers()
    {
        var shuffledNames = _availableNames.OrderBy(x => Guid.NewGuid()).ToList();
        _currentSpeaker1 = shuffledNames[0];
        _currentSpeaker2 = shuffledNames[1];
    }

    private void ContinueConversation()
    {
        if (_currentDialogueQueue == null || _currentDialogueQueue.Count == 0)
        {
            _isInConversation = false;
            return;
        }

        var (line1, line2) = _currentDialogueQueue.Dequeue();
        
        // Display both lines
        GameManager.Instance.DisplayDialogue(_currentSpeaker1, line1);
        GameManager.Instance.DisplayDialogue(_currentSpeaker2, line2);
    }

    public void EndConversation()
    {
        _isInConversation = false;
        _currentDialogueQueue = null;
    }
}
