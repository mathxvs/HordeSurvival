using Godot;
using System;

public partial class Bow : Area2D
{
    private readonly Area2D arrow = ResourceLoader.Load<Area2D>("res://scenes/Arrow.tscn");
    private static Marker2D shootingPoint;

    public void Shoot() {
        arrow.GlobalPosition = shootingPoint.GlobalPosition;
    }

    public override void _Ready()
    {
        shootingPoint = GetNode<Marker2D>("ShootingPoint");
        shootingPoint.AddChild(arrow);
    }

    public override void _Process(double delta)
    {
        if(Input.IsActionJustPressed("ui_click")) {
            Shoot();
        }
    }


    public override void _PhysicsProcess(double delta)
    {
        LookAt(GetGlobalMousePosition());
    }

}
