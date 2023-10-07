using Godot;

public partial class GameStateManager : Node
{
	private MinesGrid minesGrid;
	private Timer timer = new();
	private UI ui;

	public event MinesGrid.GameLostEventHandler GameLost;
	public event MinesGrid.GameWonEventHandler GameWon;
	public event MinesGrid.FlagChangeEventHandler FlagChange;

	private int timeElapsed = 0;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		GameLost += OnGameLost;
		GameWon += OnGameWon;
		FlagChange += OnFlagChange;
		ui.SetMineCount(minesGrid.numberOfMines);
	}

	private void OnFlagChange(int flagsCount)
	{
		ui.SetMineCount(minesGrid.numberOfMines = flagsCount);
	}

	private void OnTimerTimeout()
	{
		timeElapsed++;
		ui.SetTimerCount(timeElapsed);
	}
	public void OnGameLost()
	{
		timer.Stop();
		ui.GameLost();
	}

	private void OnGameWon()
	{
		timer.Stop();
		ui.GameWon();
	}
}


