using UnityEngine;
public class DebugSelectable : MonoBehaviour
{
    public void OnSelected(GameObject obj, Selectable.SelectedState state)
    {
        Debug.Log("certainly clicked!");
        if (state.Equals(Selectable.SelectedState.Selected))
        {
            Debug.Log($"{obj.name} selected!");
        }

        if (state.Equals(Selectable.SelectedState.Deselected))
        {
            Debug.Log($"{obj.name} deselected!");
        }

        if (state.Equals(Selectable.SelectedState.RepeatedlySelected))
        {
            Debug.Log($"{obj.name} selected again!");
        }
    }
}