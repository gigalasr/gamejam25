using Godot;
using System;

public partial class DimensionHud : Control
{

	//0: deaktivated
	//1: activated
	//-1: anti gravity
	[ExportCategory("Dimensions")]

	[Export]
	public int maxShifts;

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
	private Label hint;
	private Label leftShifts;

	public override void _Ready()
	{
		dimTexture = GetNode<TextureRect>("Dim");
		hint = GetNode<Label>("VBoxContainer/Hint");
		leftShifts = GetNode<Label>("VBoxContainer/ShiftsLeft");
		leftShifts.Text = maxShifts.ToString();
		SetStartDimension();
		SetDimIcon();
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

		if(greenDimension == 0){
			greenDimension = 1;
		}
	}

	public override void _Input(InputEvent @event)
	{
		if (@event.IsActionPressed("DimensionShift") && maxShifts > 0){
			leftShifts.Text = (--maxShifts).ToString();
			ShiftDimension(CurrentDimension);
		}
	}

	public void SetHint(string hint){
		this.hint.Text = hint;
	}


	private void ShiftDimension(EDimension from){
		CurrentDimension = CurrentDimension.getNext();

		if(GetConfig(CurrentDimension) == 0){
			ShiftDimension(from);
			return;
		}

		if(GetConfig(CurrentDimension) < 0 != GetConfig(from) < 0){
			SignalBus.Instance.InvertGravity();
		}
		SetDimIcon();
	}

	private int GetConfig(EDimension dim)
	{
		switch(dim){
			case EDimension.BLUE: return blueDimension;
			case EDimension.YELLOW: return yellowDimension;
			case EDimension.RED: return redDimension;
			case EDimension.GREEN: return greenDimension;
		}
		return 0;
	}

	private void SetDimIcon(){

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
