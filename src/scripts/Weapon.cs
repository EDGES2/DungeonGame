using System;
using System.Collections.Generic;
using Godot;

namespace Game.Components;

[GlobalClass]
public partial class Weapon : Node2D
{
	private Area2D _area;
	private readonly List<CollisionShape2D> _hitboxFrames = [];
	public override void _Ready()
	{
		_area = GetNode<Area2D>("Range");
		_area.AreaEntered += OnAreaEntered;
		foreach (Node child in _area.GetChildren())
		{
			if (child is CollisionShape2D shape)
			{
				_hitboxFrames.Add(shape);
			}
		}

	}
	public void OnAreaEntered(Area2D area)
	{
		if (area is HitboxComponent hitbox)
		{
			Attack attack = new()
			{
				Damage = 25.0f,
				KnockbackForce = 150.0f,
				AttackPosition = GlobalPosition
			};

			hitbox.Damage(attack);
		}
	}
}
