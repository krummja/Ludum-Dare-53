using Godot;
using System;

public partial class World : Node2D
{
	public const int LIMIT_LEFT = -315;
	public const int LIMIT_TOP = -250;
	public const int LIMIT_RIGHT = 955;
	public const int LIMIT_BOTTOM = 690;

	public CharacterBody2D Player;
	public Camera2D Camera;

	public override void _Ready()
	{
		Player = GetNode<CharacterBody2D>("Player");
		Camera = Player.GetNode<Camera2D>("Camera");

		// Camera.LimitLeft = LIMIT_LEFT;
		// Camera.LimitTop = LIMIT_TOP;
		// Camera.LimitRight = LIMIT_RIGHT;
		// Camera.LimitBottom = LIMIT_BOTTOM;
	}

	public override void _Process(double delta)
	{
	}
}
