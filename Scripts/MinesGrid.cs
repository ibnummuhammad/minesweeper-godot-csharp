using Godot;
using System.Collections.Generic;

public partial class MinesGrid : TileMap
{
	[Signal]
	public delegate void FlagChangeEventHandler(int numberOfFlags);

	[Signal]
	public delegate void GameLostEventHandler();

	[Signal]
	public delegate void GameWonEventHandler();

	private Dictionary<string, Godot.Vector2I> CELLS = new();

	[Export]
	private int columns = 8;

	[Export]
	private int rows = 8;

	[Export]
	public int numberOfMines = 8;

	private int TILE_SET_ID = 0;
	private int DEFAULT_LAYER = 0;
	int flagsPlaced = 0;
	private List<Vector2I> cellsWithMines = new() { };
	private List<Vector2I> cellsWithFlags = new() { };
	private List<Vector2I> cellsCheckedRecursively = new() { };
	bool isGameFinished = false;

	readonly RandomNumberGenerator random = new();

	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		CELLS.Add("1", new Godot.Vector2I(0, 0));
		CELLS.Add("2", new Godot.Vector2I(1, 0));
		CELLS.Add("3", new Godot.Vector2I(2, 0));
		CELLS.Add("4", new Godot.Vector2I(3, 0));
		CELLS.Add("5", new Godot.Vector2I(4, 0));
		CELLS.Add("6", new Godot.Vector2I(0, 1));
		CELLS.Add("7", new Godot.Vector2I(1, 1));
		CELLS.Add("8", new Godot.Vector2I(2, 1));
		CELLS.Add("CLEAR", new Godot.Vector2I(3, 1));
		CELLS.Add("MINE_RED", new Godot.Vector2I(4, 1));
		CELLS.Add("FLAG", new Godot.Vector2I(0, 2));
		CELLS.Add("MINE", new Godot.Vector2I(1, 2));
		CELLS.Add("DEFAULT", new Godot.Vector2I(2, 2));

		ClearLayer(DEFAULT_LAYER);

		for (int i = 0; i < rows; i++)
			for (int j = 0; j < columns; j++)
			{
				Godot.Vector2I cellCoord = new(i - rows / 2, j - columns / 2);
				SetTileCell(cellCoord, "DEFAULT");
			}

		PlaceMine();
	}

	public override void _Input(InputEvent @event)
	{
		if (isGameFinished)
			return;

		if (@event.GetType() == new InputEventMouseMotion().GetType())
		{
			return;
		}


		if (!(@event == new InputEventMouseButton() || @event.IsPressed()))
			return;

		Vector2I clickedCellCoord = LocalToMap(GetLocalMousePosition());

		if (@event is InputEventMouseButton eventMouseButton)
		{
			if (eventMouseButton.ButtonIndex.ToString() == "Left")
				OnCellClicked(clickedCellCoord);
			else if (eventMouseButton.ButtonIndex.ToString() == "Right")
				PlaceFlag(clickedCellCoord);
		}
	}

	private void PlaceMine()
	{
		for (int i = 0; i < numberOfMines; i++)
		{
			Vector2I cellCoordinates = new(random.RandiRange(-rows / 2, rows / 2 - 1), random.RandiRange(-columns / 2, columns / 2 - 1));

			while (cellsWithMines.Contains(cellCoordinates))
				cellCoordinates = new Vector2I(random.RandiRange(-rows / 2, rows / 2 - 1), random.RandiRange(-columns / 2, columns / 2 - 1));

			cellsWithMines.Add(cellCoordinates);
		}
	}

	private void SetTileCell(Godot.Vector2I cellCoord, string cell_type)
	{
		SetCell(DEFAULT_LAYER, cellCoord, TILE_SET_ID, CELLS[cell_type]);
	}

	private void OnCellClicked(Vector2I cellCoord)
	{
		foreach (Vector2I cell in cellsWithMines)
			if (cell.X == cellCoord.X && cell.Y == cellCoord.Y)
			{
				Lose(cellCoord);
				return;
			}

		cellsCheckedRecursively.Add(cellCoord);
		HandleCells(cellCoord, true);

		if (cellsWithFlags.Contains(cellCoord))
		{
			flagsPlaced--;
			// EmitSignal(nameof(FlagChangeEventHandler), flagsPlaced);
			cellsWithFlags.Remove(cellCoord);
		}
	}

	private void HandleCells(Vector2I cellCoord, bool shouldStopAfterMine = false)
	{
		TileData tileData = GetCellTileData(DEFAULT_LAYER, cellCoord);

		if (tileData == null)
			return;

		bool cellHasMine = (bool)tileData.GetCustomData("has_mine");

		if (cellHasMine && shouldStopAfterMine)
			return;

		int mineCount = GetSurroundingCellsMineCount(cellCoord);

		if (mineCount == 0 && !cellsWithFlags.Contains(cellCoord))
		{
			SetTileCell(cellCoord, "CLEAR");
			List<Vector2I> surroundingCells = GetSurroundingCellsToCheck(cellCoord);
			foreach (Vector2I cell in surroundingCells)
				HandleSurroundingCell(cell);
		}
		else
			SetTileCell(cellCoord, mineCount.ToString());

		if (cellsWithFlags.Contains(cellCoord))
		{
			flagsPlaced--;
			// EmitSignal(nameof(FlagChangeEventHandler), flagsPlaced);
			cellsWithFlags.Remove(cellCoord);
		}
	}

	private void HandleSurroundingCell(Vector2I cellCoord)
	{
		if (cellsCheckedRecursively.Contains(cellCoord) is true)
			return;

		cellsCheckedRecursively.Add(cellCoord);
		HandleCells(cellCoord);
	}

	private int GetSurroundingCellsMineCount(Vector2I cellCoord)
	{
		int mineCount = 0;
		List<Vector2I> surroundingCells = GetSurroundingCellsToCheck(cellCoord);

		foreach (Vector2I cell in surroundingCells)
		{
			TileData tileData = GetCellTileData(DEFAULT_LAYER, cell);
			if (tileData != null && cellsWithMines.Contains(cell))
				mineCount++;
		}

		return mineCount;
	}

	private void Lose(Vector2I cellCoord)
	{
		GD.Print("LOSE!");

		// EmitSignal("GameLostEventHandler");
		isGameFinished = true;

		foreach (Vector2I cell in cellsWithMines)
			SetTileCell(cell, "MINE");

		SetTileCell(cellCoord, "MINE_RED");
	}

	private void PlaceFlag(Vector2I cellCoord)
	{
		Vector2I atlastCoordinates = GetCellAtlasCoords(DEFAULT_LAYER, cellCoord);
		bool isEmptyCell = atlastCoordinates == new Vector2I(2, 2);
		bool isFlagCell = atlastCoordinates == new Vector2I(0, 2);

		if (!isEmptyCell && !isFlagCell)
			return;

		if (isFlagCell)
		{
			SetTileCell(cellCoord, "DEFAULT");
			cellsWithFlags.Remove(cellCoord);
			flagsPlaced--;
		}
		else if (isEmptyCell)
		{
			if (flagsPlaced == numberOfMines)
				return;

			flagsPlaced++;
			SetTileCell(cellCoord, "FLAG");
			cellsWithFlags.Add(cellCoord);
		}

		// EmitSignal(nameof(FlagChangeEventHandler), flagsPlaced);

		int count = 0;
		foreach (Vector2I flagCell in cellsWithFlags)
			foreach (Vector2I mineCell in cellsWithMines)
				if (flagCell.X == mineCell.X && flagCell.Y == mineCell.Y)
					count++;

		if (count == cellsWithMines.Count)
			Win();
	}

	private void Win()
	{
		GD.Print("WIN!");

		isGameFinished = true;
		// EmitSignal(nameof(GameWonEventHandler));
	}

	private List<Vector2I> GetSurroundingCellsToCheck(Vector2I currentCell)
	{
		Vector2I targetCell;
		List<Vector2I> surroundingCells = new() { };

		for (int y = 0; y < 3; y++)
			for (int x = 0; x < 3; x++)
			{
				if (x == 1 && y == 1)
					continue;
				targetCell = currentCell + new Vector2I(x - 1, y - 1);
				surroundingCells.Add(targetCell);
			}

		return surroundingCells;
	}
}
