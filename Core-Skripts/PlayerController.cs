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
    [ExportCategory("Interaction")]
    [Export]
    public float pushForce = 50.0f;
    [Export]
    public float animTime = 1.0f;
    [ExportCategory("Actions")]
    [Export]
    public bool canJump = true;
    [Export]
    public bool canDoubleJump = false;
    [Export]
    public bool airControl = true;

    public float currentSpeed;


    private bool hasDoubleJumped = false;

    private float gravityModifier = 1;
    public float playerGravityModifier = 1;
    private bool ignoreGravityInvert = false;

    private Label debugLabel;
    private CameraController camera;

    private Node3D neck;

    public override void _ExitTree()
    {
        SignalBus.Instance.OnGravityInvert -= InvertGravity;
        SignalBus.Instance.OnPlayerIgnoreGravityInvert -= IgnoreGravityInvert;
        SignalBus.Instance.OnPlayerRecogniseGravityInvert -= RecogniseGravityInvert;
    }

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
        //debugLabel.Text = "Floor: " + IsOnFloor() + ", Ceiling:" + IsOnCeiling();

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
        if (Input.IsActionJustPressed("up") && canJump)
        {
            if (OnFloor())
            {
                velocity.Y = playerGravityModifier * JumpVelocity;
            }
            else if (canDoubleJump && !hasDoubleJumped)
            {
                hasDoubleJumped = true;
                velocity.Y = playerGravityModifier * JumpVelocity;
            }
        }

        // Get the input direction and handle the movement/deceleration.
        // As good practice, you should replace UI actions with custom gameplay actions.
        var speed = Input.IsActionPressed("sprint") ? runningSpeed : Speed;
        Vector2 inputDir = Input.GetVector("left", "right", "forward", "backward");
        Vector3 direction = (Transform.Basis * new Vector3(inputDir.X, 0, inputDir.Y)).Normalized();
        if (direction != Vector3.Zero && (airControl || OnFloor()))
        {
            velocity.X = direction.X * speed;
            velocity.Z = direction.Z * speed;
        }
        else if(OnFloor())
        {
            velocity.X = Mathf.MoveToward(Velocity.X, 0, speed);
            velocity.Z = Mathf.MoveToward(Velocity.Z, 0, speed);
        }

        Velocity = velocity;
        currentSpeed = velocity.Dot(velocity);
        MoveAndSlide();

        /// handle push collisions
        for (int i = 0; i < GetSlideCollisionCount(); ++i)
        {
            var collision = GetSlideCollision(i);
            if (collision.GetCollider() is RigidBody3D)
            {
                var collider = (RigidBody3D)collision.GetCollider();
                collider.ApplyCentralImpulse(-collision.GetNormal() * pushForce);
            }
        }
    }

    private void InvertGravity()
    {
        gravityModifier *= -1;
        if (!ignoreGravityInvert)
        {
            playerGravityModifier = gravityModifier;
            RotateObjectLocal(Vector3.Right, Mathf.Pi);
            RotateObjectLocal(Vector3.Up, Mathf.Pi);
            camera.FlipCamera();
        }
            
        GD.Print(playerGravityModifier);
    }

    public bool OnFloor()
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
            // animte with tween
            var tween = GetTree().CreateTween().BindNode(this).SetTrans(Tween.TransitionType.Elastic);
            var rotation = Rotation + new Vector3(Mathf.Pi, Mathf.Pi, 0);
            tween.TweenProperty(this, "rotation", rotation, animTime);
            tween.Play();

            camera.FlipCamera();
        }
        playerGravityModifier = gravityModifier;
    }
}
