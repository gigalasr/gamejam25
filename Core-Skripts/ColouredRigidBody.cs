using Godot;
using System;

public partial class ColouredRigidBody : GravityInverter
{

    [ExportCategory("Dimensions")]
    [Export]
    public DimensionHud hud;

    [Export]
    public EDimension dimension;

    private readonly float bigMass = 5000;

    private float initialMass;

    public override void _Ready()
    {
        base._Ready();

        initialMass = Mass;
        SignalBus.Instance.OnDimensionShift += DimensionShift;
        DimensionShift();


        StandardMaterial3D material = new();
        switch (dimension)
        {
            case EDimension.BLUE:
                material.AlbedoColor = Colors.NavyBlue;
                break;
            case EDimension.YELLOW:
                material.AlbedoColor = Colors.Goldenrod;
                break;
            case EDimension.RED:
                material.AlbedoColor = Colors.DarkRed;
                break;
            case EDimension.GREEN:
                material.AlbedoColor = Colors.SeaGreen;
                break;
        }

        foreach (Node child in GetChildren())
        {
            MeshInstance3D mesh = child as MeshInstance3D;
            if (mesh != null)
            {
                mesh.MaterialOverlay = material;
            }
        }
    }

    private void DimensionShift()
    {
        if (hud.CurrentDimension == dimension)
        {
            Mass = initialMass;
        }
        else
        {
            Mass = bigMass;
        }
    }



}
