using Godot;

public partial class UI : CanvasLayer
{
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GD.Print("yang ini");
		GD.Print(GetNode("MinesCountLabel"));
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
