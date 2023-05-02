using Godot;

public partial class Scroll : Area2D
{
    public AnimationPlayer Animation;
    public Timer Timer;

    public override void _Ready()
    {
        Animation = GetNode<AnimationPlayer>("Animation");
        Timer = GetNode<Timer>("Timer");
        BodyEntered += OnBodyEntered;
        Animation.Play("hover");
    }

    public override void _Process(double delta)
    {
        if (Timer.IsStopped()) {
            Timer.Start();
            Animation.Play("hover");
        }
    }

    public void OnBodyEntered(Node2D body)
    {
        Actor actor = body as Actor;
        if (actor.IsPlayer) {
            actor.GainScrollPiece();
        }
        QueueFree();
    }
}
