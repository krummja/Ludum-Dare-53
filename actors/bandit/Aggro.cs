using Godot;


public partial class Aggro : Area2D
{
    public Bandit Bandit;

    public override void _Ready()
    {
        Bandit = GetParent<Bandit>();
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public void OnBodyEntered(Node2D body)
    {
        if (!Bandit.State.IsAggro) {
            Actor target = body as Actor;
            Bandit.SetAggro(target);
        }
    }

    public void OnBodyExited(Node2D body)
    {
        if (Bandit.State.IsAggro) {
            Actor target = body as Actor;
            Bandit.SetDeaggro(target);
        }
    }
}
