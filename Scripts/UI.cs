using Godot;

public partial class UI : CanvasLayer
{
	Label minesCountLabel;
	Label timerCountLabel;
	TextureButton gameStatusButton;

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		minesCountLabel = GetNode<Label>("MinesCountLabel");
		timerCountLabel = GetNode<Label>("TimerCountLabel");
		gameStatusButton = GetNode<TextureButton>("GameStatusButton");

		Resource gameLostButtonTexture = ResourceLoader.Load("res://Assets/button_dead.png");
		Resource gameWonButtonTexture = ResourceLoader.Load("res://Assets/button_cleared.png");
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

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
