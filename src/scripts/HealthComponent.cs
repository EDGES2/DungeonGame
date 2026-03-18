using System;
using Godot;

namespace Game.Components;

[GlobalClass]
public partial class HealthComponent : Node2D
{
	[Export] public float MaxHealth = 100.0f;
	private float _currentHealth;

	public override void _Ready()
	{
		_currentHealth = MaxHealth;
	}

	public void Damage(Attack attack)
	{
		_currentHealth -= attack.Damage;

		if (_currentHealth <= 0)
		{
			GetParent().QueueFree();
		}
	}
}
