using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour, IPointerClickHandler
{
    private static volatile GameObject currentlySelected = null;
    public UnityEvent<GameObject, SelectedState> onSelected;
    private bool isSelected = false;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("clicked!");
        if (((int) MouseButton.Primary).Equals(eventData.button))
        {
            if (isSelected)
            {
                onSelected.Invoke(gameObject, SelectedState.RepeatedlySelected);
                return;
            }

            onSelected.Invoke(gameObject, SelectedState.Selected);

            /*
             * TODO: Think of something else here. We need to let the previous
             * game object know that it was unselected. So look up the current
             * game objects handled selectable function and then invoke that?
             * how?
             */
            //onSelected.Invoke(currentlySelected, SelectedState.Deselected);
            currentlySelected = gameObject;
            isSelected = true;
        }

        if (((int) MouseButton.Secondary).Equals(eventData.button) && gameObject.Equals(currentlySelected))
        {
            onSelected.Invoke(gameObject, SelectedState.Deselected);
            currentlySelected = null;
            isSelected = false;
        }
    }

    public enum SelectedState : int
    {
        Selected = 1,
        Deselected = 2,
        RepeatedlySelected = 3
    }
}