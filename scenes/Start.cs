using Godot;


public partial class Start : Control
{
    public Button StartButton;
    public Button QuitButton;
    public Node WorldScene;
    public Node StartScene;

    private Window Root;
    private Node SceneRoot;

    public override void _Ready()
    {
        Root = GetTree().Root;
        SceneRoot = Root.GetChild(Root.GetChildCount() - 1);

        Control buttonGroup = GetNode<VBoxContainer>("Buttons");
        StartButton = buttonGroup.GetNode<Button>("Start");
        StartButton.Pressed += OnStartPressed;

        QuitButton = buttonGroup.GetNode<Button>("Quit");
        QuitButton.Pressed += OnQuitPressed;

        StartButton.GrabFocus();
    }

    public void Open()
    {
        Show();
        StartButton.GrabFocus();
    }

    public void OnStartPressed()
    {
        StartScene = ResourceLoader.Load<PackedScene>("res://scenes/game/World.tscn").Instantiate();
        SceneRoot.AddChild(StartScene);

        SceneTree tree = GetTree();
        if (tree.Paused) tree.Paused = false;

        Main main = SceneRoot as Main;
        main.GameState = GameState.Game;

        Hide();
    }

    public void OnQuitPressed()
    {
        Root.PropagateNotification((int) NotificationWMCloseRequest);
        GetTree().Quit();
    }
}
