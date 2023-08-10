using Godot;
using System;

public partial class Force : Resource
{
    public Vector2 direction;

    public double speed;
    public double initialSpeed;

    public double decay;
    public bool canDecay;

    public double accel;
    public bool canAccel;

    public float minSpeed;
    public float maxSpeed;

    public Force(
        Vector2 direction,
        float initialSpeed,
        double decay = 0.1, bool canDecay = false,
        double accel = 0.0, bool canAccel = false,
        float minSpeed = float.MinValue, float maxSpeed = float.MaxValue

    )
    {
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
        if (canDecay) speed -= speed * decay;
        if (canAccel) speed += speed * accel;
        speed = Math.Clamp(speed, minSpeed, maxSpeed);
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
