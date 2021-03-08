using Godot;
using System;

public class Attack : Node2D
{
    AnimationPlayer animationPlayer;
    public override void _Ready()
    {
        animationPlayer = GetNode<AnimationPlayer>("AnimationPlayer");
    }
    public void startAttack()
    {
        animationPlayer.Play("Attack");
    }

    public void onAnimationDone()
    {
        Free();
    }
}
