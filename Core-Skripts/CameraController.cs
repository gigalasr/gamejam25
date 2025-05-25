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
    [Export]
    public float bobbingAmplitude = 1.0f;
    [Export]
    public float bobbingFrquency = 1.0f;

    private Node3D playerNeck;
    private PlayerController player;

    private float bob = 0;


    public override void _Ready()
    {
        // get player Character
        playerNeck = GetTree().GetFirstNodeInGroup("PlayerNeck") as Node3D;
        player = GetTree().GetFirstNodeInGroup("Player") as PlayerController;

        Input.MouseMode = Input.MouseModeEnum.Captured;
    }

    public override void _PhysicsProcess(double delta)
    {
        // head bobbing
        bob += (float)delta * player.currentSpeed * (player.OnFloor() ? 1 : 0);
        Vector3 pos = Vector3.Zero;
        pos.Y = Mathf.Sin(bob * bobbingFrquency) * bobbingAmplitude;
        pos.X = Mathf.Cos(bob * bobbingFrquency / 2) * bobbingAmplitude;

        GlobalPosition = playerNeck.GlobalPosition + pos;
        GlobalRotation = playerNeck.GlobalRotation;

    }

    public override void _UnhandledInput(InputEvent @event)
    {
        if (@event is InputEventMouseMotion mouseMotion)
        {
            float deltaX = mouseMotion.Relative.X * mouseSensitivity;
            float deltaY = mouseMotion.Relative.Y * mouseSensitivity;

            player.RotateY(Mathf.DegToRad(-deltaX * player.playerGravityModifier));
            //playerNeck.RotateX(Mathf.DegToRad(-deltaY));
            Vector3 rotation = playerNeck.Rotation;
            rotation.X -= Mathf.DegToRad(deltaY);
            rotation.X = Mathf.Clamp(rotation.X, Mathf.DegToRad(clampDown), Mathf.DegToRad(clampUp));
            playerNeck.Rotation = rotation;

        }
        else if (@event.IsActionPressed("esc"))
        {
            Input.MouseMode = Input.MouseMode == Input.MouseModeEnum.Captured ? Input.MouseModeEnum.Visible : Input.MouseModeEnum.Captured;
        }
    }

    public void FlipCamera()
    {
        Vector3 rotation = playerNeck.Rotation;
        rotation.X = Mathf.Clamp(-rotation.X, Mathf.DegToRad(clampDown), Mathf.DegToRad(clampUp));
        var tween = GetTree().CreateTween().BindNode(this);
        tween.TweenProperty(playerNeck, "rotation", rotation, 1.0f).SetEase(Tween.EaseType.In);
        tween.Play();
    }
}
