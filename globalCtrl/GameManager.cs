using Godot;
using System;

public partial class GameManager : Node
{
    public static GameManager Instance { get; private set; }
    
    [Export]
    public int MaxEnergyPerDay { get; private set; } = 100;
    public int CurrentEnergy { get; private set; }
    public int CurrentDay { get; private set; } = 1;

    public override void _Ready()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        StartNewDay();
    }

    public void StartNewDay()
    {
        CurrentEnergy = MaxEnergyPerDay;
        EmitSignal(SignalName.DayStarted, CurrentDay, CurrentEnergy);
    }

    public bool UseEnergy(int amount)
    {
        if (CurrentEnergy >= amount)
        {
            CurrentEnergy -= amount;
            EmitSignal(SignalName.EnergyChanged, CurrentEnergy);
            return true;
        }
        return false;
    }

    public void AdvanceToNextDay()
    {
        CurrentDay++;
        StartNewDay();
    }

    [Signal]
    public delegate void DayStartedEventHandler(int day, int energy);

    [Signal]
    public delegate void EnergyChangedEventHandler(int newEnergy);
}