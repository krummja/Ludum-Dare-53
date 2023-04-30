using Godot;
using System;

public partial class Horn : Marker2D
{
    public Sprite2D HornToot;
    public Sprite2D TootText;
    public Timer Cooldown;
    public AudioStreamPlayer2D SoundToot;

    public override void _Ready()
    {
        HornToot = GetNode<Sprite2D>("HornSprite");
        TootText = HornToot.GetNode<Sprite2D>("Toot");
        Cooldown = GetNode<Timer>("Cooldown");
        SoundToot = GetNode<AudioStreamPlayer2D>("Toot");

        Cooldown.Timeout += Reset;
    }

    public bool Toot(float direction)
    {
        if (!Cooldown.IsStopped()) {
            return false;
        }

        if (direction < 0f) {
            Scale = new Vector2(-1, 1);
        } else {
            Scale = new Vector2(1, 1);
        }

        HornToot.Show();

        SoundToot.Play();
        Cooldown.Start();
        return true;
    }

    private void Reset()
    {
        HornToot.Hide();
    }
}
