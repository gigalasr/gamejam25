using Godot;
using System;

public partial class PortalRender : Node3D
{
    [Export]
    public Node3D portal1;
    [Export]
    public Node3D portal2;

    private Node3D player;
    private Node3D cameraPortal;

    private SubViewport portalView;
    private ViewportTexture viewportTexture;
    private MeshInstance3D portalPlane;
    private StandardMaterial3D portalMaterial = new StandardMaterial3D();

    public override void _Ready()
    {
        player = (GetTree().GetFirstNodeInGroup("Player") as CharacterBody3D).GetNode("Body/Neck") as Node3D;
        cameraPortal = GetTree().GetFirstNodeInGroup("PortalCamera") as Node3D;
        portalView = GetNode<SubViewport>("PortalViewport");
        portalPlane = portal1.GetChild(0) as MeshInstance3D;
        viewportTexture = new ViewportTexture
        {
            ViewportPath = portalView.GetPath()
        };
        portalMaterial.AlbedoColor = new Color(3, 0, 3);

        // set correct scaling for viewport
        float scale = portalPlane.Scale.X / portalPlane.Scale.Y;
        Vector2 resized = portalView.Size * new Vector2(scale, 1);
        portalView.Size = (Vector2I)resized;

        
    }

    public override void _PhysicsProcess(double delta)
    {
        //compute position
        Vector3 distance = portal1.GlobalPosition - player.GlobalPosition;
        //cameraPortal.GlobalPosition = portal2.GlobalPosition - distance;
        // compensate for rotation
        Vector3 rotationDelta = portal1.GlobalRotation - portal2.GlobalRotation;
        distance = distance.Rotated(Vector3.Right, -rotationDelta.X);
        distance = distance.Rotated(Vector3.Up, -rotationDelta.Y);
        distance = distance.Rotated(Vector3.Forward, -rotationDelta.Z);

        //compute rotation
        cameraPortal.LookAtFromPosition(portal2.GlobalPosition - distance, portal2.GlobalPosition, Vector3.Up);

        var img = portalView.GetTexture().GetImage();
        portalMaterial.AlbedoTexture = ImageTexture.CreateFromImage(img);
        portalPlane.SetSurfaceOverrideMaterial(0, portalMaterial);
    }
}