using Godot;
using System;

public partial class Actor : CharacterBody2D
{
    [ExportGroup("Movement")]
    [Export]
    public Vector2 Speed = new Vector2(150f, 350f);

    private float gravity;
    public float Gravity
    {
        get => (float) this.gravity;
    }

    public Vector2 FloorNormal = Vector2.Up;

    public override void _Ready()
    {
        gravity = (float) ProjectSettings.GetSetting("physics/2d/default_gravity");
    }
}
