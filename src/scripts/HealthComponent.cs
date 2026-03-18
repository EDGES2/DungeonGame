using System;
using System.Collections.Generic;
using Godot;

namespace Game.Components;

[GlobalClass]
public partial class HealthComponent : Node2D
{
	[Export] public float MaxHealth = 100.0f;
	[Export] private AnimationPlayer _anim;
	[Export] private CharacterBody2D _characterBody;

	private float _currentHealth;
	private bool _isDead = false;


	public override void _Ready()
	{
		_currentHealth = MaxHealth;
		_anim.AnimationFinished += OnAnimationFinished;
	}

	public void Damage(Attack attack)
	{
		_currentHealth -= attack.damage;
		_anim.Play("take_hit");
		_characterBody.Velocity = (GlobalPosition - attack.attack_position).Normalized() * attack.knockback_force;

		if (_currentHealth <= 0)
		{
			_isDead = true;
		}
	}
	private void OnAnimationFinished(StringName animName)
	{
		if (_isDead && animName == "take_hit")
		{
			GetParent().QueueFree();
		}
	}
}
