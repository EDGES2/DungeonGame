using Godot;

namespace Game.Enemy.Muddy;

public partial class Muddy : CharacterBody2D
{
	[Export] public float friction = 500f;

	public override void _Ready()
	{
	}
	public override void _Process(double delta)
	{
	}
	public override void _PhysicsProcess(double delta)
	{
		Velocity = Velocity.MoveToward(Vector2.Zero, friction * (float)delta);

		MoveAndSlide();
	}
}
