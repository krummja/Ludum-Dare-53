using Godot;
using System;

public partial class Player : Actor
{
    public Camera2D Camera;

    public AudioStreamPlayer2D Jump;

    public override void _Ready()
    {
        Camera = GetNode<Camera2D>("Camera");
        Jump = GetNode<AudioStreamPlayer2D>("Jump");

        Camera.CustomViewport = GetParent().GetParent();
        Camera.MakeCurrent();

        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
            Jump.Play();
        }

        Vector2 direction = GetDirection();

        bool isJumpInterrupted = Input.IsActionJustReleased("jump") && Velocity.Y < 0.0;
        Velocity = CalculateMoveVelocity(Velocity, direction, Speed, isJumpInterrupted);

        Velocity += new Vector2(0, Gravity * (float) delta);

        MoveAndSlide();
    }

    public Vector2 GetDirection()
    {
        return new Vector2(
            Input.GetActionStrength("move_right") - Input.GetActionStrength("move_left"),
            IsOnFloor() && Input.IsActionJustPressed("jump") ? -1 : 0
        );
    }

    public Vector2 CalculateMoveVelocity(
        Vector2 linearVelocity,
        Vector2 direction,
        Vector2 speed,
        bool isJumpInterrupted
    ) {
        Vector2 _velocity = linearVelocity;
        _velocity.X = speed.X * direction.X;

        if (direction.Y != 0.0) _velocity.Y = speed.Y * direction.Y;
        if (isJumpInterrupted) _velocity.Y *= 0.6f;

        return _velocity;
    }
}
