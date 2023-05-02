using Godot;

public partial class Menu : Control
{
    [Export]
    public Button ResumeButton;

    [Export]
    public Button QuitButton;

    [Export]
    public Control StartScreen;

    private Window Root;
    private Node SceneRoot;

    public override void _Ready()
    {
        Root = GetTree().Root;
        SceneRoot = Root.GetChild(Root.GetChildCount() - 1);

        ResumeButton.Pressed += OnResumePressed;
        QuitButton.Pressed += OnQuitPressed;

        Hide();
    }

    public override void _Input(InputEvent @event)
    {
        if (GetTree().Paused && @event.IsActionPressed("ui_cancel")) {
            Close();
        }
    }

    public void Close()
    {
        GetTree().Paused = false;
        Hide();
    }

    public void Open()
    {
        Show();
        ResumeButton.GrabFocus();
    }

    public void OnResumePressed()
    {
        Close();
    }

    public void OnQuitPressed()
    {
        Main main = SceneRoot as Main;
        Node currentScene;

        Start start = StartScreen as Start;

        switch (main.GameState) {

            case GameState.StoryStart:
                currentScene = SceneRoot.GetNode<Node>("StoryStart");
                SceneRoot.RemoveChild(currentScene);

                start.Open();
                break;

            case GameState.Game:
                currentScene = SceneRoot.GetNode<Node>("World");
                SceneRoot.RemoveChild(currentScene);

                start.Open();
                break;

            default:
                break;
        }

        Hide();
    }
}
