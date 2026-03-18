using System;
using System.Collections.Generic;
using Godot;

namespace Game.Components;

[GlobalClass]
public partial class Weapon : Node2D
{
	private Area2D _area;
	private readonly List<CollisionShape2D> _hitboxFrames = [];
	private readonly HashSet<HitboxComponent> _hitList = [];

	[Export] private float _damage = 25.0f;
	[Export] private float _knockback_force = 150.0f;

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
		if (area is HitboxComponent hitbox && !_hitList.Contains(hitbox))
		{
			_hitList.Add(hitbox);
			Attack attack = new()
			{
				damage = _damage,
				knockback_force = _knockback_force,
				attack_position = GlobalPosition
			};

			hitbox.Damage(attack);
		}
	}
	public void ResetAttack()
	{
		_hitList.Clear();
	}
}
