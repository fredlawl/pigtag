using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Concurrent;

public class Selectable : MonoBehaviour, IPointerClickHandler
{

    private static volatile GameObject currentlySelected = null;
    private static readonly ConcurrentDictionary<GameObject, byte> selected = new ConcurrentDictionary<GameObject, byte>();

    public UnityEvent<GameObject, SelectedState> onSelected;
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

    public bool IsSelected => gameObject.Equals(currentlySelected);

    public void OnPointerClick(PointerEventData eventData)
    {
        if (PointerEventData.InputButton.Left == eventData.button)
        {
            Select();
        }

        if (PointerEventData.InputButton.Right == eventData.button && IsSelected)
        {
            Disselect();
        }
    }

    public void Select()
    {
        if (IsSelected)
        {
            onSelected?.Invoke(gameObject, SelectedState.RepeatedlySelected);
            return;
        }

        onSelected?.Invoke(gameObject, SelectedState.Selected);
        //selected.AddOrUpdate(gameObject, 1);

        currentlySelected?.GetComponent<Selectable>()?.Disselect();
        currentlySelected = gameObject;
        //spriteRender.material.shader = selectedShader;
    }

    public void Disselect()
    {
        onSelected?.Invoke(gameObject, SelectedState.Deselected);
        currentlySelected = null;
        //spriteRender.material.shader = defaultShader;
    }

    public enum SelectedState : int
    {
        Selected = 1,
        Deselected = 2,
        RepeatedlySelected = 3
    }
}