using Godot;


public partial class ThroneRoom : Node2D
{
    public Dialogue Dialogue;
    public Area2D EventBox;
    public Marker2D CameraTarget;
    public Timer CameraTimer;
    public Timer EventTimer;
    public Timer EventTimer2;

    public Node TextBox;
    public Player Player;

    public override void _Ready()
    {
        CameraTarget = GetNode<Marker2D>("CameraTarget");

        EventBox = GetNode<Area2D>("EventBox");
        EventBox.BodyEntered += OnEventBoxEntered;

        CameraTimer = GetNode<Timer>("CameraTimer");
        CameraTimer.Timeout += StartDelivery;

        EventTimer = GetNode<Timer>("EventTimer");
        EventTimer.Timeout += StartKing;

        EventTimer2 = GetNode<Timer>("EventTimer2");
        EventTimer2.Timeout += EndGameScene;
    }

    public void OnEventBoxEntered(Node2D body)
    {
        Actor actor = body as Actor;

        if (actor.IsPlayer) {
            Player = actor as Player;

            Player.IsActive = false;

            Player.MoveTimer.Timeout += () => CameraTimer.Start();
            Player.Animation.Play("camera");

            Player.MoveTimer.Start();
        }
    }

    public void StartDelivery()
    {
        TextBox = ResourceLoader
            .Load<PackedScene>("res://scenes/TextOverlay.tscn")
            .Instantiate();

        Dialogue dialogue = TextBox as Dialogue;
        dialogue.Text = @"Hear Ye! Hear Ye!
            I come bearing a message for the king!
            Through many perils I come to deliver the news ...

            ... that your kingdom is overrun with bandits!";

        Player.HUD.AddChild(TextBox);
        dialogue.TypeTimer.Timeout += () => EventTimer.Start();
        dialogue.Open();
    }

    public void StartKing()
    {
        Player.HUD.RemoveChild(TextBox);
        TextBox = ResourceLoader
            .Load<PackedScene>("res://scenes/TextOverlay.tscn")
            .Instantiate();

        Dialogue dialogue = TextBox as Dialogue;
        dialogue.Text = @"
            How dare ye come to my throne and accuse me of harboring thieves! My kingdom is perfect! I will not have it!

            TO EXILE!
        ";

        Player.HUD.AddChild(TextBox);
        dialogue.TypeTimer.Timeout += () => EventTimer2.Start();
        dialogue.Open();
    }

    public void EndGameScene()
    {
        Player.HUD.RemoveChild(TextBox);
        Window Root = GetTree().Root;
        Node SceneRoot = Root.GetChild(Root.GetChildCount() - 1);
        Main main = SceneRoot as Main;
        main.GameOver();
    }
}
