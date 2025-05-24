using Godot;
using System;

public partial class DimensionHud : Control
{

	//0: deaktivated
	//1: activated
	//-1: anti gravity
	[ExportCategory("Dimensions")]
	[Export]
	public int blueDimension;


	[Export]
	public int yellowDimension;


	[Export]
	public int redDimension;

	[Export]
	public int greenDimension;


	public EDimension CurrentDimension{get; private set;}

	private TextureRect dimTexture;

	public override void _Ready()
	{
		SetStartDimension();
		dimTexture = GetNode<TextureRect>("Dim");

		switch(CurrentDimension){
			case EDimension.BLUE:
				if(blueDimension > 0){
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://Assets/DimensionIcons/blueDot.png");
				} else if(blueDimension < 0){
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://Assets/DimensionIcons/blueArrow.png");
				} else {
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://icon.svg");
				}
			break;
			case EDimension.YELLOW:
				if(blueDimension > 0){
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://Assets/DimensionIcons/yellowDot.png");
				} else if(blueDimension < 0){
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://Assets/DimensionIcons/yellowArrow.png");
				} else {
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://icon.svg");
				}
			break;
			case EDimension.RED:
				if(blueDimension > 0){
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://Assets/DimensionIcons/redDot.png");
				} else if(blueDimension < 0){
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://Assets/DimensionIcons/redArrow.png");
				} else {
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://icon.svg");
				}
			break;
			case EDimension.GREEN:
				if(blueDimension > 0){
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://Assets/DimensionIcons/greenDot.png");
				} else if(blueDimension < 0){
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://Assets/DimensionIcons/greenArrow.png");
				} else {
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://icon.svg");
				}
			break;
			default:
				dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://icon.svg");
			break;
		}

	}

	private void SetStartDimension(){
		if(blueDimension != 0){
			CurrentDimension = EDimension.BLUE;
			return;
		}

		if(yellowDimension != 0){
			CurrentDimension = EDimension.YELLOW;
			return;
		}

		if(redDimension != 0){
			CurrentDimension = EDimension.RED;
			return;
		}

		CurrentDimension = EDimension.GREEN;
	}

	

}
