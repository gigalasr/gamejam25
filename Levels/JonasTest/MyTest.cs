using Godot;
using System;

public partial class MyTest : Node3D
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("gravity"))SignalBus.Instance.InvertGravity();
	}
}
