using Godot;
using System;

public class EntitySprite : Sprite
{
    private AnimationPlayer animationPlayer = null;

    public override void _Ready()
    {
        animationPlayer = GetNodeOrNull<AnimationPlayer>("EntityAnimator");
    }

    private void SetAnimation(Entity.State state, Entity.Cardinal direction)
    {
        switch (state) {
            case Entity.State.Walking:
            case Entity.State.Idle:
            {
                switch(direction) {
                    case Entity.Cardinal.North:
                    {
                        animationPlayer.CurrentAnimation = "Idle_Up";
                        break;
                    }
                    case Entity.Cardinal.South:
                    {
                        animationPlayer.CurrentAnimation = "Idle_Down";
                        break;
                    }
                    case Entity.Cardinal.East:
                    {
                        animationPlayer.CurrentAnimation = "Idle_Right";
                        break;
                    }
                    case Entity.Cardinal.West:
                    {
                        animationPlayer.CurrentAnimation = "Idle_Left";
                        break;
                    }
                }
                break;
            }
        }
    }

    public void onCharacterAnimationChange(Entity.State state, Entity.Cardinal direction)
    {
        SetAnimation(state, direction);
    }
}
