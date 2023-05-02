using Godot;

public partial class GameOver : Control
{
    [Export]
    public Button QuitButton;

    [Export]
    public Control StartScreen;

    private Window Root;
    private Node SceneRoot;

    public override void _Ready()
    {
        Root = GetTree().Root;
        SceneRoot = Root.GetChild(Root.GetChildCount() -1);
        QuitButton.Pressed += OnQuitPressed;
        Hide();
    }

    public void Open()
    {
        QuitButton.GrabFocus();
        Show();
    }

    public void Close()
    {
        GetTree().Paused = false;
        Hide();
    }

    public void OnQuitPressed()
    {
        Node world = SceneRoot.GetNode<Node>("World");
        SceneRoot.RemoveChild(world);

        Start start = StartScreen as Start;

        start.Open();
        Close();
    }
}
