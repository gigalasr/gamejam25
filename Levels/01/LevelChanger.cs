using Godot;
using System;

public partial class LevelChanger : Area3D
{
    [Export]
    public string newSceneName;

    public override void _Ready()
    {
        this.BodyEntered += OnBodyEntered;
    }

    public void OnBodyEntered(Node node)
    {
        GetTree().ChangeSceneToFile(newSceneName);
    }


}
