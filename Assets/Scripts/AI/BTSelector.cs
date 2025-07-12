public class BTSelector : BTComposite
{
    public BTSelector(params BTNode[] nodes) : base(nodes) { }
    public override State Tick()
    {
        foreach (var child in children)
        {
            var result = child.Tick();
            if (result != State.Failure)
                return result;
        }
        return State.Failure;
    }
}