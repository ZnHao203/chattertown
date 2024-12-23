# ChatterTown

A social game built with Godot 4 and C#.

## Setup Requirements

1. Install Godot 4.x (.NET version)
   - Download from the official Godot website
   - Choose the .NET version that includes C# support

2. Install .NET SDK 6.0 or later
   - Download from the official Microsoft website

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
- **Ctrl + M** - Open/Close Map
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
