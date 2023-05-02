using Godot;
using System;

public partial class Actor : CharacterBody2D
{
    [Export]
    public Vector2 Speed = new Vector2(150f, 350f);

    [Export]
    public int Health = 3;

    [Export]
    public int ScrollPieces = 0;

    [Export]
    public bool IsPlayer = false;

    [Export]
    public bool IsDead = false;

    public bool CanBeHit = false;

    public bool IsActive = true;

    public Timer HurtFlash;
    public Sprite2D Sprite;
    public AnimationPlayer HitShake;
    public AudioStreamPlayer2D SoundHurt;
    public AudioStreamPlayer2D SoundDead;

    public Vector2 FloorNormal = Vector2.Up;

    private float gravity;
    public float Gravity
    {
        get => (float) this.gravity;
    }

    private bool IsHurt = false;

    public override void _Ready()
    {
        gravity = (float) ProjectSettings.GetSetting("physics/2d/default_gravity");
        HurtFlash = GetNode<Timer>("HurtFlash");
        Sprite = GetChild<Sprite2D>(1);
        HitShake = GetNode<AnimationPlayer>("HitShake");
        SoundHurt = GetNode<AudioStreamPlayer2D>("Hurt");
        SoundDead = GetNode<AudioStreamPlayer2D>("Dead");

        HurtFlash.Timeout += OnHurtEnded;
    }

    public override void _Process(double delta)
    {
        if (IsDead) CanBeHit = false;
    }

    public void GainScrollPiece()
    {
        ScrollPieces += 1;
    }

    public void GainHealth()
    {
        if (Health < 3)
        {
            Health += 1;
        }
    }

    public void DealDamage(int amount)
    {
        Health -= amount;
        IsHurt = true;
        HurtFlash.Start();
        HitShake.Play("hurt");

        if (Health > 0) {
            SoundHurt.Play();
        } else {
            SoundDead.Play();
        }
    }

    public void TryKill()
    {
        if (Health <= 0) {
            IsDead = true;
        }
    }

    public void OnHurtEnded()
    {
        IsHurt = false;
    }
}
