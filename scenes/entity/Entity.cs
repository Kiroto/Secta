using Godot;
using System;

public class Entity : KinematicBody2D
{

    PackedScene packedScene;

    [Signal]
    public delegate void AnimationChange(State state, Cardinal direction);
    public enum State
    {
        Walking,
        Idle,
        Attacking,
    }

    public enum Cardinal
    {
        North,
        South,
        East,
        West,
    }

    private const float atkLag = 0.2f;
    private const float atkLag2 = 5f;
    private float inputLag = 0f;

    private State currentState = State.Idle;
    private Stats stats = new Stats();
    private Vector2 motion = Vector2.Zero;
    private Controller controller = new Controller();

    public override void _Ready()
    {
        packedScene = (PackedScene)ResourceLoader.Load("res://SlamAttack.tscn");
        AddChild(controller);
    }

    private Cardinal getMoveCardinal()
    {
        if (Math.Abs(motion.x) > Math.Abs(motion.y)) {
            if (motion.x > 0) {
                return Cardinal.East;
            } else {
                return Cardinal.West;
            }
        } else {
            if (motion.y > 0) {
                return Cardinal.South;
            } else {
                return Cardinal.North;
            }
        }
    }
    
    private State ProcessButtons()
    {
        switch(NextAction())
        {
            case Controller.Action.Attack:
            {
                if (currentState != State.Attacking) {
                    currentState = State.Attacking;
                    inputLag = atkLag;
                    Node2D attack = (Node2D)packedScene.Instance();
                    AddChild(attack);
                } else {
                    inputLag = atkLag2;
                }
                return State.Attacking;
            }
        }
        return State.Idle;
    }


    public override void _PhysicsProcess(float delta)
    {
        Vector2 inputDirection = controller.GetDirection();
        switch(currentState)
        {
            case State.Idle: 
            {
                if (inputDirection != Vector2.Zero) {
                    currentState = State.Walking;
                }
                ProcessButtons();
                break;
            }
            case State.Walking:
            {
                if (inputDirection != Vector2.Zero) {
                    motion += inputDirection * stats.moveAcceleration;
                } else {
                    currentState = State.Idle;
                }
                ProcessButtons();
                break;
            }
            case State.Attacking:
            {
                inputLag -= delta;
                if (inputLag <= 0) {
                    currentState = ProcessButtons();
                }
                break;
            }
        }

        this.MoveAndSlide(motion);
        EmitSignal(nameof(AnimationChange), currentState, getMoveCardinal());
        motion *= stats.friction;
    }

    private Controller.Action NextAction()
    {
        Controller.Action consumedAction = Controller.Action.None;
        if (controller.inputBuffer.Count > 0 && controller.inputBuffer[0] != null) {
            consumedAction = controller.inputBuffer[0].Index;
            controller.ConsumeAction(consumedAction);
        }
        return consumedAction;
    }
}
