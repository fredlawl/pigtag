using UnityEditor;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace BehaviorTree
{
    public class Node
    {
        public enum State
        {
            Running,
            Success,
            Failure
        }

        protected Node parent;
        protected List<Node> children = new List<Node>();

        private Dictionary<string, object> stateStore = new Dictionary<string, object>();

        public virtual State Evaluate() => State.Failure;

        public Node()
        {
            parent = null;
        }

        public Node(List<Node> children)
        {
            foreach (Node child in children)
            {
                AddChild(child);
            }
        }

        public void AddChild(Node child)
        {
            child.parent = this;
            children.Add(child);
        }

        public void SetData(string key, object value)
        {
            stateStore[key] = value;
        }

        public object GetData(string key)
        {
            object value = null;
            if (stateStore.TryGetValue(key, out value))
            {
                return value;
            }

            Node node = parent;
            while (node != null)
            {
                value = node.GetData(key);
                if (value != null)
                {
                    return value;
                }

                node = node.parent;
            }

            return null;
        }

        public bool ClearData(string key)
        {
            if (stateStore.ContainsKey(key))
            {
                return stateStore.Remove(key);
            }

            Node node = parent;
            while (node != null)
            {
                bool cleared = node.ClearData(key);
                if (cleared)
                {
                    return true;
                }

                node = node.parent;
            }

            return false;
        }

        protected Node GetRootNode()
        {
            var node = parent;
            while (node.parent != null)
            {
                node = node.parent;
            }

            return node;
        }
    }
}