using Godot;


public struct BanditState
{
    public bool IsRunning;
    public bool IsAttacking;
    public bool IsAggro;
    public bool CanAttack;
}

public partial class Bandit : Actor
{
    [Export]
    public float AttackDistance = 100f;

    [Export]
    public float AttackSpeedMod = 0.5f;

    [Export]
    public int Damage = 1;

    public Line2D DebugLine;

    public AnimationPlayer Animation;
    public Area2D Aggro;
    public Area2D Attack;
    public Timer Cooldown;
    public Timer HitDelay;
    public Timer AttackAnimation;
    public Timer DeathTimer;
    public AudioStreamPlayer2D SoundAttack;

    public Vector2 Direction;

    public BanditState State {
        get => state;
    }

    private BanditState state;
    private float lastDirection;
    private Actor CurrentTarget;

    public override void _Ready()
    {
        Sprite = GetNode<Sprite2D>("Sprite");
        Animation = GetNode<AnimationPlayer>("Animation");
        Aggro = GetNode<Area2D>("Aggro");
        Cooldown = GetNode<Timer>("Cooldown");
        HitDelay = GetNode<Timer>("HitDelay");
        DeathTimer = GetNode<Timer>("DeathTimer");
        AttackAnimation = GetNode<Timer>("AttackAnimation");
        SoundAttack = GetNode<AudioStreamPlayer2D>("SoundAttack");

        Direction = Vector2.Zero;

        state = new BanditState {
            IsRunning = false,
            IsAttacking = false,
            IsAggro = false,
            CanAttack = true,
        };

        AttackAnimation.Timeout += AttackEnded;
        HitDelay.Timeout += ApplyAttack;
        DeathTimer.Timeout += OnDeath;

        base._Ready();
    }

    public override void _PhysicsProcess(double delta)
    {
        string animation = "";

        TryKill();

        if (!IsDead) {
            if (CurrentTarget != null) {
                // Check if the player is close enough to hit
                float distance = Position.DistanceTo(CurrentTarget.Position);
                if (distance <= AttackDistance) {
                    CurrentTarget.CanBeHit = true;
                } else {
                    CurrentTarget.CanBeHit = false;
                }

                if (!Cooldown.IsStopped() || !CurrentTarget.CanBeHit) {
                    state.CanAttack = false;
                } else {
                    state.CanAttack = true;
                }
            }

            // Move toward the player if target is set
            if (IsOnFloor() && CurrentTarget != null) {
                Vector2 targetPos = CurrentTarget.Position;
                Direction = new Vector2(
                    targetPos.X - Position.X,
                    Direction.Y
                ).Normalized();
            } else {
                Direction = new Vector2(0, Direction.Y);
            }

            // Movement
            Velocity = CalculateMoveVelocity(Velocity, Direction, Speed);
            Velocity += new Vector2(0, Gravity * (float) delta);

            MoveAndSlide();

            // Flip things based on facing direction
            float flipDirection;
            if (Direction.X != 0f) {
                if (Direction.X > 0f) {
                    Sprite.FlipH = true;
                    Sprite.Position = new Vector2(16, -72);
                } else {
                    Sprite.FlipH = false;
                    Sprite.Position = new Vector2(-16, -72);
                }

                flipDirection = lastDirection = Direction.X;
            } else {
                flipDirection = lastDirection;
            }

            if (CurrentTarget != null && state.CanAttack) {
                TryAttack(CurrentTarget);
            }
        }

        if (IsActive) {
            animation = GetNewAnimation(state.IsAttacking);
            if (animation != Animation.CurrentAnimation) {
                // Timer to track the duration of our attack animation.
                if (animation == "attack") {
                    AttackAnimation.Start();
                }

                Animation.Play(animation);
            }
        }

        if (IsDead && IsActive) {
            IsActive = false;
            DeathTimer.Start();
        }
    }

    public void SetAggro(Actor target)
    {
        if (!target.IsPlayer) return;
        state.IsAggro = true;
        CurrentTarget = target;
    }

    public void SetDeaggro(Actor target)
    {
        if (!target.IsPlayer) return;
        state.IsAggro = false;
        CurrentTarget = null;
    }

    public void TryAttack(Actor target)
    {
        if (!target.IsPlayer) return;

        // Set our attacking state.
        state.IsAttacking = true;
        // Start the hit delay timer to give the player a chance to move.
        HitDelay.Start();
    }

    public void ApplyAttack()
    {
        if (CurrentTarget == null) return;

        if (CurrentTarget.CanBeHit && !CurrentTarget.IsDead) {
            CurrentTarget.DealDamage(1);
        }
    }

    public void AttackEnded()
    {
        Cooldown.Start();
        state.IsAttacking = false;
    }

    public Vector2 CalculateMoveVelocity(
        Vector2 linearVelocity,
        Vector2 direction,
        Vector2 speed
    ) {
        Vector2 _velocity = linearVelocity;

        _velocity.X = speed.X * direction.X;
        if (state.IsAttacking) {
            _velocity.X *= AttackSpeedMod;
        }

        if (direction.Y != 0.0) _velocity.Y = speed.Y * direction.Y;

        return _velocity;
    }

    public void OnDeath()
    {
        QueueFree();
    }

    public string GetNewAnimation(bool isAttacking = false)
    {
        string animation_new = "";

        if (IsOnFloor()) {
            if (Mathf.Abs(Velocity.X) > 0.1f) {
                animation_new = "run";
            } else {
                animation_new = "idle";
            }
        }

        if (isAttacking) {
            animation_new = "attack";
        }

        if (IsDead) animation_new = "dead";

        return animation_new;
    }
}
