using Godot;
using System;

public partial class Player : CharacterBody2D
{
    private enum MovStates {
        IDLE, WALK, ATTACK, HURT, DEAD
    }

    public const float SPEED = 150.0f;    
    private AnimatedSprite2D animatedSprite;

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        Vector2 direction = Input.GetVector("ui_left", "ui_right", "ui_up", "ui_down");

        if(direction != Vector2.Zero) {
            velocity.X = Mathf.MoveToward(velocity.X, direction.X * SPEED, SPEED);
            velocity.Y = Mathf.MoveToward(velocity.Y, direction.Y * SPEED, SPEED);

            animatedSprite.FlipH = direction.X < 0;
            animatedSprite.Play("walk");            
        } else {
            velocity.X = Mathf.MoveToward(velocity.X, 0, SPEED);
            velocity.Y = Mathf.MoveToward(velocity.Y, 0, SPEED);

            animatedSprite.Play("idle");
        }

        Velocity = velocity;
        MoveAndSlide();
    }


} 