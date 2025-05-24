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
	public int TimesDimChanged{get; private set;}

	private TextureRect dimTexture;

	public override void _Ready()
	{
		TimesDimChanged = 0;
		SetStartDimension();
		setDimIcon();
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

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("DimensionShift")){
			shiftDimension();
			TimesDimChanged++;
		}
	}

	private void shiftDimension(){
		switch(CurrentDimension){
			case EDimension.BLUE:
				CurrentDimension = EDimension.YELLOW;
				if(yellowDimension == 0){
					shiftDimension();
					return;
				}
				if(yellowDimension < 0 != blueDimension < 0)
					SignalBus.Instance.InvertGravity();
			break;
			case EDimension.YELLOW:
				CurrentDimension = EDimension.RED;
				if(redDimension == 0){
					shiftDimension();
					return;
				}
				if(redDimension < 0 != yellowDimension < 0)
					SignalBus.Instance.InvertGravity();
			break;
			case EDimension.RED:
				CurrentDimension = EDimension.GREEN;
				if(greenDimension == 0){
					shiftDimension();
					return;
				}
				if(greenDimension < 0 != redDimension < 0)
					SignalBus.Instance.InvertGravity();
			break;
			case EDimension.GREEN:
				CurrentDimension = EDimension.BLUE;
				if(blueDimension == 0){
					shiftDimension();
					return;
				}
				if(blueDimension < 0 != greenDimension < 0)
					SignalBus.Instance.InvertGravity();
			break;
		}
		setDimIcon();
	}

	private void setDimIcon(){
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
				if(yellowDimension > 0){
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://Assets/DimensionIcons/yellowDot.png");
				} else if(yellowDimension < 0){
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://Assets/DimensionIcons/yellowArrow.png");
				} else {
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://icon.svg");
				}
			break;
			case EDimension.RED:
				if(redDimension > 0){
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://Assets/DimensionIcons/redDot.png");
				} else if(redDimension < 0){
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://Assets/DimensionIcons/redArrow.png");
				} else {
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://icon.svg");
				}
			break;
			case EDimension.GREEN:
				if(greenDimension > 0){
					dimTexture.Texture = ResourceLoader.Load<Texture2D>("res://Assets/DimensionIcons/greenDot.png");
				} else if(greenDimension < 0){
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

}
