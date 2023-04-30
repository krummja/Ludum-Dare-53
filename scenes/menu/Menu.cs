using Godot;

public partial class Menu : Control
{
    [Export]
    public Button ResumeButton;

    [Export]
    public Button QuitButton;

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

    public void Close()
    {
        GetTree().Paused = false;
        Hide();
    }

    public void Open()
    {
        Show();
        ResumeButton.GrabFocus();
        GetTree().Paused = true;
    }

    public void OnResumePressed()
    {
        Close();
    }

    public void OnQuitPressed()
    {
        Root.PropagateNotification((int) NotificationWMCloseRequest);
        GetTree().Quit();
    }
}
