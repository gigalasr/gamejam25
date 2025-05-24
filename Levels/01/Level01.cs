using Godot;
using System;
using System.Diagnostics;

public partial class Level01 : Node3D
{
    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("gravity")) SignalBus.Instance.InvertGravity();
    }

    public void _on_area_3d_body_entered(Node3D area)
    {
        GD.Print("Trigger Ignore");
        SignalBus.Instance.PlayerIgnoreGravityInvert();
    }

    public void _on_area_3d_body_exited(Node3D area)
    {
        GD.Print("Trigger COck");
        SignalBus.Instance.PlayerRecogniseGravityInvert();
    }
}
