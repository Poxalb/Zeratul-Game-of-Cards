using System;

public class BTAction : BTNode
{
    private Func<State> action;
    public BTAction(Func<State> action) => this.action = action;
    public override State Tick() => action();
}