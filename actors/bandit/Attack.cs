using Godot;


public partial class Attack : Area2D
{
    public Bandit Bandit;
    public bool IsAttacking = false;

    public override void _Ready()
    {
        Bandit = GetParent<Bandit>();
        BodyEntered += OnBodyEntered;
        BodyExited += OnBodyExited;
    }

    public void OnBodyEntered(Node2D body)
    {
        if (!IsAttacking) {
            IsAttacking = true;
            Actor target = body as Actor;
            target.CanBeHit = true;
        }
    }

    public void OnBodyExited(Node2D body)
    {
        if (IsAttacking) {
            IsAttacking = false;
            Actor target = body as Actor;
            target.CanBeHit = false;
        }
    }
}
