using System.Data;
using Godot;

public partial class GameStateManager : Node
{
	public event MinesGrid.GameLostEventHandler gameLost;
	Timer timer = new Timer();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameLost += OnGameLost;
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}

	public void OnGameLost()
	{
		timer.Stop();
	}
}
