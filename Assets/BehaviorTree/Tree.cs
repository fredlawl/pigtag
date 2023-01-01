using UnityEditor;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        protected Node root = null;

        private void Update()
        {
            root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}