using Godot;
using System;

public partial class Player : CharacterBody2D
{
    public static Player Instance { get; private set; }

    public enum MovStates {
        IDLE, WALK, ATTACK, HURT, DEATH
    }

    public float SPEED = 200.0f;    
    public float SHOOT_TIMEOUT = 0.5f;

    public float FullHealth = 100.0f;
    public float Health;
    
    private MovStates movState;

    private AnimatedSprite2D animatedSprite;
    private Sprite2D healthBar;

    public void HandleDamage(float damage)
    {
        if(Health > 0) {
            Health -= damage;
            movState = MovStates.HURT;
            HandleAnimation();
        }

        if(Health <= 0) {
            movState = MovStates.DEATH;
            HandleAnimation();
        }

        Vector2 healthBarScale = new(Health / FullHealth, 1);
        healthBar.Scale = healthBarScale;
    }

    private void HandleAnimation()
    {
       void onHurtAnimationFinished () {
            animatedSprite.Stop();
            movState = MovStates.IDLE;
            HandleAnimation();
        };

        void onDeathAnimationFinished() {
            int frameCount = animatedSprite.SpriteFrames.GetFrameCount("death");
            animatedSprite.Stop();
            animatedSprite.Frame = frameCount - 1;
        }

        switch(movState) {
            default:
            case MovStates.IDLE:
                animatedSprite.Play("idle");
                break;
            case MovStates.WALK:
                animatedSprite.Play("walk");
                break;
            case MovStates.HURT:
                animatedSprite.Play("hurt");
                animatedSprite.AnimationFinished += onHurtAnimationFinished;
                break;
            case MovStates.DEATH:
                animatedSprite.Play("death");
                
                animatedSprite.AnimationFinished += onDeathAnimationFinished;
                animatedSprite.AnimationFinished -= onHurtAnimationFinished;
                break;
        }
    }

    public override void _Ready()
    {
        Instance = this;
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite");
        healthBar = GetNode<Sprite2D>("HPBar/FG");

        Health = FullHealth;
        movState = MovStates.IDLE;
        HandleAnimation();
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Health > 0) {
            Vector2 velocity = Velocity;
            Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");
            animatedSprite.FlipH = direction.X < 0;

            if(direction != Vector2.Zero) {
                velocity.X = Mathf.MoveToward(velocity.X, direction.X * SPEED, SPEED);
                velocity.Y = Mathf.MoveToward(velocity.Y, direction.Y * SPEED, SPEED);

                movState = MovStates.WALK;

            } else {
                velocity.X = Mathf.MoveToward(velocity.X, 0, SPEED);
                velocity.Y = Mathf.MoveToward(velocity.Y, 0, SPEED);
            }

            Velocity = velocity;
            HandleAnimation();
            MoveAndSlide();
        }
    }


} 