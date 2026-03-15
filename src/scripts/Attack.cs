using System;
using Godot;

namespace Game.Components;

[GlobalClass]
public partial class Attack : RefCounted
{
    [Export] public float Damage = 10.0f;
    [Export] public float KnockbackForce = 100.0f;
    [Export] public Vector2 AttackPosition = Vector2.Zero;
    [Export] public float StunTime = 0.0f;
}
