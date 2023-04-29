using Godot;
using System;

public partial class Main : Control
{
	private TextureRect title;
	private AudioStreamPlayer bgMusic;
	private CanvasModulate canvasModulate;
	private AnimationPlayer screenFader;

	private int width;
	private int height;
	private bool initialized = false;

	public override void _Ready()
	{
		title = GetNode<TextureRect>("Title");
		bgMusic = GetNode<AudioStreamPlayer>("BGMusic");
		canvasModulate = GetNode<CanvasModulate>("CanvasModulate");
		screenFader = GetNode<AnimationPlayer>("ScreenFader");

		width = (int) ProjectSettings.GetSetting("display/window/size/viewport_width");
		height = (int) ProjectSettings.GetSetting("display/window/size/viewport_height");

		title.Texture = ResourceLoader.Load<Texture2D>("res://assets/textures/menu/title.svg");
		title.AnchorLeft = 0.5f;
		title.AnchorRight = 0.5f;
		title.AnchorTop = 0.5f;
		title.AnchorBottom = 0.5f;

		Vector2 textureSize = title.Texture.GetSize();
		title.OffsetLeft = -textureSize.X / 2;
		title.OffsetRight = textureSize.X / 2;
		title.OffsetTop = -textureSize.Y / 2;
		title.OffsetBottom = textureSize.Y / 2;

		initialized = true;
	}

	public override void _Process(double delta)
	{

	}

    public override void _Input(InputEvent @event)
    {
        if (@event.IsActionPressed("ui_accept")) {
			GD.Print(width);
			GD.Print(height);
		}
    }
}
