using Godot;
using System;

public partial class EpicUiController : Control
{
    public void BtnStart()
    {
        GetTree().ChangeSceneToFile("res://Levels/01/01.tscn");
    }

    public void BtnL1()
    {
        GetTree().ChangeSceneToFile("res://Levels/01/01.tscn");
    }

    public void BtnL2()
    {
        GetTree().ChangeSceneToFile("res://Levels/01-and-half/01-and-half.tscn");
    }

    public void BtnL3()
    {
        GetTree().ChangeSceneToFile("res://Levels/Stage 02/Level1/Level1.tscn");
    }
}
