using Godot;
using System;

public partial class Portal : Node3D
{
    [ExportCategory("Portal 1 Settings")]
    [Export]
    public MeshInstance3D portal1;
    [Export]
    public bool canTeleport = false;
    [Export]
    public bool staticImage = true;
    [ExportCategory("Portal 2 Settings")]
    [Export]
    public MeshInstance3D portal2;
    [ExportCategory("General Settings")]
    [Export]
    public Node3D teleportLocation;
    [Export]
    public Node3D portalCamera;
    [Export]
    public PortalCamera camera;




    private MeshInstance3D activePortal;
    private MeshInstance3D otherPortal;

    private Node3D player;
    private StandardMaterial3D material = new StandardMaterial3D();

    public override void _Ready()
    {
        // genrate colliders for portals
        // portal 1
        Mesh mesh = portal1.Mesh;
        var shape = mesh.CreateConvexShape();
        var collisionShape = new CollisionShape3D
        {
            Shape = shape
        };
        portal1.GetChild(0).AddChild(collisionShape);
        var body = portal1.GetNode<StaticBody3D>("StaticBody3D");
        if (canTeleport)
        {
            body.CollisionLayer = 0;
            body.CollisionMask = 0;
        }

        // portal 1 Area3D
        var area = new Area3D();
        collisionShape = new CollisionShape3D
        {
            Shape = shape
        };
        area.AddChild(collisionShape);
        area.BodyEntered += OnBodyEnter;
        portal1.AddChild(area);


        player = GetTree().GetFirstNodeInGroup("Player") as Node3D; ;

        activePortal = portal1;
        otherPortal = portal2;

        //init camera
        var scale = new Vector2(activePortal.Scale.X, activePortal.Scale.Y);
        camera.ChangeScale(scale);

        otherPortal.Hide();
        material.AlbedoColor = new Color(3, 3, 3);
        material.AlbedoTexture = camera.GetViewPortTexture();
        activePortal.SetSurfaceOverrideMaterial(0, material);
        
    }

    private void OnBodyEnter(Node3D body)
    {
        if (!canTeleport) return;
        if (body is CharacterBody3D)body.GlobalPosition = teleportLocation.GlobalPosition;
        
    }

    public override void _PhysicsProcess(double delta)
    {
        // calculate position
        Vector3 distance = activePortal.GlobalPosition - player.GlobalPosition;

        // compensate for rotation
        Vector3 rotationDelta = activePortal.GlobalRotation - otherPortal.GlobalRotation;
        distance = distance.Rotated(Vector3.Right, -rotationDelta.X);
        distance = distance.Rotated(Vector3.Up, -rotationDelta.Y);
        distance = distance.Rotated(Vector3.Forward, rotationDelta.Z);

        distance = staticImage ? distance.Normalized() : distance;

        // set camera position and rotation
        portalCamera.LookAtFromPosition(otherPortal.GlobalPosition - distance, otherPortal.GlobalPosition, Vector3.Up);

        // draw to portal
        var texture = camera.GetViewPortTexture();
        material.AlbedoTexture = texture;
    }
}