using Godot;
using System;

public partial class Arrow : Area2D
{
    private const float SPEED = 1200;
    private const float RANGE = 900;

    public override void _PhysicsProcess(double delta)
    {
        float travelledDistance = 0;

        Vector2 direction = Vector2.Right.Rotated(Rotation);
        Position += direction * SPEED * (float)delta;

        travelledDistance += SPEED * (float)delta;
        if(travelledDistance > RANGE) {
            QueueFree();
        }
    }

}
