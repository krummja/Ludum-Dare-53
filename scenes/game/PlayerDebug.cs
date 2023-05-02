using Godot;

public partial class PlayerDebug : Control
{
    [Export]
    public Actor Player;

    public Label HealthMeter;
    public Label CanBeHit;

    public override void _Ready()
    {
        HealthMeter = GetNode<Label>("Health");
        CanBeHit = GetNode<Label>("CanBeHit");
        HealthMeter.Text = $"Health: {Player.Health}";
        CanBeHit.Text = $"CanBeHit: {Player.CanBeHit}";
    }

    public override void _Process(double delta)
    {
        HealthMeter.Text = $"Health: {Player.Health}";
        CanBeHit.Text = $"CanBeHit: {Player.CanBeHit}";
    }
}
