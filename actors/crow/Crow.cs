using Godot;
using System;
using System.Collections.Generic;

public partial class Crow : Area2D
{
    [Export]
    public float Speed = 0.1f;

    public AnimationPlayer Animation;
    public Area2D Spawn;

    public override void _Ready()
    {
        Animation = GetNode<AnimationPlayer>("Animation");
        Spawn = GetParent<Area2D>();
    }

    public override void _PhysicsProcess(double delta)
    {
        string animation = GetNewAnimation();

        if (animation != Animation.CurrentAnimation) {
            Animation.Play(animation);
        }
    }

    public string GetNewAnimation()
    {
        string animation_new = "";
        animation_new = "crow_fly";
        return animation_new;
    }
}
