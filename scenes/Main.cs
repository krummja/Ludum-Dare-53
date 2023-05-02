using Godot;
using System;


public enum GameState
{
	Start,
	StoryStart,
	Game,
	Win,
	Lose,
}

public partial class Main : Node
{
	[Export]
	public Menu Menu;

	[Export]
	public GameOver GameOverScreen;

	public GameState GameState = GameState.Start;

	public bool IsGameOver = false;

    public override void _UnhandledInput(InputEvent @event)
    {
		if (@event.IsActionPressed("toggle_pause") && GameState != GameState.Start) {
			SceneTree tree = GetTree();
			tree.Paused = !tree.Paused;

			if (tree.Paused) {
				Menu.Open();
			} else {
				Menu.Close();
			}

			tree.Root.SetInputAsHandled();
		}
    }

	public void GameOver()
	{
		GameState = GameState.Lose;
		IsGameOver = true;
		GameOverScreen.Open();
		SceneTree tree = GetTree();
		tree.Paused = true;
	}

	public void GameWon()
	{
		GameState = GameState.Win;
		// SceneTree tree = GetTree();
		// tree.Paused = true;
	}
}
