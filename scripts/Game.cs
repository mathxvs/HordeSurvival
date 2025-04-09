using Godot;
using System;

public partial class Game : Node2D
{
    public static RandomNumberGenerator rng = new();
    PathFollow2D spawnPoint;
    CanvasLayer gameOverScreen;
    Timer timer;

    public static int enemyCount = 0;
    
    private void SpawnEnemy() {
        PackedScene enemyScene = ResourceLoader.Load<PackedScene>("res://scenes/Enemy.tscn");
        CharacterBody2D enemy = enemyScene.Instantiate<CharacterBody2D>();
        spawnPoint.ProgressRatio = rng.Randf();
        
        enemy.GlobalPosition = spawnPoint.GlobalPosition;
        AddChild(enemy);
    }

    public void OnSpawnTimerTimeout() {
        if(enemyCount < 10) {
            SpawnEnemy();
        }
    }

    public override void _Ready()
    {
        spawnPoint = GetNode<PathFollow2D>("SpawnPath/PointSampler");
        timer = GetNode<Timer>("SpawnTimer");
        gameOverScreen = GetNode<CanvasLayer>("GameOver");
    }

    public override void _Process(double delta)
    {
        if(Player.Instance.Health <= 0) {
            gameOverScreen.Visible = true;
        }
    }

}
