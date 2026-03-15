using System;
using Godot;

namespace Game.Player;

public partial class Player : CharacterBody2D
{
    [Export] public float speed = 150f;
    [Export] public float friction = 700f;
    private bool _isStopping = false;

    private AnimatedSprite2D _playerSprite2D;

    public void GetInput(double delta)
    {

        Vector2 direction = Input.GetVector("left", "right", "up", "down");


        //Movement handling
        if (direction != Vector2.Zero)
        {
            Velocity = direction * speed;
            _isStopping = false;
            _playerSprite2D.Play("run");
        }
        else
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, friction * (float)delta);
            if (!_isStopping)
            {
                if (_playerSprite2D.Animation == "run")
                {
                    _isStopping = true;
                    _playerSprite2D.Play("stop");
                }
                else if (_playerSprite2D.Animation != "stop")
                {
                    _playerSprite2D.Play("idle");
                }
            }
        }


        //Direction change
        if (direction.X > 0)
            _playerSprite2D.FlipH = false;
        else if (direction.X < 0)
            _playerSprite2D.FlipH = true;

    }

    private void OnAnimationFinished()
    {
        if (_playerSprite2D.Animation == "stop")
        {
            _isStopping = false;
            _playerSprite2D.Play("idle");
        }
    }

    // Called when the node enters the scene tree for the first time.
    public override void _Ready()
    {
        _playerSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _playerSprite2D.AnimationFinished += OnAnimationFinished;
    }
    public override void _PhysicsProcess(double delta)
    {
        GetInput(delta);
        MoveAndSlide();
    }
    // Called every frame. 'delta' is the elapsed time since the previous frame.
    public override void _Process(double delta)
    {
    }
}
