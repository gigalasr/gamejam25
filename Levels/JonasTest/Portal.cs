using Godot;
using System;

public partial class Portal : Node3D
{
    [ExportCategory("Portal 1 Settings")]
    [Export]
    public MeshInstance3D portal1;
    [Export]
    public bool isActive1 = false;
    [ExportCategory("Portal 2 Settings")]
    [Export]
    public MeshInstance3D portal2;
    [Export]
    public bool isActive2 = false;
    [ExportCategory("General Settings")]
    [Export]
    public bool useAsMirror = false;
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

        // portal 2
        mesh = portal2.Mesh;
        shape = mesh.CreateConvexShape();
        collisionShape = new CollisionShape3D
        {
            Shape = shape
        };
        portal2.GetChild(0).AddChild(collisionShape);

        player = GetTree().GetFirstNodeInGroup("Player") as Node3D; ;


        if (isActive1)
        {
            activePortal = portal1;
            otherPortal = portal2;
        }
        else
        {
            activePortal = portal2;
            otherPortal = portal1;
        }

        //init camera
        var scale = new Vector2(activePortal.Scale.X, activePortal.Scale.Y);
        camera.ChangeScale(scale);

        otherPortal.Hide();
        material.AlbedoColor = new Color(3, 3, 3);
        activePortal.SetSurfaceOverrideMaterial(0, material);
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

        // set camera position and rotation
        portalCamera.LookAtFromPosition(otherPortal.GlobalPosition - distance, otherPortal.GlobalPosition, Vector3.Up);

        // draw to portal
        var texture = camera.GetViewPortTexture();
        material.AlbedoTexture = texture;
    }
}