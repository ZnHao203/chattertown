# ChatterTown

A social game built with Godot 4 and C#.

## Setup Requirements

1. Install Godot 4.x (.NET version)
   - Download from the official Godot website
   - Choose the .NET version that includes C# support

2. Install .NET SDK 6.0 or later
   - Download from the official Microsoft website

3. Install Amazon Bedrock
dotnet add package dotenv.net
dotnet add package AWSSDK.BedrockRuntime

If you want to use Amazon Bedrock to interact with characters in a more interesting way, please enter your aws access key and secret access key.
You need to have access to anthropic claude 3.5 Haiku model. us.anthropic.claude-3-5-haiku-20241022-v1:0
AWS_ACCESS_KEY_ID=your_access_key
AWS_SECRET_ACCESS_KEY=your_secret_key
AWS_DEFAULT_REGION=us-east-2

## Running the Project

1. Clone this repository
2. Open Godot Engine
3. Click "Import" and select the project.godot file from the cloned directory
4. Click "Run Project" (or press F5) to start the game

## Game Controls

### Movement
- **Left Arrow** - Move character left
- **Right Arrow** - Move character right

### Navigation
- **Left Click** - Interact with buildings and objects
- **M** - Open/Close Map
- **ESC** - Exit current building/Return to previous scene

### Scene Transitions
- Click on houses to enter them
- Click on doors inside houses to exit
- Use the map to fast travel between locations

## Project Structure

- `main/Main.tscn` - Main neighborhood scene
- `map/map.tscn` - World map scene
- `home/home.tscn` - Interior scene for houses

## Development Notes

The project uses:
- Godot 4.x
- C# as the scripting language
- Scene-based architecture for different game areas

## Contributing

1. Fork the repository
2. Create a new branch for your feature
3. Submit a pull request
