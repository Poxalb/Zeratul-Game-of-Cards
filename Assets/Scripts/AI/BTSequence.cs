public class BTSequence : BTComposite
{
    public BTSequence(params BTNode[] nodes) : base(nodes) { }
    
    public override State Tick()
    {
        foreach (var child in children)
        {
            var result = child.Tick();
            if (result != State.Success)
                return result;
        }
        return State.Success;
    }
}
