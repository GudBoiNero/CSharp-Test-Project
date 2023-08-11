using Godot;
using System;

public static partial class Wait {
    public static void For(Node onWhat, float duration, Action onTimeout) {
        Timer timer = new() { WaitTime = duration, Autostart = true };
        onWhat.AddChild(timer);
        timer.Timeout += () => { onTimeout(); timer.QueueFree(); };
    }
}