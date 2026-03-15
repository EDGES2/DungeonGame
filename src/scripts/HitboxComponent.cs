using System;
using Godot;

namespace Game.Components;

[GlobalClass]
public partial class HitboxComponent : Area2D
{
    [Export] public HealthComponent HealthComponent;

    public void Damage(Attack attack)
    {
        HealthComponent?.Damage(attack);
    }
}
