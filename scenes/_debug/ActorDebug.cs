using Godot;


public partial class ActorDebug : Control
{
    public Bandit Actor;

    public Label Aggro;
    public Label Attacking;
    public Label Cooldown;
    public Label HitDelay;

    public override void _Ready()
    {
        Actor = GetTree().Root.GetNode<Bandit>("Game/World/Enemies/Bandit");

        Aggro = GetNode<Label>("Aggro");
        Attacking = GetNode<Label>("Attacking");
        Cooldown = GetNode<Label>("Cooldown");
        HitDelay = GetNode<Label>("HitDelay");

        Aggro.Text = Actor.State.IsAggro ? "Aggro" : "";
        Attacking.Text = Actor.State.IsAttacking ? "Attacking" : "";
        Cooldown.Text = $"Cooldown: {0}";
        HitDelay.Text = $"HitDelay: {0}";
    }

    public override void _Process(double delta)
    {
        Aggro.Text = Actor.State.IsAggro ? "Aggro" : "";
        Attacking.Text = Actor.State.IsAttacking ? "Attacking" : "";
        Cooldown.Text = $"Cooldown: {Actor.Cooldown.TimeLeft}";
        HitDelay.Text = $"HitDelay: {Actor.HitDelay.TimeLeft}";
    }
}
