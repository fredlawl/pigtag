using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Enemy
{
    public class DummyPatrol : BehaviorTree.Node
    {
        public override State Evaluate()
        {
            return State.Running;
        }
    }
}
