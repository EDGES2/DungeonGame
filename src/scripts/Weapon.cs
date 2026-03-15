using System;
using Godot;

namespace Game.Components;

[GlobalClass]
public partial class Weapon : Node2D
{
    private Area2D _area;
    private CollisionShape2D _hitboxShape;
    public override void _Ready()
    {
        _area = GetNode<Area2D>("Range");
        _hitboxShape = _area.GetNode<CollisionShape2D>("CollisionShape2D");
        DisableWeapon();
        _area.AreaEntered += OnAreaEntered;

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
    public void EnableWeapon()
    {
        _hitboxShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, false);
    }

    public void DisableWeapon()
    {
        _hitboxShape.SetDeferred(CollisionShape2D.PropertyName.Disabled, true);
    }
}
