using Godot;
using System;

public partial class Analytics : CanvasLayer
{
	[ExportCategory("General")]
	[Export]
	public bool showFPS = true;
	[Export]
	public bool showFrameTime = true;
	[Export]
	public bool maxFPS = true;
	[ExportCategory("GPU Advanced")]
	[Export]
	public bool videoMemoryUsed = false;
	[Export]
	public bool drawCalls = false;
	[ExportCategory("CPU Advanced")]

	private Label label;
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		label = GetNode<Label>("Label");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		label.Text = "";
		if (showFPS) label.Text += Engine.GetFramesPerSecond() + " FPS\n";
		if (showFrameTime) label.Text += Performance.GetMonitor(Performance.Monitor.TimeProcess) + " ms\n";
		if (maxFPS) label.Text += (1000 / Performance.GetMonitor(Performance.Monitor.TimeProcess)).ToString("#,#") + " FPS\n";
	}
}
