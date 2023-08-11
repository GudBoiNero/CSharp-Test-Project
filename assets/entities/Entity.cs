using Godot;
using System;
using System.Collections.Generic;


public partial class Entity : RigidBody2D
{
    [Export]
    public Sprite2D sprite;
    [Export]
    public CollisionShape2D collider;

    readonly public List<Force> forces = new();

    private float gravity = 0.0f;
    readonly public float gravitySpeed = 600f;
    readonly public float gravityAccel = 0.10f;

    public Entity() {}

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
        gravity = Mathf.Lerp(gravity, gravitySpeed, gravityAccel);

        Wait.For(this, 0.5f, () => {});

        Vector2 velocity = GetVelocity();
        GD.Print(forces.Count);
        ApplyCentralForce(velocity);
    }

    public void AddForce(Force force)
    {
        forces.Add(force);
    }

    public float GetGravity() {
        return gravity;        
    }

    public void SetGravity(float gravity) {
        this.gravity = gravity;
    }

    public virtual Vector2 GetVelocity()
    {
        Vector2 velocity = new();
        
        velocity.Y += GetGravity();
        SetGravity(0);

        foreach (var force in forces)
        {
            Vector2 forceVel = force.GetVelocity(tick: true);
            velocity += forceVel;
        }
        return velocity;
    }
}
