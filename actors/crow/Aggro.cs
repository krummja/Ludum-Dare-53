using Godot;
using System;
using System.Collections.Generic;

public partial class Aggro : Area2D
{
    [Export]
    public float Speed = 0.1f;

    public Transform2D Target;
    public Area2D Agent;

    private Stack<Vector2> Points;
    private Vector2[] debugPoints;
    private Tween Tween;

    public override void _Ready()
    {
        Agent = GetChild<Area2D>(1);
        BodyEntered += OnBodyEntered;
    }

    public override void _Process(double delta)
    {
        QueueRedraw();
    }

    public void OnBodyEntered(Node2D body)
    {
        Target = body.GlobalTransform;
        Attack(Target.Origin);
    }

    public void Attack(Vector2 target)
    {
        Vector2 midPoint = (GlobalPosition + target) / 2;
        debugPoints = CreateArcPoints(target, midPoint, GlobalPosition, 50);

        Points = new Stack<Vector2>();

        for (int i = 0; i < debugPoints.Length; i++) {
            Points.Push(debugPoints[i]);
        }

        Points = new Stack<Vector2>(Points);
        Agent.Position = Points.Pop();
        CallDeferred("Begin");
    }

    public void Begin()
    {
        Tween = CreateTween()
            .BindNode(this)
            .SetTrans(Tween.TransitionType.Linear);

        Tween.Finished += MoveToNextPoint;
        MoveToNextPoint();
    }

    public void MoveToNextPoint()
    {
        if (Points.Count == 0) {
            Tween.Finished -= MoveToNextPoint;
            return;
        }

        Vector2 nextPoint = Points.Pop();
        Tween.Stop();
        Tween.TweenProperty(Agent, "position", nextPoint, Speed);
        Tween.Play();
    }

    public Vector2[] CreateArcPoints(Vector2 p0, Vector2 p1, Vector2 p2, int steps)
    {
        Vector2[] points = new Vector2[steps];
        float steppingConstant = 1f / (float) steps;

        for (int i = 0; i < steps; i++) {
            Vector2 i_p0_p1 = p0.Lerp(p1, i * steppingConstant);
            Vector2 i_p1_p2 = p1.Lerp(p2, i * steppingConstant);

            points[i] = i_p0_p1.Lerp(i_p1_p2, i * steppingConstant);
        }

        return points;
    }

    public override void _Draw()
    {
        if (debugPoints != null && debugPoints.Length > 0) {
            for (int i = 0; i < debugPoints.Length; i++) {
                DrawCircle(debugPoints[i], 3f, new Color(1, 0, 0));
            }
        }

        if (Target != null) {
            DrawLine(Agent.Position, Target.Origin, new Color(1, 1, 0), 2f);
        }
    }
}
