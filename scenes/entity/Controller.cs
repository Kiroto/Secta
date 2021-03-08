using Godot;
using System.Collections.Generic;
class Controller : Node {

    private const float bufferTime = 5f;
    public enum Action
    {
        None,
        Attack,
        Dodge
    }

    public List<KeyVal<Action, float>> inputBuffer { get; } = new List<KeyVal<Action, float>>();

    public override void _Process(float delta)
    {
        TickBuffer(delta);
        RegisterActions(GetActions());
    }

    private void TickBuffer(float delta)
    {
        foreach(KeyVal<Action, float> kvp in inputBuffer)
        {
            if (kvp != null) {
                kvp.Value = kvp.Value - delta;
                if (kvp.Value <= 0f) {
                    inputBuffer.Remove(kvp);
                }
            }
        }
    }

    public Vector2 GetDirection()
    {
        Vector2 direction = new Vector2(0f, 0f);
        if (Input.IsActionPressed("ui_left")) {
            direction.x -= 1f;
        }
        if (Input.IsActionPressed("ui_right")) {
            direction.x += 1f;
        }
        if (Input.IsActionPressed("ui_up")) {
            direction.y -= 1f;
        }
        if (Input.IsActionPressed("ui_down")) {
            direction.y += 1f;
        }
        return direction.Normalized();
    }

    private List<Action> GetActions()
    {
        List<Action> actions = new List<Action>();
        if(Input.IsActionJustPressed("ui_accept")) {
            actions.Add(Action.Attack);
        }
        if(Input.IsActionJustPressed("ui_cancel")) {
            actions.Add(Action.Dodge);
        }
        return actions;
    }

    private void RegisterActions(List<Action> actions)
    {
        foreach (Action action in actions)
        {
            KeyVal<Action, float> keyval = new KeyVal<Action, float>(action, bufferTime);
            inputBuffer.Add(keyval);
        }
    }

    public void ConsumeAction(Action action)
    {
        foreach (var bufferedAction in inputBuffer)
        {
            if (bufferedAction.Index == action) {
                inputBuffer.Remove(bufferedAction);
                break;
            }
        }
    }
}