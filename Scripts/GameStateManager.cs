using Godot;

public partial class GameStateManager : Node
{
	MinesGrid minesGrid;
	Timer timer = new Timer();
	UI ui;

	public event MinesGrid.GameLostEventHandler gameLost;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameLost += OnGameLost;
		ui.SetMineCount(minesGrid.numberOfMines);
	}

	private void OnFlagChange(int flagsCount)
	{
		ui.SetMineCount(minesGrid.numberOfMines = flagsCount);
	}

	private void OnTimerTimeout()
	{
		// Replace with function body.
	}
	public void OnGameLost()
	{
		timer.Stop();
	}
}


