using UnityEngine;

using System.Collections.Generic;

public abstract class BTComposite : BTNode
{
    protected List<BTNode> children = new List<BTNode>();
    public BTComposite(params BTNode[] nodes) => children.AddRange(nodes);
}