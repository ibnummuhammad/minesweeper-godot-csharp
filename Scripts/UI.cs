using Godot;

public partial class UI : CanvasLayer
{
	Label minesCountLabel;
	Label timerCountLabel;
	Resource gameLostButtonTexture;
	Resource gameWonButtonTexture;
	TextureButton gameStatusButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		minesCountLabel = GetNode<Label>("MinesCountLabel");
		timerCountLabel = GetNode<Label>("TimerCountLabel");
		gameStatusButton = GetNode<TextureButton>("GameStatusButton");

		gameLostButtonTexture = ResourceLoader.Load("res://Assets/button_dead.png");
		gameWonButtonTexture = ResourceLoader.Load("res://Assets/button_cleared.png");
	}

	public void SetMineCount(int minesCount)
	{
		string minesCountString = minesCount.ToString();
		if (minesCountString.Length < 3)
			minesCountString.PadLeft(3, '0');

		minesCountLabel.Text = minesCountString;
	}

	public void SetTimerCount(int timerCount)
	{
		string timerString = timerCount.ToString();
		if (timerString.Length < 3)
			timerString = timerString.PadLeft(3, '0');

		timerCountLabel.Text = timerString;
	}

	private void OnGameStatusButtonPressed()
	{
		GetTree().ReloadCurrentScene();
	}

	private void GameLost()
	{
		gameStatusButton.TextureNormal = (Texture2D)gameLostButtonTexture;
	}

	private void GameWon()
	{
		gameStatusButton.TextureNormal = (Texture2D)gameWonButtonTexture;
	}
}
