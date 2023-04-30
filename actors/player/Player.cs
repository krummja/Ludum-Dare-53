using Godot;
using System;


public struct PlayerRefs
{
    public Sprite2D Sprite;
    public Camera2D Camera;
    public AnimationPlayer Animation;
    public AudioStreamPlayer2D Jump;
    public Timer TootTimer;
}


public struct PlayerState
{
    public bool IsRunning;
    public bool IsJumping;
    public bool IsTooting;
}


public partial class Player : Actor
{
    public Vector2 Direction;
    public Horn Horn;

    private PlayerRefs refs;
    private PlayerState state;

    private float lastDirection;

    public override void _Ready()
    {
        // Set up references.
        refs = new PlayerRefs();
        refs.Sprite = GetNode<Sprite2D>("Sprite");
        refs.Camera = GetNode<Camera2D>("Camera");
        refs.Animation = GetNode<AnimationPlayer>("Animation");
        refs.Jump = GetNode<AudioStreamPlayer2D>("Jump");
        refs.TootTimer = GetNode<Timer>("TootAnimation");

        // Configure the camera.
        refs.Camera.CustomViewport = GetParent().GetParent();
        refs.Camera.MakeCurrent();

        // Get reference to the horn object.
        Horn = GetNode<Horn>("Horn");

        // Initialize direction.
        Direction = Vector2.Zero;

        // Initialize the player state.
        state = new PlayerState {
            IsRunning = false,
            IsJumping = false,
            IsTooting = false,
        };

        // Call the Actor base to set Gravity.
        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        string animation = "";

        Direction = GetDirection();

        if (Input.IsActionJustPressed("jump") && IsOnFloor()) {
            refs.Jump.Play();
        }

        bool isJumpInterrupted = Input.IsActionJustReleased("jump") && Velocity.Y < 0.0;
        Velocity = CalculateMoveVelocity(Velocity, Direction, Speed, isJumpInterrupted);
        Velocity += new Vector2(0, Gravity * (float) delta);

        MoveAndSlide();

        // Flip the sprite based on input direction.
        float flipDirection;
        if (Direction.X != 0f) {
            if (Direction.X > 0f) {
                refs.Sprite.FlipH = false;
            } else if (Direction.X < 0f) {
                refs.Sprite.FlipH = true;
            }

            flipDirection = Direction.X;
            lastDirection = Direction.X;
        } else {
            flipDirection = lastDirection;
        }

        state.IsTooting = false;
        if (Input.IsActionJustPressed("attack")) {
            state.IsTooting = Horn.Toot(flipDirection);
        }

        // We set the animation above - if we get to this point and the animation
        // is not the currently loaded one, we need to set the new animation for
        // the next frame.

        animation = GetNewAnimation(state.IsTooting);
        if (animation != refs.Animation.CurrentAnimation && refs.TootTimer.IsStopped()) {
            if (state.IsTooting) {
                refs.TootTimer.Start();
            }
            refs.Animation.Play(animation);
        }
    }

    public Vector2 GetDirection()
    {
        float xInput = (
            Input.GetActionStrength("move_right") -
            Input.GetActionStrength("move_left")
        );

        float yInput = Input.IsActionJustPressed("jump") && IsOnFloor() ? -1 : 0;

        return new Vector2(xInput, yInput);
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

    public string GetNewAnimation(bool isTooting = false)
    {
        string animation_new = "";

        if (IsOnFloor()) {
            if (Mathf.Abs(Velocity.X) > 0.1f) {
                animation_new = "run";
            } else {
                animation_new = "idle";
            }
        } else {
            if (Velocity.Y > 0f) {
                animation_new = "fall";
            } else {
                animation_new = "jump";
            }
        }

        if (isTooting) {
            animation_new += "_toot";
        }

        return animation_new;
    }
}
