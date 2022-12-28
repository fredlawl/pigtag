using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BehaviorTree
{
    public class Selector : Node
    {
        public Selector() : base() { }
        public Selector(List<Node> children) : base(children) { }

        public override State Evaluate()
        {
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case State.Failure:
                        continue;
                    case State.Success:
                        return State.Success;
                    case State.Running:
                        return State.Running;
                    default:
                        continue;
                }
            }

            return State.Failure;
        }
    }
}