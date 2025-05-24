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

	public override void _PhysicsProcess(double delta)
    {
        Vector3 velocity = Velocity;

        // Add the gravity.
        if (!IsOnFloor())
        {
            velocity += GetGravity() * (float)delta;
        }
        else
        {
            hasDoubleJumped = false;
        }

        // Handle Jump.
        if (Input.IsActionJustPressed("up"))
        {
            if (IsOnFloor())
            {
                velocity.Y = JumpVelocity;
            }
            else if (canDoubleJump && !hasDoubleJumped)
            {
                hasDoubleJumped = true;
                velocity.Y = JumpVelocity;
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
}
