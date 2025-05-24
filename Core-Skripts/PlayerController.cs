using Godot;
using System;

public partial class PlayerController : CharacterBody3D
{
    [ExportCategory("Movement")]
    [Export]
    public float Speed = 5.0f;
    [Export]
    public float runningSpeed = 7.5f;
    [Export]
    public float JumpVelocity = 4.5f;
    [ExportCategory("Actions")]
    [Export]
    public bool canJump = true;
    [Export]
    public bool canDoubleJump = false;


    private bool hasDoubleJumped = false;

    private float gravityModifier = 1;
    public float playerGravityModifier = 1;
    private bool ignoreGravityInvert = false;

    private Label debugLabel;
    private CameraController camera;

    public override void _Ready()
    {
        SignalBus.Instance.OnGravityInvert += InvertGravity;
        SignalBus.Instance.OnPlayerIgnoreGravityInvert += IgnoreGravityInvert;
        SignalBus.Instance.OnPlayerRecogniseGravityInvert += RecogniseGravityInvert;
        //debugLabel = GetTree().GetFirstNodeInGroup("DebugPrint") as Label;
        camera = GetTree().GetFirstNodeInGroup("Camera") as CameraController;
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;
       // debugLabel.Text = "Floor: " + IsOnFloor() + ", Ceiling:" + IsOnCeiling();

        // Add the gravity.
        if (!OnFloor())
        {
            velocity += playerGravityModifier * GetGravity() * (float)delta;
        }
        else
        {
            hasDoubleJumped = false;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("up"))
        {
            if (OnFloor())
            {
                velocity.Y =  playerGravityModifier * JumpVelocity;
            }
            else if (canDoubleJump && !hasDoubleJumped)
            {
                hasDoubleJumped = true;
                velocity.Y = playerGravityModifier * JumpVelocity;
            }
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero)
        {
            velocity.X = direction.X * Speed;
            velocity.Z = direction.Z * Speed;
        }
        else
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, Speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, Speed);
        }

        Velocity = velocity;
        MoveAndSlide();
    }

    private void InvertGravity()
    {
        gravityModifier *= -1;
        if (!ignoreGravityInvert)
        {
            playerGravityModifier = gravityModifier;
            RotateObjectLocal(Vector3.Right, Mathf.Pi);
            camera.FlipCamera();
        }
            
        GD.Print(playerGravityModifier);
    }

    private bool OnFloor()
    {
        if (playerGravityModifier == 1)
        {
            return IsOnFloor();
        }
        else
        {
            return IsOnCeiling();
        }
    }

    private void IgnoreGravityInvert()
    {
        ignoreGravityInvert = true;
    }

    private void RecogniseGravityInvert()
    {
        ignoreGravityInvert = false;
        if (playerGravityModifier != gravityModifier)
        {
            RotateObjectLocal(Vector3.Right, Mathf.Pi);
            camera.FlipCamera();
        }
        playerGravityModifier = gravityModifier;
    }
}
