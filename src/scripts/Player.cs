using System;
using Game.Components;
using Godot;

namespace Game.Player;

public partial class Player : CharacterBody2D
{
	[Export] public float speed = 150f;
	[Export] public float friction = 700f;
	[Export] private CollisionShape2D _collisionShape;
	[Export] private AnimatedSprite2D _playerSprite2D;


	private AnimationNodeStateMachinePlayback _stateMachine;
	private Node2D _directionalPhysics;
	private AnimationTree _animTree;


	private int _facingDirection = 1;
	private float _shapeOffsetX = -2.0f;
	private float _shapeOffsetXShift = 2.0f;

	public override void _Ready()
	{
		_directionalPhysics = GetNode<Node2D>("DirectionalPhysics");

		_animTree = GetNode<AnimationTree>("AnimationTree");
		_stateMachine = _animTree.Get("parameters/playback").As<AnimationNodeStateMachinePlayback>();
	}

	public override void _PhysicsProcess(double delta)
	{
		Vector2 direction = Input.GetVector("left", "right", "up", "down");
		if (direction != Vector2.Zero)
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
		else if (direction != Vector2.Zero)
		{
			_stateMachine.Travel("Run");
		}
		else if (_stateMachine.GetCurrentNode() == "Run")
		{
			_stateMachine.Travel("Idle");
		}
	}

	private void FlipCharacter(bool isFacingLeft)
	{
		int newDirection = isFacingLeft ? -1 : 1;
		_playerSprite2D.FlipH = isFacingLeft;

		Vector2 newPosition = _collisionShape.Position;
		newPosition.X = isFacingLeft ? +_shapeOffsetX - _shapeOffsetXShift : -_shapeOffsetX - _shapeOffsetXShift;
		_collisionShape.Position = newPosition;

		if (_facingDirection != newDirection)
		{
			InvertPhysicsNodes(_directionalPhysics);
			_facingDirection = newDirection;
		}
	}

	private static void InvertPhysicsNodes(Node parentNode)
	{
		foreach (Node child in parentNode.GetChildren())
		{
			if (child is Node2D node2D)
			{
				node2D.Position = new Vector2(-node2D.Position.X, node2D.Position.Y);
				node2D.Rotation = -node2D.Rotation;
				InvertPhysicsNodes(node2D);
			}
		}
	}
}
