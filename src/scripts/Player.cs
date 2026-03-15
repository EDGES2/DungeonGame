using System;
using Godot;

namespace Game.Player;

public partial class Player : CharacterBody2D
{
    [Export] public float speed = 150f;
    [Export] public float friction = 700f;

    public enum PlayerState
    {
        Idle,
        Running,
        Stopping,
        Attacking
    }

    private PlayerState _currentState = PlayerState.Idle;
    private AnimatedSprite2D _playerSprite2D;
    private CollisionShape2D _collisionShape;
    private float _shapeOffsetX = -2.0f;
    private float _shapeOffsetY = -3.0f;

    public override void _Ready()
    {
        _playerSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _playerSprite2D.AnimationFinished += OnAnimationFinished;
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("left", "right", "up", "down");

        if (_currentState != PlayerState.Attacking)
        {
            if (direction.X > 0)
                FlipCharacter(false);
            else if (direction.X < 0)
                FlipCharacter(true);
        }


        switch (_currentState)
        {
            case PlayerState.Idle:
                HandleIdleState(direction);
                break;
            case PlayerState.Running:
                HandleRunningState(direction);
                break;
            case PlayerState.Stopping:
                HandleStoppingState(direction, delta);
                break;
            case PlayerState.Attacking:
                HandleAttackingState(delta);
                break;
        }

        MoveAndSlide();
    }


    private void HandleIdleState(Vector2 direction)
    {
        _playerSprite2D.Play("idle");
        Velocity = Vector2.Zero;

        if (Input.IsActionJustPressed("melee_attack"))
            _currentState = PlayerState.Attacking;
        else if (direction != Vector2.Zero)
            _currentState = PlayerState.Running;
    }

    private void HandleRunningState(Vector2 direction)
    {
        _playerSprite2D.Play("run");
        Velocity = direction * speed;

        if (Input.IsActionJustPressed("melee_attack"))
            _currentState = PlayerState.Attacking;
        else if (direction == Vector2.Zero)
            _currentState = PlayerState.Stopping;
    }

    private void HandleStoppingState(Vector2 direction, double delta)
    {
        _playerSprite2D.Play("stop");
        Velocity = Velocity.MoveToward(Vector2.Zero, friction * (float)delta);

        if (Input.IsActionJustPressed("melee_attack"))
            _currentState = PlayerState.Attacking;
        else if (direction != Vector2.Zero)
            _currentState = PlayerState.Running;
    }

    private void HandleAttackingState(double delta)
    {
        _playerSprite2D.Play("melee_attack");
        Velocity = Velocity.MoveToward(Vector2.Zero, 0.5f * friction * (float)delta);
    }

    private void OnAnimationFinished()
    {
        if (_currentState == PlayerState.Attacking)
            _currentState = PlayerState.Idle;
        else if (_currentState == PlayerState.Stopping && _playerSprite2D.Animation == "stop")
            _currentState = PlayerState.Idle;
    }
    private void FlipCharacter(bool isFacingLeft)
    {
        _playerSprite2D.FlipH = isFacingLeft;
        Vector2 newPosition = _collisionShape.Position;

        if (isFacingLeft)
        {
            newPosition.X = +(_shapeOffsetX * 3);
        }
        else
        {
            newPosition.X = -(_shapeOffsetX);
        }

        _collisionShape.Position = newPosition;
    }
}
