using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace BehaviorTree
{
    public class Sequence : Node
    {
        public Sequence() : base() { }
        public Sequence(List<Node> children) : base(children) { }

        public override State Evaluate()
        {
            bool anyChildIsRunning = false;
            foreach (Node node in children)
            {
                switch (node.Evaluate())
                {
                    case State.Failure:
                        return State.Failure;
                    case State.Success:
                        continue;
                    case State.Running:
                        anyChildIsRunning = true;
                        continue;
                    default:
                        return State.Success;
                }
            }

            return anyChildIsRunning ? State.Running : State.Success;
        }
    }
}