using Godot;


public partial class PitDeath : Area2D
{
    public CollisionShape2D Collider;

    public override void _Ready()
    {
        Collider = GetChild<CollisionShape2D>(0);
        BodyEntered += OnBodyEntered;
    }

    public void OnBodyEntered(Node2D body)
    {
        Actor actor = body as Actor;
        if (!actor.IsPlayer) return;

        actor.DealDamage(1000);
    }
}
