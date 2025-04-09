using Godot;
using System;

public partial class Enemy : CharacterBody2D
{
    public enum MovStates {
        IDLE, WALK, ATTACK, HURT, DEATH
    }

    public const float SPEED = 75.0f;
    public float DAMAGE = 3.0f;
    public const float ATTACK_TIMEOUT = 1.0f;
    public const float FADE_OUT_TIMEOUT = 2.0f;
    public float Health = 20.0f;

    private MovStates movState;
    
    private AnimatedSprite2D animatedSprite;
    private Area2D hitBox;
    private CollisionShape2D hitBoxCollision;
    private CharacterBody2D playerNode;
    private CollisionShape2D playerHurtBox;
    private Timer timer;

    private static readonly Player player = Player.Instance;

    public void HandleDamage(float damage)
    {
        Health -= damage;
        movState = MovStates.HURT;
        HandleAnimation();

        if(Health <= 0) {
            movState = MovStates.DEATH;
            HandleAnimation();
            hitBoxCollision.Disabled = true;
            timer.Start(FADE_OUT_TIMEOUT);

            timer.Timeout += () => QueueFree();
        }
    }

    public void HandleAttack()
    {
        if(player.Health > 0) {
            movState = MovStates.ATTACK;

            if(timer.IsStopped()) {
                HandleAnimation();

                player.HandleDamage(DAMAGE);          

                timer.Start(ATTACK_TIMEOUT);
            }
        }
    }

    private void HandleAnimation()
    {
        animatedSprite.Stop();

        void onAttackAnimationFinished () {
            animatedSprite.Stop();
            movState = MovStates.WALK;
            HandleAnimation();
        }

        void onHurtAnimationFinished() {
            movState = MovStates.WALK;
            HandleAnimation();
        }

        void onDeathAnimationFinished() {
            int frameCount = animatedSprite.SpriteFrames.GetFrameCount("death");
            animatedSprite.Stop();
            animatedSprite.Frame = frameCount - 1;
        }

        switch(movState) {
            case MovStates.IDLE:
                animatedSprite.Play("idle");
                break;
            case MovStates.WALK:
                animatedSprite.Play("walk");
                break;
            case MovStates.ATTACK: 
                int animNumber = Game.rng.RandiRange(0, 1);
                string animation = animNumber == 0 ? "attack_01" : "attack_02";
                animatedSprite.Play(animation);

                animatedSprite.AnimationFinished += onAttackAnimationFinished;

                break;
            case MovStates.HURT:
                animatedSprite.Play("hurt");

                animatedSprite.AnimationFinished += onHurtAnimationFinished;

                break;
            case MovStates.DEATH:
                animatedSprite.Play("death");

                animatedSprite.AnimationFinished += onDeathAnimationFinished;
                animatedSprite.AnimationFinished -= onAttackAnimationFinished;
                animatedSprite.AnimationFinished -= onHurtAnimationFinished;
                break;
        }
    }

    public void OnEnemyTreeEnter() {
        Game.enemyCount += 1;
    }

    public void OnEnemyTreeExit() {
        Game.enemyCount -= 1;
    }

    public override void _Ready()
    {
        animatedSprite = GetNode<AnimatedSprite2D>("AnimatedSprite");
        hitBox = GetNode<Area2D>("HitBox");
        hitBoxCollision = GetNode<CollisionShape2D>("HitBox/Collision");
        playerNode = GetNode<CharacterBody2D>("/root/Game/Player");
        playerHurtBox = GetNode<CollisionShape2D>("/root/Game/Player/HurtBox");
        timer = GetNode<Timer>("Timer");
        timer.OneShot = true;

        movState = MovStates.WALK;
        HandleAnimation();
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Health > 0 && player.Health > 0) {
            Vector2 velocity = Velocity;
            Vector2 direction = GlobalPosition.DirectionTo(player.GlobalPosition);
            animatedSprite.FlipH = direction.X < 0;

        if(movState == MovStates.WALK) {
            velocity.X = Mathf.MoveToward(velocity.X, direction.X * SPEED, SPEED);
            velocity.Y = Mathf.MoveToward(velocity.Y, direction.Y * SPEED, SPEED);
        }

            if(hitBox.OverlapsBody(playerNode)) {
                velocity.X = Mathf.MoveToward(velocity.X, 0, SPEED);
                velocity.Y = Mathf.MoveToward(velocity.Y, 0, SPEED);

                HandleAttack();
            }

            Velocity = velocity;
            MoveAndSlide();
        }
    }
} 