using Godot;
using System;

public partial class Force : Resource
{
    public Entity entity;
    public Vector2 direction;

    public double speed;
    public double initialSpeed;

    public double decay;
    public bool canDecay;

    public double accel;
    public bool canAccel;

    public float minSpeed;
    public float maxSpeed;
    
    public bool active = true;

    public Force(
        Entity entity,
        Vector2 direction,
        float initialSpeed,
        double decay = 0.0, bool canDecay = false,
        double accel = 0.0, bool canAccel = false,
        float minSpeed = float.MinValue, float maxSpeed = float.MaxValue
    )
    {
        this.entity = entity;
        this.direction = direction;
        this.initialSpeed = initialSpeed;
        this.decay = decay;
        this.canDecay = canDecay;
        this.accel = accel;
        this.canAccel = canAccel;
        this.minSpeed = minSpeed;
        this.maxSpeed = maxSpeed;

        speed = initialSpeed;
    }

    private void Tick()
    {
        if (ShouldDisable()) active = false;
        if (!active) 
        {
            speed = 0.0;
            return;
        }

        if (canDecay) speed -= speed * decay;
        if (canAccel) speed += speed * accel;
        speed = Math.Clamp(speed, minSpeed, maxSpeed);
    }

    public virtual bool ShouldDisable()
    {
        return false;
    }

    public void ResetForce()
    {
        speed = initialSpeed;
    }

    public Vector2 GetVelocity(bool tick = false, bool normalized = true)
    {
        Vector2 velocity = new(direction.X, direction.Y);
        if (normalized) velocity = velocity.Normalized();
        velocity.X *= (float)speed;
        velocity.Y *= (float)speed;

        if (tick) Tick();

        return velocity;
    }
}
