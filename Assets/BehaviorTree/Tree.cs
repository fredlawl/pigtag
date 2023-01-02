using UnityEditor;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        protected Node root = null;

        protected virtual void Start()
        {
            root = SetupTree();
        }

        protected virtual void Update()
        {
            root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}