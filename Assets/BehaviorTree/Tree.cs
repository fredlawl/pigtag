using UnityEditor;
using UnityEngine;

namespace BehaviorTree
{
    public abstract class Tree : MonoBehaviour
    {
        private Node root = null;

        protected void Awake()
        {
            root = SetupTree();
        }

        protected void Start()
        {
            
        }

        private void Update()
        {
            root?.Evaluate();
        }

        protected abstract Node SetupTree();
    }
}