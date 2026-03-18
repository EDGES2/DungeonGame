using System;
using Godot;

namespace Game.Components;

[GlobalClass]
public partial class HealthComponent : Node2D
{
	[Export] public float MaxHealth = 100.0f;
	[Export] private AnimationPlayer anim;

	private float _currentHealth;
	private bool _isDead = false;

	public override void _Ready()
	{
		_currentHealth = MaxHealth;
		anim.AnimationFinished += OnAnimationFinished;
	}

	public void Damage(Attack attack)
	{
		_currentHealth -= attack.Damage;
		anim.Play("take_hit");

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
