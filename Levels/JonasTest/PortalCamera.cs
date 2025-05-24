using Godot;
using System;

public partial class PortalCamera : Node3D
{
    private SubViewport viewport;
    private Camera3D camera;
    public override void _Ready()
    {
        viewport = GetChild(0) as SubViewport;
        camera = GetNode<Camera3D>("SubViewport/Camera3D");
    }

    public override void _PhysicsProcess(double delta)
    {
        camera.GlobalTransform = GlobalTransform;
    }

    public ViewportTexture GetViewPortTexture()
    {
        return viewport.GetTexture();
    }


    public void ChangeScale(Vector2 size)
    {
        float scale = size.X / size.Y;
        GD.Print(scale);
        Vector2 newSize = (Vector2)viewport.Size;
        newSize.X *= scale;
        viewport.Size = (Vector2I)newSize;
        GD.Print(viewport.Size);
    }
}