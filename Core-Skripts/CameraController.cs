using Godot;
using System;
using System.Runtime.CompilerServices;

public partial class CameraController : Node3D
{

	[Export]
	public float mouseSensitivity = 1.0f;
	[Export]
	public float clampUp = 80.0f;
	[Export]
	public float clampDown = -60.0f;

	private Node3D playerNeck;
	private PlayerController player;



	public override void _Ready()
	{
		// get player Character
		playerNeck = GetTree().GetFirstNodeInGroup("PlayerNeck") as Node3D;
		player = GetTree().GetFirstNodeInGroup("Player") as PlayerController;

		Input.MouseMode = Input.MouseModeEnum.Captured;
	}

	public override void _PhysicsProcess(double delta)
	{
		GlobalPosition = playerNeck.GlobalPosition;
		GlobalRotation = playerNeck.GlobalRotation;
	}

	public override void _UnhandledInput(InputEvent @event)
	{
		if (@event is InputEventMouseMotion mouseMotion)
		{
			float deltaX = mouseMotion.Relative.X * mouseSensitivity;
			float deltaY = mouseMotion.Relative.Y * mouseSensitivity;

			

			player.RotateY(Mathf.DegToRad(-deltaX * player.playerGravityModifier));
			playerNeck.RotateX(Mathf.DegToRad(-deltaY));

		
		}
	}
}
