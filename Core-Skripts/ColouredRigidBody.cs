using Godot;
using System;
using System.Collections.Generic;

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

        Stack<Node> to_visit = new Stack<Node>();
        to_visit.Push(this);

        while (to_visit.Count != 0)
        {
            Node child = to_visit.Pop();
            foreach (Node c in child.GetChildren())
            {
                to_visit.Push(c);
            }

            MeshInstance3D mesh = child as MeshInstance3D;
            if (mesh != null)
            {
                GD.Print(mesh.Name);
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
