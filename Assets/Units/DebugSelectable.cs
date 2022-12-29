using UnityEngine;
using System;

[Serializable]
public class DebugSelectable : MonoBehaviour
{
    public void OnSelected(GameObject obj, Selectable.SelectedState state)
    {
        Debug.Log("certainly clicked!");
        if (state == Selectable.SelectedState.Selected)
        {
            Debug.Log($"{obj.name} selected!");
        }

        if (state == Selectable.SelectedState.Deselected)
        {
            Debug.Log($"{obj.name} deselected!");
        }

        if (state == Selectable.SelectedState.RepeatedlySelected)
        {
            Debug.Log($"{obj.name} selected again!");
        }
    }
}