using System;
using Godot;

namespace Game.Components;

[GlobalClass]
public partial class Attack : RefCounted
{
    [Export] public float damage = 10.0f;
    [Export] public float knockback_force = 100.0f;
    [Export] public Vector2 attack_position = Vector2.Zero;
    [Export] public float stunTime = 0.0f;
}
