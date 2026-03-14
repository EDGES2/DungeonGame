using System;
using Godot;

namespace Game.Player;

public partial class Player : CharacterBody2D
{
    [Export] public float speed = 150f;

    public void GetInput()
    {
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        if (direction != Vector2.Zero)
        {
            Velocity = direction * speed;
        }
        else
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, speed);
        }
        Vector2 velocity = Velocity;
        velocity = direction * speed;
        Velocity = velocity;

    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
    }
    public override void _PhysicsProcess(double delta)
    {
        GetInput();
        MoveAndSlide();
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
