using Godot;
using System;

public class Attack : Node2D
{
    AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
        animationPlayer.Play("Attack");
        
    }

    public void _on_AnimationPlayer_animation_finished(string animation)
    {
        QueueFree();
    }
}
