using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    public const float SPEED = 100.0f;
    
    private AnimatedSprite2D animatedSprite;
    private CharacterBody2D Player;

    private enum MovStates {
        IDLE, WALK, ATTACK, HURT, DEAD
    }

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        Player = GetNode<CharacterBody2D>("/root/Game/Player");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = Velocity;
        Vector2 direction = GlobalPosition.DirectionTo(Player.GlobalPosition);

        velocity.X = Mathf.MoveToward(velocity.X, direction.X * SPEED, SPEED);
        velocity.Y = Mathf.MoveToward(velocity.Y, direction.Y * SPEED, SPEED);

        animatedSprite.Play("walk");

        Velocity = velocity;
        MoveAndSlide();
    }


} 