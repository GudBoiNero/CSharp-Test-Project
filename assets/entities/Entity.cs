using Godot;
using System;
using System.Collections.Generic;


public partial class Entity : CharacterBody2D
{
    [ExportGroup("Collider")]
    [Export]
    public Vector2 colliderShape = new(0, 0);
    [Export]
    public Vector2 colliderOffset = new(0, 0);
    readonly public CollisionShape2D collider = new();

    [ExportGroup("Sprite")]
    [Export]
    public Texture2D texture;
    [Export]
    public Vector2 spriteScale = new(1, 1);
    readonly public Sprite2D sprite = new();

    readonly public List<Force> forces = new();

    [Export]
    public Force gravity;

    public Entity() 
    { 
        gravity = new(new(0, 1f), initialSpeed: 40f, accel: 0.35f, canAccel: true, maxSpeed: 800f);
    }

    public override void _Ready()
    {
        RectangleShape2D colliderRect = new()
        {
            Size = colliderShape
        };

        collider.Shape = colliderRect;
        collider.Position = colliderOffset;
        collider.DebugColor = new(0.6f, 0.25f, 0.5f, 0.41f);
        collider.ZIndex = 1;
        AddChild(collider);

        sprite.Texture = texture;
        sprite.Scale = spriteScale;
        AddChild(sprite);
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
