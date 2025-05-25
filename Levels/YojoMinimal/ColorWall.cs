using Godot;
using System;
using System.ComponentModel;

public partial class ColorWall : MeshInstance3D
{

	[ExportCategory("Dimensions")]
	[Export]
	public DimensionHud hud;
	[Export]
	public EDimension dimension;

	private CollisionShape3D shape;
	public override void _Ready()
	{
		shape = GetNode<CollisionShape3D>("StaticBody3D/CollisionShape3D");

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

		MaterialOverride = material;
	}

	private void DimensionShift()
	{
		if (hud.CurrentDimension == dimension)
		{
			shape.Disabled = true;
		}
		else
		{
			shape.Disabled = false;
		}
	}

}
