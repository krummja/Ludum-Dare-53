using Godot;


public partial class Gate : StaticBody2D
{
    public CollisionShape2D Collider;
    public Node2D Open;
    public Node2D Closed;

    public Main Main;

    public override void _Ready()
    {
        Open = GetNode<Node2D>("Open");
        Closed = GetNode<Node2D>("Closed");

        Window Root = GetTree().Root;
        Main = Root.GetChild(Root.GetChildCount() - 1) as Main;
    }

    public override void _Process(double delta)
    {
        if (Main.GameState == GameState.Win) {
            Open.Show();
            Closed.Hide();
            SetCollisionLayerValue(1, false);
        } else {
            Open.Hide();
            Closed.Show();
            SetCollisionLayerValue(1, true);
        }
    }
}
