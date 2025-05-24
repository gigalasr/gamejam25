using Godot;
using System;

public partial class SignalBus : Node
{
    /// <summary>
    /// This is really stupid, bit idgaf and it works
    /// </summary>

    public static SignalBus Instance { get; private set; }
    public override void _Ready()
    {
        Instance = this;
    }

    /// <summary>
    /// Signal to invert Gravity for player and all subscribed 3dRigidBodys
    /// </summary>
    public event Action OnGravityInvert;
    public void InvertGravity()
    {
        OnGravityInvert?.Invoke();
    }

    /// <summary>
    /// Ignore all Gravity Changing Events
    /// </summary>
    public event Action OnPlayerIgnoreGravityInvert;
    public void PlayerIgnoreGravityInvert()
    {
        OnPlayerIgnoreGravityInvert?.Invoke();
    }

    /// <summary>
    /// Recognise changed Gravity Events
    /// </summary>
    public event Action OnPlayerRecogniseGravityInvert;
    public void PlayerRecogniseGravityInvert()
    {
        OnPlayerRecogniseGravityInvert?.Invoke();
    }
}