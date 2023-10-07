using Godot;

public partial class UI : CanvasLayer
{
	Label minesCountLabel;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		minesCountLabel = GetNode<Label>("MinesCountLabel");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
