using Godot;
using System;

public partial class Bow : Marker2D
{
    private Marker2D shootingPoint;
    private Timer timer;

    public void Shoot()
    {
        PackedScene arrowScene = ResourceLoader.Load<PackedScene>("res://scenes/Arrow.tscn");
        Area2D arrow = arrowScene.Instantiate<Area2D>();
        shootingPoint.AddChild(arrow);
        arrow.GlobalPosition = shootingPoint.GlobalPosition;
        timer.Start(Player.Instance.SHOOT_TIMEOUT);
    }

    public override void _Ready()
    {
        shootingPoint = GetNode<Marker2D>("Sprite/ShootingPoint");
        timer = GetNode<Timer>("Timer");
        timer.OneShot = true;
    }

    public override void _PhysicsProcess(double delta)
    {
        if(Player.Instance.Health > 0) {
            LookAt(GetGlobalMousePosition());

            if(Input.IsActionJustPressed("ui_click") && GetNodeOrNull<Area2D>("Arrow") == null && timer.IsStopped())
            {
                Shoot();
            } 
        }   
    }

}
