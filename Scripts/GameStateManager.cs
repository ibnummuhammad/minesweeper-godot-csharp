using Godot;

public partial class GameStateManager : Node
{
	MinesGrid minesGrid;
	UI ui;

	public event MinesGrid.GameLostEventHandler gameLost;
	Timer timer = new Timer();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameLost += OnGameLost;
		ui.SetMineCount(minesGrid.numberOfMines);
	}

	private void OnFlagChange(int flagsCount)
	{

	}

	public void OnGameLost()
	{
		timer.Stop();
	}
}
