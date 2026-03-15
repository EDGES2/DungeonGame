using System;
using Game.Components;
using Godot;

namespace Game.Player;

public partial class Player : CharacterBody2D
{
    [Export] public float speed = 150f;
    [Export] public float friction = 700f;
    [Export] public Weapon EquippedWeapon;

    private AnimatedSprite2D _playerSprite2D;
    private CollisionShape2D _collisionShape;
    private AnimationNodeStateMachinePlayback _stateMachine;

    private float _shapeOffsetX = -2.0f;

    public override void _Ready()
    {
        _playerSprite2D = GetNode<AnimatedSprite2D>("AnimatedSprite2D");
        _collisionShape = GetNode<CollisionShape2D>("CollisionShape2D");

        AnimationTree animTree = GetNode<AnimationTree>("AnimationTree");
        _stateMachine = animTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 direction = Input.GetVector("left", "right", "up", "down");
        bool isTurningAround = direction != Vector2.Zero && Velocity.Length() > 10f && Velocity.Dot(direction) < 0;

        if (direction != Vector2.Zero && !isTurningAround)
        {
            Velocity = direction * speed;
            FlipCharacter(direction.X < 0);
        }
        else
        {
            Velocity = Velocity.MoveToward(Vector2.Zero, friction * (float)delta);
        }

        MoveAndSlide();

        if (Input.IsActionJustPressed("melee_attack"))
        {
            _stateMachine.Travel("Attack");
        }
        else if (direction != Vector2.Zero && !isTurningAround)
        {
            _stateMachine.Travel("Run");
        }
        else if (_stateMachine.GetCurrentNode() == "Run")
        {
            _stateMachine.Travel("Stop");
        }
    }

    private void FlipCharacter(bool isFacingLeft)
    {
        _playerSprite2D.FlipH = isFacingLeft;
        Vector2 newPosition = _collisionShape.Position;

        newPosition.X = isFacingLeft ? +_shapeOffsetX - 4 : -_shapeOffsetX;

        _collisionShape.Position = newPosition;
    }

    // public void DisableWeapon()
    // {
    //     EquippedWeapon?.DisableWeapon();
    // }
}
