using Godot;
using System;
using System.Collections.Generic;


public partial class Entity : CharacterBody2D
{
    [ExportGroup("Collider")]
    [Export]
    private Vector2 colliderShape = new(0, 0);
    [Export]
    private Vector2 colliderOffset = new(0, 0);
    readonly private CollisionShape2D collider = new();

    [ExportGroup("Sprite")]
    [Export]
    private Texture2D texture;
    [Export]
    private Vector2 spriteScale = new(1, 1);
    readonly private Sprite2D sprite = new();

    readonly private RayCast2D bottomRaycast = new();
    readonly private List<Force> forces = new();

    [Export(PropertyHint.Enum)]
    private Force gravity;

    public Entity() 
    { 
        gravity = new(new(0, 1f), initialSpeed: 40f, accel: 0.35f, canAccel: true, maxSpeed: 800f);
    }

    public override void _Ready()
    {
        collider.Shape = new RectangleShape2D();
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
        GD.Print(velocity);
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
            if (forceVel == Vector2.Zero) {
                forces.Remove(force);
            }
        }
        return velocity;
    }
}
