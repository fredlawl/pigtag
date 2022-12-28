using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class Selectable : MonoBehaviour, IPointerClickHandler
{
    private static volatile GameObject currentlySelected = null;
    public UnityEvent<GameObject, SelectedState> onSelected;
    private bool isSelected = false;
    private SpriteRenderer spriteRender;

    /*
     * TODO: Figure out how to add a circle or square outline
     * material shader to the sprite without making it dissapear
     * :|
     */
    //private Shader defaultShader;
    //private Shader selectedShader;

    private void Start()
    {
        spriteRender = GetComponent<SpriteRenderer>();
        //selectedShader = Shader.Find("NewUnitShader");
        //defaultShader = spriteRender.material.shader;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PointerEventData.InputButton.Left == eventData.button)
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
            //spriteRender.material.shader = selectedShader;
        }

        if (PointerEventData.InputButton.Right == eventData.button && gameObject.Equals(currentlySelected))
        {
            onSelected.Invoke(gameObject, SelectedState.Deselected);
            currentlySelected = null;
            isSelected = false;
            //spriteRender.material.shader = defaultShader;
        }
    }

    public enum SelectedState : int
    {
        Selected = 1,
        Deselected = 2,
        RepeatedlySelected = 3
    }
}