using Godot;


public partial class CrowTrigger : Node
{
    public Area2D Crow;
    public Area2D Crow1;
    public Area2D Crow2;

    public Area2D Trigger;

    public Timer CrowTimer;

    public override void _Ready()
    {
        Crow = GetNode<Area2D>("Crow");
        Crow1 = GetNode<Area2D>("Crow1");
        Crow2 = GetNode<Area2D>("Crow2");
        Trigger = GetNode<Area2D>("Trigger");

        CrowTimer = GetNode<Timer>("CrowTimer");

        Trigger.BodyEntered += OnTriggerEntered;
        CrowTimer.Timeout += OnTimerTimeout;
    }

    public void OnTriggerEntered(Node2D body)
    {
        Actor actor = body as Actor;

        if (!actor.IsPlayer) return;

        AnimationPlayer crow = Crow.GetNode<AnimationPlayer>("Animation");
        AnimationPlayer crow1 = Crow1.GetNode<AnimationPlayer>("Animation");
        AnimationPlayer crow2 = Crow2.GetNode<AnimationPlayer>("Animation");

        crow.Play("crow_fly");
        crow1.Play("crow_fly_2");
        crow2.Play("crow_fly_3");

        CrowTimer.Start();
    }

    public void OnTimerTimeout()
    {
        Crow.QueueFree();
        Crow1.QueueFree();
        Crow2.QueueFree();
    }
}
