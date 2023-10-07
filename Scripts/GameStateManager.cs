using Godot;

public partial class GameStateManager : Node
{
	MinesGrid minesGrid;
	Timer timer = new Timer();
	UI ui;

	public event MinesGrid.GameLostEventHandler gameLost;
	public event MinesGrid.GameWonEventHandler gameWon;
	public event MinesGrid.FlagChangeEventHandler flagChange;

	private int timeElapsed = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		gameLost += OnGameLost;
		gameWon += OnGameWon;
		flagChange += OnFlagChange;
		ui.SetMineCount(minesGrid.numberOfMines);
	}

	private void OnFlagChange(int flagsCount)
	{
		ui.SetMineCount(minesGrid.numberOfMines = flagsCount);
	}

	private void OnTimerTimeout()
	{
		timeElapsed++;
	}
	public void OnGameLost()
	{
		timer.Stop();
	}

	private void OnGameWon()
	{
		timer.Stop();
	}
}


