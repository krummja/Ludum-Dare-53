using Godot;
using System;

public partial class Main : Node
{
	[Export]
	public Menu Menu;

    public override void _UnhandledInput(InputEvent @event)
    {
		if (@event.IsActionPressed("toggle_pause")) {
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
}
