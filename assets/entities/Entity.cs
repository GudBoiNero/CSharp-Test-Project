using Godot;
using System;
using System.Collections.Generic;


public partial class Entity : CharacterBody2D
{
    [ExportGroup("Collider")]
    [Export]
    private Vector2I colliderShape = new(0, 0);
    [Export]
    private Vector2I colliderOffset = new(0, 0);
    readonly private CollisionShape2D collider = new();

    [ExportGroup("Sprite")]
    [Export]
    private Texture2D texture;
    readonly private Sprite2D sprite = new();

    readonly private List<Force> forces = new();
    readonly private Force gravity = new(new(0,1f), initialSpeed: 440f, accel: 0.1f, maxSpeed: 1200f);

    public Entity() {}

    public override void _Ready() {
        collider.Shape = new RectangleShape2D();
        AddChild(collider);

        sprite.Texture = texture;
        AddChild(sprite);
    }

    public override void _PhysicsProcess(double delta)
    {
        Vector2 velocity = GetVelocity();
        velocity.X *= (float)delta;
        velocity.Y *= (float)delta;
        GD.Print(velocity);
        MoveAndCollide(velocity);
    }

    public void AddForce(Force force) {
        forces.Add(force);
    }

    public Vector2 GetVelocity() {
        Vector2 velocity = new();
        velocity += gravity.GetVelocity(tick: true);
        foreach (var force in forces)
        {
            velocity += force.GetVelocity(tick: true);
        }
        return velocity;
    }
}
 