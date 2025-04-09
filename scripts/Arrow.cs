using Godot;
using System;

public partial class Arrow : Area2D
{
    public const float SPEED = 1200;
    public const float RANGE = 900;
    public const float DAMAGE = 15;

    private float travelledDistance = 0;

    public void OnBodyEntered(Enemy body)
    {
        if(body.HasMethod("HandleDamage")) {
            body.HandleDamage(DAMAGE);
        }

        QueueFree();
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Vector2.Right.Rotated(Rotation);
        Position += direction * SPEED * (float)delta;

        travelledDistance += SPEED * (float)delta;

        if(travelledDistance >= RANGE) {
            QueueFree();
        }
    }
}
