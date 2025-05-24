using Godot;
using System;

public partial class GravityInverter : RigidBody3D
{
	public override void _Ready()
	{
		GravityScale = 1;
		//SignalBus.GravityInvertEventHandler += InvertGravity;
		SignalBus.Instance.OnGravityInvert += GravityInvert;
	}

	private void GravityInvert()
	{
		GravityScale *= -1;
	}

	public override void _PhysicsProcess(double delta)
	{
		if (Sleeping)
		{
			Sleeping = false;
		}
	}
}
