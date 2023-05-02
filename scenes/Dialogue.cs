using Godot;
using System.Threading.Tasks;

public partial class Dialogue : Control
{
    [Export]
    public RichTextLabel Content;

    public Timer TypeTimer;
    public Timer PauseTimer;

    public string Text = @"";

    public async override void _Ready()
    {
        TypeTimer = GetNode<Timer>("TypeTimer");
        PauseTimer = GetNode<Timer>("PauseTimer");

        TypeTimer.Timeout += OnTypeTimerTimeout;

        await StartDialogue();
    }

    public async Task StartDialogue()
    {
        await ToSignal(GetTree().CreateTimer(1.0f), "timeout");
        UpdateMessage(Text);
    }

    public void UpdateMessage(string message)
    {
        Content.Text = message;
        Content.VisibleCharacters = 0;
        TypeTimer.Start();
    }

    private void OnTypeTimerTimeout()
    {
        if (Content.VisibleCharacters < Content.Text.Length) {
            Content.VisibleCharacters += 1;
        } else {
            TypeTimer.Stop();
        }
    }

    public void Open()
    {
        Show();
    }
}
