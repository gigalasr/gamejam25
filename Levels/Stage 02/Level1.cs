using Godot;
using System;

public partial class Level1 : Node3D
{

    [Export]
    public Area3D area_3;
    [Export]
    public Area3D area_2;
    [Export]
    public RigidBody3D cube_unmoveable;


    private int cubes_3 = 0;
    private int cubes_2 = 0;
    private bool finished = false;

    public override void _Ready()
    {
        area_3.BodyEntered += OnEnter3;
        area_3.BodyExited += OnLeave3;

        area_2.BodyEntered += OnEnter2;
        area_2.BodyExited += OnLeave2;

    }

    public override void _PhysicsProcess(double delta)
    {
        if (finished) return;
        if (cubes_3 == 3 && cubes_2 == 2)
        {
            RemoveChild(cube_unmoveable);
            cube_unmoveable.QueueFree();
            finished = true;
        }
    }

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("gravity")) SignalBus.Instance.InvertGravity();
    }

    private void OnEnter3(Node3D body)
    {
        if (body is CharacterBody3D) return;
        cubes_3++;
    }

    private void OnLeave3(Node3D body)
    {
        if (body is CharacterBody3D) return;
        cubes_3--;
    }

    private void OnEnter2(Node3D body)
    {
        if (body is CharacterBody3D) return;
        cubes_2++;
    }

    private void OnLeave2(Node3D body)
    {
        if (body is CharacterBody3D) return;
        cubes_2--;
    }
    

}
