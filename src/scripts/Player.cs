using System;
using Godot;

namespace Game.Player;

public partial class Player : CharacterBody2D
{
    [Export] public float speed = 150f;

    private AnimatedSprite2D _playerSprite2D;

    public void GetInput()
    {
        Vector2 direction = Input.GetVector("left", "right", "up", "down");

        //Movement handling
        if (direction != Vector2.Zero)
        {
            Velocity = direction * speed;
        }
        else
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, speed);
        }

        //Direction change
        if (direction.X > 0)
            _playerSprite2D.FlipH = false;
        else if (direction.X < 0)
            _playerSprite2D.FlipH = true;
        Vector2 velocity = Velocity;
        velocity = direction * speed;
        Velocity = velocity;

    }
    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _playerSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
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
