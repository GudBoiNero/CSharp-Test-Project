using Godot;
using System;
using System.Collections.Generic;


public partial class Entity : CharacterBody2D
{
    [Export]
    public Sprite2D sprite;
    [Export]
    public CollisionShape2D collider;

    readonly public List<Force> forces = new();
    readonly public Force gravity;

    public Entity() {
        gravity = new(this, new(0, 1f), initialSpeed: 20f, accel: 0.35f, canAccel: true, maxSpeed: 800f);
    }

    public override void _Ready()
    {
        if (!IsInstanceValid(sprite)) throw new("`sprite` is not a valid instance.");
        if (!IsInstanceValid(collider)) throw new("`collider` is not a valid instance.");
        else {
            collider.DebugColor = new(0.6f, 0.25f, 0.5f, 0.41f);
            collider.ZIndex = 1;
        }
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = GetVelocity();
        GD.Print(forces.Count);
        Velocity = velocity;
        MoveAndSlide();
    }

    public void AddForce(Force force)
    {
        forces.Add(force);
    }

    public virtual Vector2 GetVelocity()
    {
        Vector2 velocity = new();
        
        if (!IsOnFloor())
        {
            velocity += gravity.GetVelocity(tick: true);
        }
        else
        {
            gravity.ResetForce();
        };

        foreach (var force in forces)
        {
            Vector2 forceVel = force.GetVelocity(tick: true);
            velocity += forceVel;
        }
        return velocity;
    }
}
