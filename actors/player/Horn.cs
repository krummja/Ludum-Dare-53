using Godot;
using System;

public partial class Horn : Marker2D
{
    public Sprite2D HornToot;
    public Sprite2D TootText;
    public Timer Cooldown;
    public AudioStreamPlayer2D SoundToot;
    public Area2D HitArea;

    public Actor CurrentTarget;

    public override void _Ready()
    {
        HornToot = GetNode<Sprite2D>("HornSprite");
        TootText = HornToot.GetNode<Sprite2D>("Toot");
        Cooldown = GetNode<Timer>("Cooldown");
        SoundToot = GetNode<AudioStreamPlayer2D>("Toot");
        HitArea = GetParent().GetNode<Area2D>("HitArea");

        Cooldown.Timeout += Reset;
        HitArea.BodyEntered += OnBodyEntered;
        HitArea.BodyExited += OnBodyExited;
    }

    public bool Toot(float direction)
    {
        if (!Cooldown.IsStopped()) return false;

        if (direction < 0f) {
            Scale = new Vector2(-1, 1);
        } else {
            Scale = new Vector2(1, 1);
        }

        HornToot.Show();

        SoundToot.Play();
        Cooldown.Start();

        if (CurrentTarget != null && CurrentTarget.CanBeHit) {
            CurrentTarget.DealDamage(1);
        }

        return true;
    }

    public void OnBodyEntered(Node2D body)
    {
        Actor actor = body as Actor;
        if (actor.IsPlayer) return;
        actor.CanBeHit = true;
        CurrentTarget = actor;
    }

    public void OnBodyExited(Node2D body)
    {
        Actor actor = body as Actor;
        if (actor.IsPlayer) return;
        actor.CanBeHit = false;
    }

    private void Reset()
    {
        HornToot.Hide();
    }
}
